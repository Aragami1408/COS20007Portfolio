#ifndef simp_value_h
#define simp_value_h

#include "common.h"

#include <stdio.h>

typedef double value_t;

typedef struct {
  int capacity;
  int count;
  value_t* values;
} value_array_t;

void value_array$init(value_array_t *array);
void value_array$write(value_array_t *array, value_t value);
void value_array$free(value_array_t *array);

void print_value(value_t value);

#endif
