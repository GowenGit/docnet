namespace Docnet.Core.Exceptions
{
    public class DocnetLoadDocumentsException : DocnetException
    {
        public DocnetLoadDocumentError[] FilesWithErrors { get; }

        public DocnetLoadDocumentsException(string message, DocnetLoadDocumentError[] loadErrors) : base(message)
        {
            FilesWithErrors = loadErrors;
        }
    }

    public class DocnetLoadDocumentError
    {
        public int DocumentIndex { get; }

        public DocnetLoadDocumentException Exception { get; }

        public DocnetLoadDocumentError(int documentIndex, DocnetLoadDocumentException exception)
        {
            DocumentIndex = documentIndex;
            Exception = exception;
        }
    }
}