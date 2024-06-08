#ifndef simp_compiler_h
#define simp_compiler_h

#include "object.h"
#include "vm.h"

obj_function_t* compiler$compile(const char *source);

#endif
