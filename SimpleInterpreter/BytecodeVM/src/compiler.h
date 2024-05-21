#ifndef simp_compiler_h
#define simp_compiler_h

#include "vm.h"

bool compiler$compile(const char *source, chunk_t *chunk);

#endif
