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
    }
}
