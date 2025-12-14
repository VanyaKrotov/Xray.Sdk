#!/usr/bin/env bash
set -e

SRC_DIR="$(dirname "$0")"
OUT_DIR="../$1"   # получаем OutputPath из MSBuild

mkdir -p "$OUT_DIR"

echo "Building Go shared library for current system → $OUT_DIR"

UNAME=$(uname -s)
ARCH=$(uname -m)

case "$UNAME" in
  Darwin)
    if [ "$ARCH" = "arm64" ]; then
      echo "→ macOS ARM64"
      go build -buildmode=c-shared -o "$OUT_DIR/NativeWrapper.dylib" -trimpath -ldflags "-s -w -buildid=" "$SRC_DIR/main.go"
    else
      echo "→ macOS x64"
      go build -buildmode=c-shared -o "$OUT_DIR/NativeWrapper.dylib" -trimpath -ldflags "-s -w -buildid=" "$SRC_DIR/main.go"
    fi
    ;;
  Linux)
    if [ "$ARCH" = "x86_64" ]; then
      echo "→ Linux x64"
      go build -buildmode=c-shared -o "$OUT_DIR/NativeWrapper.so" -trimpath -ldflags "-s -w -buildid=" "$SRC_DIR/main.go"
    elif [ "$ARCH" = "aarch64" ]; then
      echo "→ Linux ARM64"
      go build -buildmode=c-shared -o "$OUT_DIR/NativeWrapper.so" -trimpath -ldflags "-s -w -buildid=" "$SRC_DIR/main.go"
    else
      echo "Unsupported Linux architecture: $ARCH"
      exit 1
    fi
    ;;
  MINGW*|MSYS*|CYGWIN*|Windows_NT)
    echo "→ Windows x64"
    go build -buildmode=c-shared -o "$OUT_DIR/NativeWrapper.dll" -trimpath -ldflags "-s -w -buildid=" "$SRC_DIR/main.go"
    ;;
  *)
    echo "Unsupported OS: $UNAME"
    exit 1
    ;;
esac

echo "✅ Build complete. Artifact in $OUT_DIR:"
ls -1 "$OUT_DIR"
