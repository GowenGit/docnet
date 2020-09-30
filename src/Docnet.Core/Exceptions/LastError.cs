namespace Docnet.Core.Exceptions
{
    internal static class LastError
    {
        internal static string ErrorCodePhrase(uint errorCode)
        {
            switch (errorCode)
            {
                case 0:
                    return "no error";
                case 1:
                    return "unknown error";
                case 2:
                    return "file not found or could not be opened";
                case 3:
                    return "file not in PDF format or corrupted";
                case 4:
                    return "password required or incorrect password";
                case 5:
                    return "unsupported security scheme";
                case 6:
                    return "page not found or content error";
                case 1001:
                    return "the requested operation cannot be completed due to a license restrictions";
                default:
                    return "unknown error";
            }
        }
    }
}