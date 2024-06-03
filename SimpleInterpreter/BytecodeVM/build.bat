@echo off
@set OBJ_DIR=obj
@set OUT_DIR=bin
@set OUT_EXE=simpc_win32
@set INCLUDES=/Isrc\
@set SOURCES=src\chunk.c src\compiler.c src\debug.c src\main.c src\memory.c src\object.c src\scanner.c src\table.c src\value.c src\vm.c

if not exist "%OUT_DIR%" mkdir %OUT_DIR%

if "%1" == "debug" (
  echo "Debug mode"
  cl /nologo /Zi /Od /MD /utf-8 %INCLUDES% %SOURCES% /Fe%OUT_DIR%/%OUT_EXE%.exe /Fo%OUT_DIR%/ /link
)
if "%1" == "release" (
  echo "Release mode"
  cl /nologo /O2 /MD /D NDEBUG /utf-8 %INCLUDES% %SOURCES% /Fe%OUT_DIR%/%OUT_EXE%.exe /Fo%OUT_DIR%/ /link
)
if "%1" == "clean" (
  echo "Clean mode"
  del /q bin
)
