using System;
using System.Collections.Generic;
using System.Text;

namespace Docnet.Core.Models
{
    public enum FpdfAnnotationSubtype
    {
        UNKNOWN = 0,
        FPDF_ANNOT_TEXT = 1,
        FPDF_ANNOT_LINK = 2,
        FPDF_ANNOT_FREETEXT = 3,
        FPDF_ANNOT_LINE = 4,
        FPDF_ANNOT_SQUARE = 5,
        FPDF_ANNOT_CIRCLE = 6,
        FPDF_ANNOT_POLYGON = 7,
        FPDF_ANNOT_POLYLINE = 8,
        FPDF_ANNOT_HIGHLIGHT = 9,
        FPDF_ANNOT_UNDERLINE = 10,
        FPDF_ANNOT_SQUIGGLY = 11,
        FPDF_ANNOT_STRIKEOUT = 12,
        FPDF_ANNOT_STAMP = 13,
        FPDF_ANNOT_CARET = 14,
        FPDF_ANNOT_INK = 15,
        FPDF_ANNOT_POPUP = 16,
        FPDF_ANNOT_FILEATTACHMENT = 17,
        FPDF_ANNOT_SOUND = 18,
        FPDF_ANNOT_MOVIE = 19,
        FPDF_ANNOT_WIDGET = 20,
        FPDF_ANNOT_SCREEN = 21,
        FPDF_ANNOT_PRINTERMARK = 22,
        FPDF_ANNOT_TRAPNET = 23,
        FPDF_ANNOT_WATERMARK = 24,
        FPDF_ANNOT_THREED = 25,
        FPDF_ANNOT_RICHMEDIA = 26,
        FPDF_ANNOT_XFAWIDGET = 27
    }
}
