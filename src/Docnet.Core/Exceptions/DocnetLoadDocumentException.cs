using System;

#pragma warning disable CA2237

namespace Docnet.Core.Exceptions
{
    public class DocnetLoadDocumentException : DocnetException
    {
        public uint ErrorCode { get; set; } = 1;

        public DocnetLoadDocumentException(string message) : base(message) { }

        public DocnetLoadDocumentException(string message, uint errorCode) : base(DecorateMessage(message, errorCode))
        {
            ErrorCode = errorCode;
        }

        public DocnetLoadDocumentException(string message, uint errorCode, Exception innerException) : base(DecorateMessage(message, errorCode), innerException)
        {
            ErrorCode = errorCode;
        }

        private static string DecorateMessage(string message, uint errorCode)
        {
            return $"{message}: ErrorCode={errorCode}, ReasonPhrase={LastError.ErrorCodePhrase(errorCode)}";
        }
    }
}
