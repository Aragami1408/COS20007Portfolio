#include <stdlib.h>

#include "memory.h"
#include "vm.h"

static void free_object(obj_t* object) {
  switch (object->type) {
    case OBJ_STRING: {
      obj_string_t* string = (obj_string_t*)object;
      FREE_ARRAY(char, string->chars, string->length + 1);
      FREE(obj_string_t, object);
      break;
    }
    case OBJ_NATIVE: {
      FREE(obj_native_t, object);
      break;
    } 
    case OBJ_FUNCTION: {
      obj_function_t *function = (obj_function_t*)object;
      chunk$free(&function->chunk);
      FREE(obj_function_t, object);
      break;
    }
  }
}

void *reallocate(void *pointer, usize old_size, usize new_size) {
  if (new_size == 0) {
    free(pointer);
    return NULL;
  }

  void *result = realloc(pointer, new_size);
  if (result == NULL) exit(1);
  return result;
}

void free_objects() {
  obj_t* object = vm.objects;
  while (object != NULL) {
    obj_t* next = object->next;
    free_object(object);
    object = next;
  }
}
