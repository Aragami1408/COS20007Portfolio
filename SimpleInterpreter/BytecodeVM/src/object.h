#ifndef simp_object_h
#define simp_object_h

#include "common.h"
#include "value.h"
#include "chunk.h"

#define OBJ_TYPE(value)     (AS_OBJ(value)->type)

#define IS_FUNCTION(value)  is_obj_type(value, OBJ_FUNCTION)
#define IS_NATIVE(value)    is_obj_type(value, OBJ_NATIVE)
#define IS_STRING(value)    is_obj_type(value, OBJ_STRING)

#define AS_FUNCTION(value)  ((obj_function_t*)AS_OBJ(value))
#define AS_NATIVE(value) \
    (((obj_native_t*)AS_OBJ(value))->function)
#define AS_STRING(value)    ((obj_string_t*)AS_OBJ(value))
#define AS_CSTRING(value)   (((obj_string_t*)AS_OBJ(value))->chars)

typedef enum {
  OBJ_FUNCTION,
  OBJ_NATIVE,
  OBJ_STRING,
} obj_type_t;

struct obj {
  obj_type_t type;
  struct obj* next;
};

typedef struct {
  obj_t obj;
  int arity;
  chunk_t chunk;
  obj_string_t *name;
} obj_function_t;

typedef value_t (*native_fn_t)(int argCount, value_t* args);

typedef struct {
  obj_t obj;
  native_fn_t function;
} obj_native_t;

struct obj_string {
  obj_t obj;
  int   length;
  char* chars;
  u32 hash;
};

obj_function_t* new_function();
obj_native_t* new_native(native_fn_t function);
obj_string_t* take_string(char* chars, int length);
obj_string_t* copy_string(const char* chars, int length);
void print_function(obj_function_t *function);
void print_object(value_t value);

static inline bool is_obj_type(value_t value, obj_type_t type) {
  return IS_OBJ(value) && AS_OBJ(value)->type == type;
}

#endif
