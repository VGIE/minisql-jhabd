using DbManager.Security;

namespace OurTests
{
    public class ProfileTests
    {
        [Fact]
        public void GrantPrivilegeTest()
        {
            var profile = new Profile { Name = "profile" };

            Assert.True(profile.GrantPrivilege("table", Privilege.Select));
            Assert.True(profile.PrivilegesOn.ContainsKey("table"));
            Assert.Contains(Privilege.Select, profile.PrivilegesOn["table"]);
        }

        [Fact]
        public void RevokePrivilegeTest()
        {
            var profile = new Profile { Name = "profile" };

            Assert.False(profile.RevokePrivilege("table", Privilege.Select));

            profile.GrantPrivilege("table", Privilege.Select);

            Assert.True(profile.RevokePrivilege("table", Privilege.Select));
            Assert.True(profile.PrivilegesOn.ContainsKey("table"));
            Assert.DoesNotContain(Privilege.Select, profile.PrivilegesOn["table"]);
            Assert.False(profile.RevokePrivilege("table", Privilege.Select));
        }

        [Fact]
        public void IsGrantedPrivilegeTest()
        {
            var profile = new Profile { Name = "profile" };

            Assert.False(profile.IsGrantedPrivilege("table", Privilege.Select));

            profile.GrantPrivilege("table", Privilege.Select);

            Assert.True(profile.IsGrantedPrivilege("table", Privilege.Select));
            Assert.False(profile.IsGrantedPrivilege("table", Privilege.Insert));
        }

        [Fact]
        public void AdditionalProfileTest()
        {
            var profile = new Profile { Name = "profile" };
            Assert.Equal("profile", profile.Name);

            profile.Users = [new User("Walter", "Albuquerque308")];

            Assert.Equal("Walter", profile.Users[0].Username);
            Assert.NotNull(profile.Users[0].EncryptedPassword);
        }
    }
}
