#ifndef simp_vm_h
#define simp_vm_h

#include "chunk.h"
#include "value.h"
#include "table.h"
#include "object.h"

#define FRAMES_MAX 64
#define STACK_MAX (FRAMES_MAX * U8_COUNT)

typedef struct {
  obj_function_t *function;
  u8 *ip;
  value_t *slots;
} call_frame_t;

typedef enum {
  INTERPRET_OK,
  INTERPRET_COMPILE_ERROR,
  INTERPRET_RUNTIME_ERROR
} interpret_result_t;

typedef struct {
  call_frame_t frames[FRAMES_MAX];
  int frame_count;

  chunk_t* chunk;
  u8* ip;
  value_t stack[STACK_MAX];
  value_t* stack_top;
  table_t globals;
  table_t strings;
  obj_t* objects;
} vm_t;

extern vm_t vm;

void vm$init();
void vm$free();

interpret_result_t vm$interpret(const char* source);
void vm$push(value_t value);
value_t vm$pop();

#endif
