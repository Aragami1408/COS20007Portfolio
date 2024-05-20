@echo off
setlocal

REM Run vcvarsall.bat to set up MSVC environment
call "vcvarsall.bat" x64

REM Check if vcvarsall.bat was successful
if errorlevel 1 (
    echo Failed to set up MSVC environment!
    exit /b 1
)

REM Set directories
set SRC_DIR=src
set OBJ_DIR=obj
set BIN_DIR=bin

REM Create directories if they don't exist
if not exist %OBJ_DIR% mkdir %OBJ_DIR%
if not exist %BIN_DIR% mkdir %BIN_DIR%

REM Set the compiler and linker
set CC=cl
set CFLAGS=/c /Fo%OBJ_DIR%\ /I%SRC_DIR% /nologo
set LDFLAGS=/Fe%BIN_DIR%\

REM Compile each .c file to an object file
for %%f in (%SRC_DIR%\*.c) do (
    echo Compiling %%f...
    %CC% %CFLAGS% %%f
    if errorlevel 1 (
        echo Compilation failed!
        exit /b 1
    )
)

REM Link all object files to create the binary
echo Linking...
%CC% %LDFLAGS% %OBJ_DIR%\*.obj
if errorlevel 1 (
    echo Linking failed!
    exit /b 1
)

echo Build succeeded!
endlocal
