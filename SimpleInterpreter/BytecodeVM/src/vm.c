#include <stdio.h>
#include <stdarg.h>
#include <string.h>
#include <time.h>

#include "vm.h"
#include "chunk.h"
#include "debug.h"
#include "compiler.h"
#include "memory.h"
#include "object.h"

vm_t vm;

static value_t clock_native(int arg_count, value_t *args) {
  return NUMBER_VAL((double)clock() / CLOCKS_PER_SEC);
}

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
  vm.frame_count = 0;
}

static void runtime_error(const char *format, ...) {
  va_list args;
  va_start(args, format);
  vfprintf(stderr, format, args);
  va_end(args);
  fputs("\n", stderr);

  for (int i = vm.frame_count - 1; i >= 0; i--) {
    call_frame_t* frame = &vm.frames[i];
    obj_function_t* function = frame->function;
    size_t instruction = frame->ip - function->chunk.code - 1;
    fprintf(stderr, "[line %zu] in ", 
            function->chunk.lines[instruction]);
    if (function->name == NULL) {
      fprintf(stderr, "script\n");
    } else {
      fprintf(stderr, "%s()\n", function->name->chars);
    }
  }

  reset_stack();
}

static void define_native(const char *name, native_fn_t function) {
  vm$push(OBJ_VAL(copy_string(name, (int)strlen(name))));
  vm$push(OBJ_VAL(new_native(function)));
  table$set(&vm.globals, AS_STRING(vm.stack[0]), vm.stack[1]);
  vm$pop();
  vm$pop();
}

static bool call(obj_function_t *function, int arg_count) {
  if (arg_count != function->arity) {
    runtime_error("Expected %d arguments but got %d.", function->arity, arg_count);
    return false;
  }

  if (vm.frame_count == FRAMES_MAX) {
    runtime_error("Stack overflow");
    return false;
  }

  call_frame_t *frame = &vm.frames[vm.frame_count++];
  frame->function = function;
  frame->ip = function->chunk.code;
  frame->slots = vm.stack_top - arg_count - 1;
  return true;
}

static bool call_value(value_t callee, int arg_count) {
  if (IS_OBJ(callee)) {
    switch (OBJ_TYPE(callee)) {
      case OBJ_FUNCTION:
        return call(AS_FUNCTION(callee), arg_count);
      case OBJ_NATIVE: {
        native_fn_t native = AS_NATIVE(callee);
        value_t result = native(arg_count, vm.stack_top - arg_count);
        vm.stack_top -= arg_count + 1;
        vm$push(result);
        return true;
      }
      default:
        break;
    }
  }
  runtime_error("Can only call functions");
  return false;
}

static interpret_result_t run() {
  call_frame_t *frame = &vm.frames[vm.frame_count - 1];

#define READ_BYTE() (*frame->ip++)
#define READ_CONSTANT() \
  (frame->function->chunk.constants.values[READ_BYTE()])
#define READ_SHORT() \
    (frame->ip += 2, \
    (uint16_t)((frame->ip[-2] << 8) | frame->ip[-1]))
#define READ_STRING() AS_STRING(READ_CONSTANT())
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
    disassemble_instruction(&frame->function->chunk, (int)(frame->ip - frame->function->chunk.code));
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
      case OP_POP: vm$pop(); break;
      case OP_GET_LOCAL: {
        u8 slot = READ_BYTE();
        vm$push(frame->slots[slot]);
        break;
      }
      case OP_SET_LOCAL: {
        u8 slot = READ_BYTE();
        frame->slots[slot] = peek(0);
        break;
      }
      case OP_GET_GLOBAL: {
        obj_string_t *name = READ_STRING();
        value_t value;
        if (!table$get(&vm.globals, name, &value)) {
          runtime_error("Undefined variable '%s'.", name->chars);
          return INTERPRET_RUNTIME_ERROR;
        }
        vm$push(value);
        break;
      }
      case OP_DEFINE_GLOBAL: {
        obj_string_t *name = READ_STRING();
        table$set(&vm.globals, name, peek(0));
        vm$pop();
        break;
      }
      case OP_SET_GLOBAL: {
        obj_string_t* name = READ_STRING();
        if (table$set(&vm.globals, name, peek(0))) {
          table$delete(&vm.globals, name);
          runtime_error("Undefined variable '%s'.", name->chars);
          return INTERPRET_RUNTIME_ERROR;
        }
        break;
      }
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

      case OP_PRINT:
        print_value(vm$pop());
        printf("\n");
        break;

      case OP_JUMP: {
        u16 offset = READ_SHORT();
        frame->ip += offset;
        break;
      }

      case OP_JUMP_IF_FALSE: {
        u16 offset = READ_SHORT();
        if (is_falsey(peek(0))) frame->ip += offset;
        break;
      }

      case OP_LOOP: {
        u16 offset = READ_SHORT();
        frame->ip -= offset;
        break;
      }

      case OP_CALL: {
        int arg_count = READ_BYTE();
        if (!call_value(peek(arg_count), arg_count)) {
          return INTERPRET_RUNTIME_ERROR;
        }
        frame = &vm.frames[vm.frame_count - 1];
        break;
      }

      case OP_RETURN: {
        value_t result = vm$pop();
        vm.frame_count--;
        if (vm.frame_count == 0) {
          vm$pop();
          return INTERPRET_OK;
        }

        vm.stack_top = frame->slots;
        vm$push(result);
        frame = &vm.frames[vm.frame_count - 1];
        break;
      }
    }
  }

#undef READ_BYTE
#undef READ_CONSTANT
#undef READ_SHORT
#undef READ_STRING
#undef BINARY_OP
}


void vm$init() {
  reset_stack();
  vm.objects = NULL;
  table$init(&vm.globals);
  table$init(&vm.strings);

  define_native("clock", clock_native);
}

void vm$free() {
  table$free(&vm.globals);
  table$free(&vm.strings);
  free_objects();
}

interpret_result_t vm$interpret(const char *source) {
  obj_function_t *function = compiler$compile(source);
  if (function == NULL) return INTERPRET_COMPILE_ERROR;
  
  vm$push(OBJ_VAL(function));
  call(function, 0);  

  return run();

}

void vm$push(value_t value) {
  *vm.stack_top = value;
  vm.stack_top++;
}

value_t vm$pop() {
  vm.stack_top--;
  return *vm.stack_top;
}
