# docnet

## Description

**docnet** aims to be a fast PDF editing and data extraction library. It is a `.NET Standard 2.0` wrapper for `PDFium C++` library that is used by `chromium`.

## Notes

* **docnet** currently supports `x64` configuration only.

## Features

- [x] Extract PDF version
- [x] Extract page count
- [ ] Extract page information
   - [x] Get page width
   - [x] Get page height
   - [x] Get page text
   - [ ] Get characters
   - [ ] Get character boundaries
   - [ ] Cluster characters into words
   - [ ] Render page to image
- [x] Split PDF document
- [x] Merge PDF document
- [x] Unlock PDF document