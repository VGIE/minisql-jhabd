using DbManager;
using DbManager.Security;

namespace OurTests
{
    public class EncryptionTests
    {
        [Fact]
        public void EncryptionTest()
        {
            Assert.Equal("AE-79-E3-6A-7E-58-67-92-CE-D7-A4-D7-1C-17-69-F4", Encryption.Encrypt("albuquerque308"));
            Assert.Null(Encryption.Encrypt(null));
        }
    }
}
