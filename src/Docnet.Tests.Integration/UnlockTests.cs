using System;
using System.IO;
using Docnet.Core.Exceptions;
using Docnet.Tests.Integration.Utils;
using Xunit;

namespace Docnet.Tests.Integration
{
    [Collection("Lib collection")]
    public sealed class UnlockTests
    {
        private readonly LibFixture _fixture;

        public UnlockTests(LibFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Unlock_WhenCalledWithNullFilePath_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Unlock((string)null, null));
        }

        [Fact]
        public void Unlock_WhenCalledWithNullBytes_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Lib.Unlock((byte[])null, null));
        }

        [Fact]
        public void Unlock_WhenCalledWithLockedPath_ShouldReturnUnlockedDocument()
        {
            var unlockedBytes = _fixture.Lib.Unlock("Docs/protected_0.pdf", "password");

            using (var file = new TempFile(unlockedBytes))
            {
                var sameBytes = _fixture.Lib.Unlock(file.FilePath, null);

                Assert.Equal(unlockedBytes, sameBytes);
            }
        }

        [Fact]
        public void Unlock_WhenCalledWithLockedBytes_ShouldReturnUnlockedDocument()
        {
            var bytes = File.ReadAllBytes("Docs/protected_0.pdf");

            var unlockedBytes = _fixture.Lib.Unlock(bytes, "password");

            using (var file = new TempFile(unlockedBytes))
            {
                var sameBytes = _fixture.Lib.Unlock(file.FilePath, null);

                Assert.Equal(unlockedBytes, sameBytes);
            }
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData(null)]
        public void Unlock_WhenCalledWithPathWrongPassword_ShouldThrow(string password)
        {
            Assert.Throws<DocnetException>(() => _fixture.Lib.Unlock("Docs/protected_0.pdf", password));
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData(null)]
        public void Unlock_WhenCalledWithBytesWrongPassword_ShouldThrow(string password)
        {
            var bytes = File.ReadAllBytes("Docs/protected_0.pdf");

            Assert.Throws<DocnetException>(() => _fixture.Lib.Unlock(bytes, password));
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData(null)]
        public void Unlock_WhenCalledWithBytesWrongPassword_ShouldThrowAndGiveErrorMessage(string password)
        {
            var bytes = File.ReadAllBytes("Docs/protected_0.pdf");

            Assert.Throws<DocnetException>(() => _fixture.Lib.Unlock(bytes, password));

            var message = _fixture.Lib.GetLastError();

            Assert.Equal("password required or incorrect password", message);
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData("password")]
        [InlineData(null)]
        public void Unlock_WhenCalledForUnlockedFilePathWithAnyPassword_ShouldReturnByteArray(string password)
        {
            var unlockedBytes = _fixture.Lib.Unlock("Docs/simple_0.pdf", password);

            Assert.True(unlockedBytes.Length > 0);
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData("password")]
        [InlineData(null)]
        public void Unlock_WhenCalledForUnlockedFileBytesWithAnyPassword_ShouldReturnByteArray(string password)
        {
            var bytes = File.ReadAllBytes("Docs/simple_0.pdf");

            var unlockedBytes = _fixture.Lib.Unlock(bytes, password);

            Assert.True(unlockedBytes.Length > 0);
        }

        [Theory]
        [InlineData("fake_password")]
        [InlineData("password")]
        [InlineData(null)]
        public void Unlock_WhenCalledForUnlockedFileBytesWithAnyPassword_ShouldReturnByteArrayAndNoError(string password)
        {
            var bytes = File.ReadAllBytes("Docs/simple_0.pdf");

            var unlockedBytes = _fixture.Lib.Unlock(bytes, password);

            Assert.True(unlockedBytes.Length > 0);

            var message = _fixture.Lib.GetLastError();

            Assert.Equal("no error", message);
        }
    }
}
