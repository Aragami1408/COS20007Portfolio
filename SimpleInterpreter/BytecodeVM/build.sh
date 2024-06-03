#!/bin/bash

# Compiler
CC=gcc

# Directories
SRC_DIR="src"
OBJ_DIR="obj"
BIN_DIR="bin"

# Compilation flags
DEBUG_FLAGS="-ggdb -O0"
RELEASE_FLAGS="-O2 -DNDEBUG"

# Output executable name
DEBUG_EXE="simpc_debug"
RELEASE_EXE="simpc_release"

# Function to compile source files
compile() {
    mkdir -p "$OBJ_DIR" "$BIN_DIR"
    local FLAGS=$1

    # Compile each source file into an object file
    for SRC_FILE in $SRC_DIR/*.c; do
        OBJ_FILE="$OBJ_DIR/$(basename "${SRC_FILE%.c}.o")"
        echo $CC -c $FLAGS "$SRC_FILE" -o "$OBJ_FILE"
        $CC -c $FLAGS "$SRC_FILE" -o "$OBJ_FILE"
        if [ $? -ne 0 ]; then
            echo "Compilation failed for $SRC_FILE"
            exit 1
        fi
    done
}

# Function to link object files into executable
link() {
    local FLAGS=$1
    local EXE_PROFILE=$2
    echo $CC $OBJ_DIR/*.o -o "$BIN_DIR/$EXE_PROFILE" $FLAGS
    $CC $OBJ_DIR/*.o -o "$BIN_DIR/$EXE_PROFILE" $FLAGS
    if [ $? -ne 0 ]; then
        echo "Linking failed"
        exit 1
    fi
}

# Function to clean up build artifacts
clean() {
    rm -rf "$OBJ_DIR" "$BIN_DIR"
}

usage() {
    echo "Usage: $0 [debug|release|clean] [gcc|clang]"
    exit 1
}

if [ -z "$2" ]
then
  echo "No second argument supplied, assume gcc"
  CC=gcc
fi

case "$2" in
  gcc)
    CC=gcc
    ;;
  clang)
    CC=clang
    ;;
esac

case "$1" in
    debug)
        echo "Building in debug mode..."
        compile "$DEBUG_FLAGS"
        link "$DEBUG_FLAGS" "$DEBUG_EXE"
        ;;
    release)
        echo "Building in release mode..."
        compile "$RELEASE_FLAGS"
        link "$RELEASE_FLAGS" "$RELEASE_EXE"
        ;;
    clean)
        echo "Cleaning up..."
        clean
        ;;
    *)
        echo "Invalid option: $1"
        usage
        ;;
esac


echo "Done."
