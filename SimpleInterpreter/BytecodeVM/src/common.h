#ifndef simp_common_h
#define simp_common_h

#include <stdbool.h>
#include <stddef.h>
#include <stdint.h>

#ifdef NDEBUG
#undef DEBUG_PRINT_CODE
#undef DEBUG_TRACE_EXECUTION
#else
#define DEBUG_PRINT_CODE
#define DEBUG_TRACE_EXECUTION
#endif

#define U8_COUNT (UINT8_MAX + 1)

typedef int8_t i8;
typedef int16_t i16;
typedef int32_t i32;
typedef int64_t i64;

typedef uint8_t u8;
typedef uint16_t u16;
typedef uint32_t u32;
typedef uint64_t u64;

typedef size_t usize;

typedef i8 b8;
typedef i16 b16;
typedef i32 b32;
typedef i64 b64;

typedef float f32;
typedef double f64;

#endif
