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
EXECUTABLE="simpc"

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
    echo $CC $OBJ_DIR/*.o -o "$BIN_DIR/$EXECUTABLE" $FLAGS
    $CC $OBJ_DIR/*.o -o "$BIN_DIR/$EXECUTABLE" $FLAGS
    if [ $? -ne 0 ]; then
        echo "Linking failed"
        exit 1
    fi
}

# Function to clean up build artifacts
clean() {
    rm -rf "$OBJ_DIR" "$BIN_DIR"
}

# Parse command line argument
if [ "$#" -ne 1 ]; then
    echo "Usage: $0 [debug|release|clean]"
    exit 1
fi

case "$1" in
    debug)
        echo "Building in debug mode..."
        compile "$DEBUG_FLAGS"
        link "$DEBUG_FLAGS"
        ;;
    release)
        echo "Building in release mode..."
        compile "$RELEASE_FLAGS"
        link "$RELEASE_FLAGS"
        ;;
    clean)
        echo "Cleaning up..."
        clean
        ;;
    *)
        echo "Invalid option: $1"
        echo "Usage: $0 [debug|release|clean]"
        exit 1
        ;;
esac

echo "Done."
