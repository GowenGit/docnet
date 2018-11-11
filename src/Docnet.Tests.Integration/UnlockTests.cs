using Docnet.Core.Exceptions;
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
            var unlockedBytes = _fixture.Lib.Unlock("Docs/protected_0.pdf", "password");

            using (var file = new TempFile(unlockedBytes))
            {
                var sameBytes = _fixture.Lib.Unlock(file.FilePath, null);

                Assert.Equal(unlockedBytes, sameBytes);
            }
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData(null)]
        public void Unlock_WhenCalledWithWrongPassword_ShouldThrow(string password)
        {
            Assert.Throws<DocnetException>(() => _fixture.Lib.Unlock("Docs/protected_0.pdf", password));
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData("password")]
        [InlineData(null)]
        public void Unlock_WhenCalledForUnlockedFileWithAnyPassword_ShouldReturnByteArray(string password)
        {
            var unlockedBytes = _fixture.Lib.Unlock("Docs/simple_0.pdf", password);

            Assert.True(unlockedBytes.Length > 0);
        }
    }
}
