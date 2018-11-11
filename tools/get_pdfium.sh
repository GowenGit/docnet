#!/bin/bash

wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F3580/pdfium-linux.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F3580/pdfium-windows-x64.zip

mkdir linux
mkdir windows

tar -xvf pdfium-linux.tgz -C linux
unzip pdfium-windows-x64.zip -d windows

cp linux/lib/libpdfium.so ../src/Docnet.Core/runtimes/linux-x64/native/pdfium.so
cp linux/LICENSE ../src/Docnet.Core/runtimes/linux-x64/native/LICENSE

cp windows/x64/bin/pdfium.dll ../src/Docnet.Core/runtimes/win-x64/native/pdfium.dll
cp windows/LICENSE ../src/Docnet.Core/runtimes/win-x64/native/LICENSE

rm pdfium-linux.tgz pdfium-windows-x64.zip
rm -rf linux windows
