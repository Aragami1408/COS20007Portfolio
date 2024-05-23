#ifndef simp_vm_h
#define simp_vm_h

#include "chunk.h"
#include "value.h"

#define STACK_MAX 256

typedef enum {
  INTERPRET_OK,
  INTERPRET_COMPILE_ERROR,
  INTERPRET_RUNTIME_ERROR
} interpret_result_t;

typedef struct {
  chunk_t* chunk;
  u8* ip;
  value_t stack[STACK_MAX];
  value_t* stack_top;
  obj_t* objects;
} vm_t;

extern vm_t vm;

void vm$init();
void vm$free();

interpret_result_t vm$interpret(const char* source);
void vm$push(value_t value);
value_t vm$pop();

#endif
