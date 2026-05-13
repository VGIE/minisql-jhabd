using DbManager.Network;

namespace OurTests
{
    public class XmlDeserializerTests
    {
        [Fact]
        public void ParseOpen()
        {
            Assert.True(XmlDeserializer.ParseOpen(@"<Open Database=""TestDB"" User=""root"" Password=""toor""/>", out string database, out string username, out string password));
            Assert.Equal("TestDB", database);
            Assert.Equal("root", username);
            Assert.Equal("toor", password);

            Assert.False(XmlDeserializer.ParseOpen("", out database, out username, out password));
        }

        [Fact]
        public void ParseOpenCreateAnswer()
        {
            Assert.True(XmlDeserializer.ParseOpenCreateAnswer(XmlSerializer.OpenCreateSuccess, out string error));
            Assert.Null(error);

            Assert.False(XmlDeserializer.ParseOpenCreateAnswer(@"<Error>Database already exists</Error>", out error));
            Assert.Equal("Database already exists", error);

            Assert.False(XmlDeserializer.ParseOpenCreateAnswer("", out error));
            Assert.Null(error);
        }

        [Fact]
        public void ParseCreate()
        {
            Assert.True(XmlDeserializer.ParseCreate(@"<Create Database=""TestDB"" User=""root"" Password=""toor""/>", out string database, out string username, out string password));
            Assert.Equal("TestDB", database);
            Assert.Equal("root", username);
            Assert.Equal("toor", password);

            Assert.False(XmlDeserializer.ParseCreate("", out database, out username, out password));
        }

        [Fact]
        public void ParseQuery()
        {
            Assert.True(XmlDeserializer.ParseQuery(@"<Query>SELECT * FROM Users</Query>", out string query));
            Assert.Equal("SELECT * FROM Users", query);

            Assert.False(XmlDeserializer.ParseQuery("", out query));
        }

        [Fact]
        public void ParseQueryAnswer()
        {
            Assert.False(XmlDeserializer.ParseQueryAnswer(@"<Answer><Error>Syntax error</Error></Answer>", out string answerContent));
            Assert.Equal("Syntax error", answerContent);
            Assert.True(XmlDeserializer.ParseQueryAnswer(@"<Answer>Result of the query</Answer>", out answerContent));
            Assert.False(XmlDeserializer.ParseQueryAnswer("", out answerContent));
        }

        [Fact]
        public void IsCloseCommand()
        {
            Assert.True(XmlDeserializer.IsCloseCommand(@"<Close/>"));
            Assert.False(XmlDeserializer.IsCloseCommand(@"<Open Database=""TestDB"" User=""root"" Password=""toor""/>"));
        }
    }
}
