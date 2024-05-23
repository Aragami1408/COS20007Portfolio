#include <stdio.h>
#include <stdarg.h>
#include <string.h>

#include "vm.h"
#include "chunk.h"
#include "debug.h"
#include "compiler.h"
#include "memory.h"
#include "object.h"

vm_t vm;

static value_t peek(int distance) {
  return vm.stack_top[-1 - distance];
}

static bool is_falsey(value_t value) {
  return IS_NIL(value) || (IS_BOOL(value) && !AS_BOOL(value));
}

static void concatenate() {
  obj_string_t* b = AS_STRING(vm$pop());
  obj_string_t* a = AS_STRING(vm$pop());

  int length = a->length + b->length;
  char* chars = ALLOCATE(char, length + 1);
  memcpy(chars, a->chars, a->length);
  memcpy(chars + a->length, b->chars, b->length);
  chars[length] = '\0';

  obj_string_t* result = take_string(chars, length);
  vm$push(OBJ_VAL(result));
}

static void reset_stack() {
  vm.stack_top = vm.stack;
}

static void runtime_error(const char *format, ...) {
  va_list args;
  va_start(args, format);
  vfprintf(stderr, format, args);
  va_end(args);
  fputs("\n", stderr);

  size_t instruction = vm.ip - vm.chunk->code - 1;
  int line = vm.chunk->lines[instruction];
  fprintf(stderr, "[line %d] in script\n", line);
  reset_stack();
}

static interpret_result_t run() {
#define READ_BYTE() (*vm.ip++)
#define READ_CONSTANT() (vm.chunk->constants.values[READ_BYTE()])
#define BINARY_OP(value_type, op) \
  do { \
    if (!IS_NUMBER(peek(0)) || !IS_NUMBER(peek(1))) { \
      runtime_error("Opearands must be numbers"); \
      return INTERPRET_RUNTIME_ERROR; \
    }\
    double b = AS_NUMBER(vm$pop());\
    double a = AS_NUMBER(vm$pop());\
    vm$push(value_type(a op b)); \
  } while (false)

#ifdef DEBUG_TRACE_EXECUTION
  printf("=== debug ===");
#endif

  for (;;) {
#ifdef DEBUG_TRACE_EXECUTION
    printf("          ");
    for (value_t* slot = vm.stack; slot < vm.stack_top; slot++) {
      printf("[ ");
      print_value(*slot);
      printf(" ]");
    }
    printf("\n");
    disassemble_instruction(vm.chunk, (int)(vm.ip - vm.chunk->code));
#endif
    u8 instruction;
    switch (instruction = READ_BYTE()) {
      case OP_CONSTANT: {
        value_t constant = READ_CONSTANT();
        vm$push(constant);
        break;
      }

      case OP_NIL:    vm$push(NIL_VAL); break;
      case OP_TRUE:   vm$push(BOOL_VAL(true)); break;
      case OP_FALSE:  vm$push(BOOL_VAL(false)); break;
      case OP_EQUAL: {
        value_t b = vm$pop();
        value_t a = vm$pop();
        vm$push(BOOL_VAL(values_equal(a,b)));
        break;
      }
      case OP_GREATER:  BINARY_OP(BOOL_VAL, >); break;
      case OP_LESS:     BINARY_OP(BOOL_VAL, <); break;
      case OP_ADD: {
        if (IS_STRING(peek(0)) && IS_STRING(peek(1)))
          concatenate();
        else if (IS_NUMBER(peek(0)) && IS_NUMBER(peek(1))) {
          double b = AS_NUMBER(vm$pop());
          double a = AS_NUMBER(vm$pop());
          vm$push(NUMBER_VAL(a+b));
        }
        else {
          runtime_error("Opearands must be two numbers or two strings");
          return INTERPRET_RUNTIME_ERROR;
        }
        break;
      }
      case OP_SUBTRACT: BINARY_OP(NUMBER_VAL, -); break;
      case OP_MULTIPLY: BINARY_OP(NUMBER_VAL, *); break;
      case OP_DIVIDE:   BINARY_OP(NUMBER_VAL, /); break;
      case OP_NOT:
        vm$push(BOOL_VAL(is_falsey(vm$pop())));
        break;
      case OP_NEGATE:
        if (!IS_NUMBER(peek(0))) {
          runtime_error("Operand must be a number");
          return INTERPRET_RUNTIME_ERROR;
        }
        vm$push(NUMBER_VAL(-AS_NUMBER(vm$pop())));
        break;

      case OP_RETURN: {
        print_value(vm$pop());
        printf("\n");
        return INTERPRET_OK;
      }
    }
  }

#undef READ_BYTE
#undef READ_CONSTANT
#undef BINARY_OP
}


void vm$init() {
  reset_stack();
  vm.objects = NULL;
}

void vm$free() {
  free_objects();
}

interpret_result_t vm$interpret(const char *source) {
  chunk_t chunk;
  chunk$init(&chunk);

  if (!compiler$compile(source, &chunk)) {
    chunk$free(&chunk);
    return INTERPRET_COMPILE_ERROR;
  }

  vm.chunk = &chunk;
  vm.ip = vm.chunk->code;

  interpret_result_t result = run();

  chunk$free(&chunk);
  return result;
}

void vm$push(value_t value) {
  *vm.stack_top = value;
  vm.stack_top++;
}

value_t vm$pop() {
  vm.stack_top--;
  return *vm.stack_top;
}
