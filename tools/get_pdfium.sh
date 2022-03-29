#!/bin/bash

wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-linux-x64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-linux-arm.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-linux-arm64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-win-x64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-win-x86.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4722/pdfium-mac-x64.tgz

mkdir linux
mkdir linux-arm
mkdir linux-arm64
mkdir windows
mkdir windowsx86
mkdir osx

tar -xvf pdfium-linux-x64.tgz -C linux
tar -xvf pdfium-linux-arm.tgz -C linux-arm
tar -xvf pdfium-linux-arm64.tgz -C linux-arm64
tar -xvf pdfium-mac-x64.tgz -C osx
tar -xvf pdfium-win-x64.tgz -C windows
tar -xvf pdfium-win-x86.tgz -C windowsx86

mkdir -p ../src/Docnet.Core/runtimes/linux/native/
mkdir -p ../src/Docnet.Core/runtimes/linux-arm/native/
mkdir -p ../src/Docnet.Core/runtimes/linux-arm64/native/
mkdir -p ../src/Docnet.Core/runtimes/osx/native/
mkdir -p ../src/Docnet.Core/runtimes/win-x64/native/
mkdir -p ../src/Docnet.Core/runtimes/win-x86/native/

cp linux/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux/native/pdfium.so
cp linux/LICENSE ../src/Docnet.Core/runtimes/linux/native/LICENSE

cp linux-arm/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux-arm/native/pdfium.so
cp linux-arm/LICENSE ../src/Docnet.Core/runtimes/linux-arm/native/LICENSE

cp linux-arm64/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux-arm64/native/pdfium.so
cp linux-arm64/LICENSE ../src/Docnet.Core/runtimes/linux-arm64/native/LICENSE

cp osx/lib/libpdfium.dylib ../src/Docnet.Core/runtimes/osx/native/pdfium.dylib
cp osx/LICENSE ../src/Docnet.Core/runtimes/osx/native/LICENSE

cp windows/x64/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x64/native/pdfium.dll
cp windows/LICENSE ../src/Docnet.Core/runtimes/win-x64/native/LICENSE

cp windowsx86/x86/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x86/native/pdfium.dll
cp windowsx86/LICENSE ../src/Docnet.Core/runtimes/win-x86/native/LICENSE

rm pdfium-linux-x64.tgz pdfium-linux-arm.tgz pdfium-linux-arm64.tgz pdfium-win-x64.tgz pdfium-win-x86.tgz pdfium-mac-x64.tgz
rm -rf linux linux-arm linux-arm64 windows windowsx86 osx
