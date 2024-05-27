#include <stdio.h>
#include <string.h>

#include "memory.h"
#include "object.h"
#include "value.h"
#include "vm.h"
#include "table.h"

#define ALLOCATE_OBJ(type, object_type) \
  (type*)allocate_object(sizeof(type), object_type);

static obj_t* allocate_object(usize size, obj_type_t type) {
  obj_t* object = (obj_t*)reallocate(NULL, 0, size);
  object->type = type;

  object->next = vm.objects;
  vm.objects = object;
  return object;
}

static obj_string_t* allocate_string(char* chars, int length, u32 hash) {
  obj_string_t* string = ALLOCATE_OBJ(obj_string_t, OBJ_STRING);
  string->length = length;
  string->chars = chars;
  string->hash = hash;
  table$set(&vm.strings, string, NIL_VAL);
  return string;
}

static u32 hash_string(const char* key, int length) {
  u32 hash = 216613626u;
  for (usize i = 0; i < length; i++) {
    hash ^= (u8)key[i];
    hash *= 16777619;
  }
  return hash;
}

obj_string_t* copy_string(const char* chars, int length) {
  u32 hash = hash_string(chars, length);
  obj_string_t* interned = table$find_string(&vm.strings, chars, length, hash);

  if (interned != NULL) return interned;

  char *heap_chars = ALLOCATE(char, length + 1);
  memcpy(heap_chars, chars, length);
  heap_chars[length] = '\0';
  return allocate_string(heap_chars, length, hash);
}

obj_string_t* take_string(char* chars, int length) {
  u32 hash = hash_string(chars, length);
  obj_string_t *interned = table$find_string(&vm.strings, chars, length, hash);

  if (interned != NULL) {
    FREE_ARRAY(char, chars, length + 1);
    return interned;
  }

  return allocate_string(chars, length, hash);
}

void print_object(value_t value) {
  switch (OBJ_TYPE(value)) {
  case OBJ_STRING:
    printf("%s", AS_CSTRING(value));
    break;
  }
}
