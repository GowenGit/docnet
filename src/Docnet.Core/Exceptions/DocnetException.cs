using System;

#pragma warning disable CA2237

namespace Docnet.Core.Exceptions
{
    public class DocnetException : Exception
    {
        public DocnetException(string message) : base(message) { }

        public DocnetException(string message, Exception innerException) : base(message, innerException) { }
    }
}
