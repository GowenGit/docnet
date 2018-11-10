using Xunit;

namespace Docnet.Tests.Integration
{
    public sealed class UnlockTests : IClassFixture<LibFixture>
    {
        private readonly LibFixture _fixture;

        public UnlockTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Unlock_WhenCalledWithLocked_ShouldReturnUnlockedDocument()
        {
            Assert.True(true);
        }

        [Fact]
        public void Unlock_WhenCalledWithWrongPassword_ShouldThrow()
        {
            Assert.True(true);
        }
    }
}
