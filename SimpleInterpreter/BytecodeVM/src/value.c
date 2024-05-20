#include "value.h"
#include "memory.h"

void value_array$init(value_array_t *array) {
  array->values = NULL;
  array->capacity = 0;
  array->count = 0;
}
void value_array$write(value_array_t *array, value_t value) {

  if (array->capacity < array->count + 1) {
    int old_capacity = array->capacity;
    array->capacity = GROW_CAPACITY(old_capacity);
    array->values = GROW_ARRAY(u8, array->values, old_capacity, array->capacity);
  }

  array->values[array->count] = value;
  array->count++;
}

void value_array$free(value_array_t *array) {
  FREE_ARRAY(u8, array->values, array->capacity);
  value_array$init(array);
}

void print_value(value_t value) {
  printf("%g", value);
}
