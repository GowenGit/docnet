using Docnet.Core.Validation;

namespace Docnet.Tests.Integration.Utils
{
    public class PageRangeValidatorFixture
    {
        public PageRangeValidatorFixture()
        {
            PageRangeValidator = new PageRangeValidator();
        }

        public IPageRangeValidator PageRangeValidator { get; }
    }
}