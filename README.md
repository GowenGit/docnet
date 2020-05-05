# docnet

![Master Health](https://github.com/GowenGit/docnet/workflows/Test%20Commit/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Docnet.Core.svg)](https://www.nuget.org/packages/Docnet.Core)

## Description

**docnet** aims to be a fast PDF editing and data extraction library. It is a `.NET Standard 2.0` wrapper for `PDFium C++` library that is used by `chromium`.

PDFium version: `4120`

## Notes

* **docnet** currently supports `x64` configuration only.

Supported platforms:

- win
- linux
- osx

## Features

- [x] Extract PDF version
- [x] Extract page count
- [ ] Extract page information
   - [x] Get page width
   - [x] Get page height
   - [x] Get page text
   - [x] Get characters
   - [x] Get character boundaries
   - [ ] Cluster characters into words
   - [x] Render page to image
- [x] Split PDF document
- [x] Merge PDF document
- [x] Unlock PDF document
- [x] Convert **JPEG** files to PDF

## Examples

* Render PDF page as PNG and display all character bounding boxes: [example](examples/pdf-to-image/PdfToImage/Program.cs)

   ![Render PDF page example](assets/demo_thumb_0.png)

    **Note:** If you have issues running on Linux make sure that `libgdiplus` is installed since this example uses `System.Drawing.Common`.

* Convert JPEG file to PDF: [example](examples/image-to-pdf/ImageToPdf/Program.cs)
