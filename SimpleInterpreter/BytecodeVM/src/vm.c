#include <stdio.h>
#include "vm.h"
#include "chunk.h"
#include "debug.h"
#include "compiler.h"

vm_t vm;

static interpret_result_t run() {
#define READ_BYTE() (*vm.ip++)
#define READ_CONSTANT() (vm.chunk->constants.values[READ_BYTE()])
#define BINARY_OP(op) \
  do { \
    double b = vm$pop(); \
    double a = vm$pop(); \
    vm$push(a op b); \
  } while (false)

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

      case OP_ADD:      BINARY_OP(+); break;
      case OP_SUBTRACT: BINARY_OP(-); break;
      case OP_MULTIPLY: BINARY_OP(*); break;
      case OP_DIVIDE:   BINARY_OP(/); break;
      case OP_NEGATE:   vm$push(-vm$pop()); break;

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

static void reset_stack() {
  vm.stack_top = vm.stack;
}

void vm$init() {
  reset_stack();
}

void vm$free() {

}

interpret_result_t vm$interpret(const char *source) {
  compiler$compile(source);
  return INTERPRET_OK;
}

void vm$push(value_t value) {
  *vm.stack_top = value;
  vm.stack_top++;
}

value_t vm$pop() {
  vm.stack_top--;
  return *vm.stack_top;
}
