#include "common.h"
#include "chunk.h"
#include "debug.h"

int main(int argc, const char* argv[]) {
  chunk_t chunk;
  chunk$init(&chunk);

  int constant = chunk$add_constant(&chunk, 1.2);
  chunk$write(&chunk, OP_CONSTANT, 123);
  chunk$write(&chunk, constant, 123);

  chunk$write(&chunk, OP_RETURN, 123);

  disassemble_chunk(&chunk, "test chunk");
  chunk$free(&chunk);
  return 0;
}
