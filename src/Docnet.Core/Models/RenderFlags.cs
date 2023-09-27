using System;

namespace Docnet.Core.Models
{
    [Flags]
    public enum RenderFlags
    {
        None = 0x00, // None
        RenderAnnotations = 0x01, // FPDF_ANNOT: Set if annotations are to be rendered.
        OptimizeTextForLcd = 0x02, // FPDF_LCD_TEXT: Set if using text rendering optimized for LCD display. This flag will only take effect if anti-aliasing is enabled for text.
        NoNativeText = 0x04, // FPDF_NO_NATIVETEXT: Don't use the native text output available on some platforms
        Grayscale = 0x08, // FPDF_GRAYSCALE: Grayscale output
        LimitImageCacheSize = 0x200, // FPDF_RENDER_LIMITEDIMAGECACHE: Limit image cache size
        ForceHalftone = 0x400, // FPDF_RENDER_FORCEHALFTONE: Always use halftone for image stretching
        RenderForPrinting = 0x800, // FPDF_PRINTING: Render for printing
        DisableTextAntialiasing = 0x1000, // FPDF_RENDER_NO_SMOOTHTEXT: Set to disable anti-aliasing on text. This flag will also disable LCD optimization for text rendering
        DisableImageAntialiasing = 0x2000, // FPDF_RENDER_NO_SMOOTHIMAGE: Set to disable anti-aliasing on images
        DisablePathAntialiasing = 0x4000 // FPDF_RENDER_NO_SMOOTHPATH: Set to disable anti-aliasing on paths
    }
}