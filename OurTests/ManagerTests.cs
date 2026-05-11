using DbManager;
using DbManager.Security;

namespace OurTests
{
    public class ManagerTests
    {
        [Fact]
        public void IsUserAdmin()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.SecurityManager.IsUserAdmin());    // Default checks
            Assert.False(new Manager("user").IsUserAdmin());
            Assert.False(new Manager(null).IsUserAdmin());

            database.Save("IsUserAdminTest");
            database = Database.Load("IsUserAdminTest", "admin", "adminPassword");

            Assert.True(database.SecurityManager.IsUserAdmin());    // Checks after reloading the database
            Assert.False(new Manager("user").IsUserAdmin());
            Assert.False(new Manager(null).IsUserAdmin());
        }

        [Fact]
        public void IsPasswordCorrect()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.SecurityManager.IsPasswordCorrect("admin", "adminPassword"));    // Default checks
            Assert.False(database.SecurityManager.IsPasswordCorrect("admin", "a"));
            Assert.False(database.SecurityManager.IsPasswordCorrect(null, "adminPassword"));
        }

        [Fact]
        public void PrivilegeTests()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.SecurityManager.IsGrantedPrivilege("admin", "TestTable", Privilege.Select));   // Default checks
            Assert.False(database.SecurityManager.IsGrantedPrivilege("user", "TestTable", Privilege.Insert));
            Assert.False(database.SecurityManager.IsGrantedPrivilege(null, "TestTable", Privilege.Select));
            Assert.False(database.SecurityManager.IsGrantedPrivilege("admin", null, Privilege.Select));

            var profile = new Profile { Name = "client" };
            var user = new User("client1", "userPassword");
            profile.Users.Add(user);

            database.SecurityManager.AddProfile(profile);

            Assert.False(database.SecurityManager.IsGrantedPrivilege("client1", "TestTable", Privilege.Select));   // Check before granting the privilege

            database.SecurityManager.GrantPrivilege(profile.Name, "TestTable", Privilege.Select);

            Assert.True(database.SecurityManager.IsGrantedPrivilege("client1", "TestTable", Privilege.Select));

            database.Save("IsGrantedPrivilegeTest");
            database = Database.Load("IsGrantedPrivilegeTest", "admin", "adminPassword");

            Assert.True(database.SecurityManager.IsGrantedPrivilege("admin", "TestTable", Privilege.Select));   // Checks after reloading the database
            Assert.False(database.SecurityManager.IsGrantedPrivilege("user", "TestTable", Privilege.Insert));
            Assert.False(database.SecurityManager.IsGrantedPrivilege(null, "TestTable", Privilege.Select));
            Assert.False(database.SecurityManager.IsGrantedPrivilege("admin", null, Privilege.Select));

            Assert.True(database.SecurityManager.IsGrantedPrivilege("client1", "TestTable", Privilege.Select));
        }

        [Fact]
        public void AddProfile()
        {
            var database = Database.CreateTestDatabase();
            var profile = new Profile { Name = "client" };

            database.SecurityManager.AddProfile(profile);

            Assert.NotNull(database.SecurityManager.ProfileByName("client"));
            Assert.Equal("client", database.SecurityManager.ProfileByName("client").Name);
        }

        [Fact]
        public void RemoveProfile()
        {
            var database = Database.CreateTestDatabase();
            var profile = new Profile { Name = "client" };

            database.SecurityManager.AddProfile(profile);
            Assert.NotNull(database.SecurityManager.ProfileByName("client"));

            database.SecurityManager.RemoveProfile("client");
            Assert.Null(database.SecurityManager.ProfileByName("client"));

            Assert.False(database.SecurityManager.RemoveProfile("a"));

            var man = new Manager("user");
            Assert.False(man.RemoveProfile("client"));
        }

        [Fact]
        public void GrantRevokePrivilege()
        {
            var database = Database.CreateTestDatabase();
            var profile = new Profile { Name = "client" };
            var user = new User("client1", "userPassword");
            profile.Users.Add(user);

            database.SecurityManager.AddProfile(profile);

            database.SecurityManager.GrantPrivilege("client", "TestTable", Privilege.Select);
            database.SecurityManager.GrantPrivilege("client", "TestTable", Privilege.Insert);
            database.SecurityManager.RevokePrivilege("client", "TestTable", Privilege.Insert);

            Assert.True(profile.IsGrantedPrivilege("TestTable", Privilege.Select));

            database.SecurityManager.GrantPrivilege(null, "TestTable", Privilege.Insert);
            database.SecurityManager.RevokePrivilege(null, "TestTable", Privilege.Insert);

            database.Save("GrantPrivilegeTest");
            database = Database.Load("GrantPrivilegeTest", "client1", "userPassword");

            database.SecurityManager.GrantPrivilege("client", "TestTable", Privilege.Insert);
            Assert.False(profile.IsGrantedPrivilege("TestTable", Privilege.Insert));

            database.SecurityManager.RevokePrivilege("client", "TestTable", Privilege.Select);
            Assert.True(profile.IsGrantedPrivilege("TestTable", Privilege.Select));
        }

        [Fact]
        public void LoadException()
        {
            Assert.NotNull(Manager.Load("!·$%&/()=?¿.n@|#~€¬{[]}---̣̣_<>Ç", null));

            File.WriteAllText("ManagerLoadExceptionTest_Security.txt", "NO_ES_UN_NUMERO");

            Assert.Null(Manager.Load("ManagerLoadExceptionTest", null));
        }
    }
}
