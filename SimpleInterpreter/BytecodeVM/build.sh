#!/bin/bash

# Set directories
SRC_DIR="src"
OBJ_DIR="obj"
BIN_DIR="bin"

# Create directories if they don't exist
mkdir -p $OBJ_DIR
mkdir -p $BIN_DIR

# Set the compiler and linker
CC=gcc
CFLAGS="-g -c -I$SRC_DIR -Wall -Wextra -pedantic -std=c11 -DNDEBUG"
LDFLAGS="-o $BIN_DIR/simp"

# Compile each .c file to an object file
for src_file in $SRC_DIR/*.c; do
    obj_file="$OBJ_DIR/$(basename ${src_file%.c}.o)"
    compile_cmd="$CC $CFLAGS -o $obj_file $src_file"
    echo "Compiling $src_file..."
    echo $compile_cmd
    $compile_cmd
    if [ $? -ne 0 ]; then
        echo "Compilation failed for $src_file!"
        exit 1
    fi
done

# Link all object files to create the binary
echo "Linking..."
link_cmd="$CC $LDFLAGS $OBJ_DIR/*.o"
echo $link_cmd
$link_cmd
if [ $? -ne 0 ]; then
    echo "Linking failed!"
    exit 1
fi

echo "Build succeeded!"
