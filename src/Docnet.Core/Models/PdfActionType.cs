using System;
using System.Collections.Generic;
using System.Text;

namespace Docnet.Core.Models
{
    public enum PdfActionType
    {
        // Unsupported action type.
        UNSUPPORTED = 0,

        // Go to a destination within current document.
        GOTO = 1,

        // Go to a destination within another document.
        REMOTEGOTO = 2,

        // URI, including web pages and other Internet resources.
        URI = 3,

        // Launch an application or open a file.
        PDFACTION_LAUNCH = 4
    }
}
