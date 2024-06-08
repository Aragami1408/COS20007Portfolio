#include <stdlib.h>

#include "chunk.h"
#include "memory.h"

void chunk$init(chunk_t *chunk) {
  chunk->count = 0;
  chunk->capacity = 0;
  chunk->code = NULL;
  chunk->lines = NULL;
  value_array$init(&chunk->constants);
}

void chunk$free(chunk_t *chunk) {
  FREE_ARRAY(u8, chunk->code, chunk->capacity);
  FREE_ARRAY(usize, chunk->lines, chunk->capacity);
  value_array$free(&chunk->constants);
  chunk$init(chunk);
}

void chunk$write(chunk_t *chunk, u8 byte, usize line) {
  if (chunk->capacity < chunk->count + 1) {
    int old_capacity = chunk->capacity;
    chunk->capacity = GROW_CAPACITY(old_capacity);
    chunk->code = GROW_ARRAY(u8, chunk->code, old_capacity, chunk->capacity);
    chunk->lines = GROW_ARRAY(usize, chunk->lines, old_capacity, chunk->capacity);
  }

  chunk->code[chunk->count] = byte;
  chunk->lines[chunk->count] = line;
  chunk->count++;
}

int chunk$add_constant(chunk_t *chunk, value_t value) {
  value_array$write(&chunk->constants, value);
  return chunk->constants.count - 1;
}
