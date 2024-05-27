#ifndef simp_table_h
#define simp_table_h

#include "common.h"
#include "value.h"

typedef struct {
  obj_string_t* key;
  value_t value;
} entry_t;

typedef struct {
  usize count;
  usize capacity;
  entry_t* entries;
} table_t;

void table$init(table_t* table);
void table$free(table_t* table);
bool table$get(table_t* table, obj_string_t* key, value_t* value);
bool table$set(table_t* table, obj_string_t* key, value_t value);
bool table$delete(table_t* table, obj_string_t* key);
void table$add_all(table_t* from, table_t* to);
obj_string_t* table$find_string(table_t* table, const char* chars, int length, u32 hash);


#endif
