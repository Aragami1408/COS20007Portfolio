#ifndef simp_chunk_h
#define simp_chunk_h

#include "common.h"
#include "value.h"

typedef enum {
  OP_CONSTANT,
  OP_NIL,
  OP_TRUE,
  OP_FALSE,
  OP_POP,
  OP_GET_LOCAL,
  OP_SET_LOCAL,
  OP_GET_GLOBAL,
  OP_DEFINE_GLOBAL,
  OP_SET_GLOBAL,
  OP_EQUAL,
  OP_GREATER,
  OP_LESS,
  OP_ADD,
  OP_SUBTRACT,
  OP_MULTIPLY,
  OP_DIVIDE,
  OP_NOT,
  OP_NEGATE,
  OP_PRINT,
  OP_RETURN
} opcode_t;

typedef struct {
  usize count;
  usize capacity;
  u8* code;
  usize* lines;
  value_array_t constants;
} chunk_t;

void chunk$init(chunk_t *chunk);
void chunk$free(chunk_t *chunk);
void chunk$write(chunk_t *chunk, u8 byte, usize line);
int chunk$add_constant(chunk_t *chunk, value_t value);

#endif
