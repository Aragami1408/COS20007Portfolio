#ifndef simp_chunk_h
#define simp_chunk_h

#include "common.h"
#include "value.h"

typedef enum {
  OP_CONSTANT,
  OP_ADD,
  OP_SUBTRACT,
  OP_MULTIPLY,
  OP_DIVIDE,
  OP_NEGATE,
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
