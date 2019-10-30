#!/bin/bash

wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F3951/pdfium-linux.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F3951/pdfium-windows-x64.zip
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F3951/pdfium-darwin.tgz

mkdir linux
mkdir windows
mkdir osx

tar -xvf pdfium-linux.tgz -C linux
tar -xvf pdfium-darwin.tgz -C osx
unzip pdfium-windows-x64.zip -d windows

mkdir -p ../src/Docnet.Core/runtimes/linux-x64/native/
mkdir -p ../src/Docnet.Core/runtimes/osx-x64/native/
mkdir -p ../src/Docnet.Core/runtimes/win-x64/native/

cp linux/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux-x64/native/pdfium.so
cp linux/LICENSE ../src/Docnet.Core/runtimes/linux-x64/native/LICENSE

cp osx/lib/libpdfium.dylib ../src/Docnet.Core/runtimes/osx-x64/native/pdfium.dylib
cp osx/LICENSE ../src/Docnet.Core/runtimes/osx-x64/native/LICENSE

cp windows/x64/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x64/native/pdfium.dll
cp windows/LICENSE ../src/Docnet.Core/runtimes/win-x64/native/LICENSE

rm pdfium-linux.tgz pdfium-windows-x64.zip pdfium-darwin.tgz
rm -rf linux windows osx
