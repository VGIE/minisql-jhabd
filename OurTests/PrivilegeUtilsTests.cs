using DbManager.Parser;
using DbManager.Security;

namespace OurTests
{
    public class PrivilegeUtilsTests
    {
        [Fact]
        public void FromPrivilegeName()
        {
            var priv = PrivilegeUtils.FromPrivilegeName("SELECT");
            Assert.Equal(Privilege.Select, priv);

            priv = PrivilegeUtils.FromPrivilegeName("INSERT");
            Assert.Equal(Privilege.Insert, priv);

            priv = PrivilegeUtils.FromPrivilegeName("UPDATE");
            Assert.Equal(Privilege.Update, priv);

            priv = PrivilegeUtils.FromPrivilegeName("DELETE");
            Assert.Equal(Privilege.Delete, priv);

            Assert.Throws<Exception>(() => PrivilegeUtils.FromPrivilegeName("CREATE"));
        }
    }
}
