using DbManager.Security;

namespace OurTests
{
    public class UserTests
    {
        [Fact]
        public void CreateUserWithCredentialsTest()
        {
            var user = new User("Walter", "Albuquerque308");

            Assert.Equal("Walter", user.Username);
            Assert.Equal(user.EncryptedPassword, Encryption.Encrypt("Albuquerque308"));
        }

        [Fact]
        public void CreateEmptyUserTest()
        {
            var user = new User();

            Assert.Null(user.Username);
            Assert.Null(user.EncryptedPassword);
        }
    }
}
