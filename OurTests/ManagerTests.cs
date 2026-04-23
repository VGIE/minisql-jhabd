using DbManager;
using DbManager.Security;

namespace OurTests
{
    public class ManagerTests
    {
        /*
        [Fact]
        public void IsUserAdmin()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.SecurityManager.IsUserAdmin());    // Default checks
            Assert.False(new Manager("user").IsUserAdmin());
            Assert.False(new Manager(null).IsUserAdmin());

            database.Save("test");
            database = Database.Load("test", "admin", "adminPassword");

            Assert.True(database.SecurityManager.IsUserAdmin());    // Checks after reloading the database
            Assert.False(new Manager("user").IsUserAdmin());
            Assert.False(new Manager(null).IsUserAdmin());
        }

        [Fact]
        public void PrivilegeTests()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.SecurityManager.IsGrantedPrivilege("admin", "table", Privilege.Select));   // Default checks
            Assert.False(database.SecurityManager.IsGrantedPrivilege("user", "table", Privilege.Insert));
            Assert.False(database.SecurityManager.IsGrantedPrivilege(null, "table", Privilege.Select));

            var profile = new Profile { Name = "client" };
            var user = new User("client1", "userPassword");
            profile.Users.Add(user);

            database.SecurityManager.AddProfile(profile);
            database.SecurityManager.GrantPrivilege(profile.Name, "table", Privilege.Select);

            Assert.True(database.SecurityManager.IsGrantedPrivilege("client1", "table", Privilege.Select));

            database.Save("test");
            database = Database.Load("test", "admin", "adminPassword");

            //Assert.True(database.SecurityManager.IsGrantedPrivilege("admin", "table", Privilege.Select));   // Checks after reloading the database
            //Assert.False(database.SecurityManager.IsGrantedPrivilege("user", "table", Privilege.Insert));
            //Assert.False(database.SecurityManager.IsGrantedPrivilege(null, "table", Privilege.Select));

            //Assert.True(database.SecurityManager.IsGrantedPrivilege("client1", "table", Privilege.Select));
        }
        */
    }
}
