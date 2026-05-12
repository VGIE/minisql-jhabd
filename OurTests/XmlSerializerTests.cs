using DbManager.Network;

namespace OurTests
{
    public class XmlSerializerTests
    {
        [Fact]
        public void OpenDatabase()
        {
            Assert.Equal(@"<Open Database=""TestDB"" User=""root"" Password=""toor""/>", XmlSerializer.OpenDatabase("TestDB", "root", "toor"));
        }

        [Fact]
        public void OpenCreateSuccess()
        {
            Assert.Equal("<Success/>", XmlSerializer.OpenCreateSuccess);
        }

        [Fact]
        public void OpenCreateError()
        {
            Assert.Equal(@"<Error>Database already exists</Error>", XmlSerializer.OpenCreateError("Database already exists"));
        }

        [Fact]
        public void CreateDatabase()
        {
            Assert.Equal(@"<Create Database=""TestDB"" User=""root"" Password=""toor""/>", XmlSerializer.CreateDatabase("TestDB", "root", "toor"));
        }

        [Fact]
        public void CreateSuccess()
        {
            Assert.Equal("<Success/>", XmlSerializer.CreateSuccess);
        }

        [Fact]
        public void CreateError()
        {
            Assert.Equal(@"<Error>Database already exists</Error>", XmlSerializer.CreateError("Database already exists"));
        }

        [Fact]
        public void Query()
        {
            Assert.Equal(@"<Query>SELECT * FROM Users</Query>", XmlSerializer.Query("SELECT * FROM Users"));
        }

        [Fact]
        public void SucessfulAnswer()
        {
            Assert.Equal(@"<Answer>42</Answer>", XmlSerializer.SucessfulAnswer("42"));
        }

        [Fact]
        public void ErrorAnswer()
        {
            Assert.Equal(@"<Answer><Error>Database not found</Error></Answer>", XmlSerializer.ErrorAnswer("Database not found"));
        }

        [Fact]
        public void CloseConnection()
        {
            Assert.Equal("<Close/>", XmlSerializer.CloseConnection);
        }
    }
}