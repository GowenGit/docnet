#!/bin/bash

wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4250/pdfium-linux.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4648/pdfium-linux-arm.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4250/pdfium-windows-x64.zip
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4250/pdfium-windows-x86.zip
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F4250/pdfium-darwin.tgz

mkdir linux
mkdir linux-arm
mkdir windows
mkdir osx

tar -xvf pdfium-linux.tgz -C linux
tar -xvf pdfium-linux-arm.tgz -C linux-arm
tar -xvf pdfium-darwin.tgz -C osx
unzip pdfium-windows-x64.zip -d windows
unzip pdfium-windows-x86.zip -d windowsx86

mkdir -p ../src/Docnet.Core/runtimes/linux/native/
mkdir -p ../src/Docnet.Core/runtimes/linux-arm/native/
mkdir -p ../src/Docnet.Core/runtimes/osx/native/
mkdir -p ../src/Docnet.Core/runtimes/win-x64/native/
mkdir -p ../src/Docnet.Core/runtimes/win-x86/native/

cp linux/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux/native/pdfium.so
cp linux/LICENSE ../src/Docnet.Core/runtimes/linux/native/LICENSE

cp linux-arm/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux-arm/native/pdfium.so
cp linux-arm/LICENSE ../src/Docnet.Core/runtimes/linux-arm/native/LICENSE

cp osx/lib/libpdfium.dylib ../src/Docnet.Core/runtimes/osx/native/pdfium.dylib
cp osx/LICENSE ../src/Docnet.Core/runtimes/osx/native/LICENSE

cp windows/x64/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x64/native/pdfium.dll
cp windows/LICENSE ../src/Docnet.Core/runtimes/win-x64/native/LICENSE

cp windowsx86/x86/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x86/native/pdfium.dll
cp windowsx86/LICENSE ../src/Docnet.Core/runtimes/win-x86/native/LICENSE

rm pdfium-linux.tgz pdfium-linux-arm.tgz pdfium-windows-x64.zip pdfium-windows-x86.zip pdfium-darwin.tgz
rm -rf linux linux-arm windows windowsx86 osx
