using DbManager;

namespace OurTests
{
    public class MiniSQLParserTests
    {
        [Fact]
        public void ParseSelect()
        {
            var query = MiniSQLParser.Parse("SELECT age, name FROM users WHERE age > 30");

            Assert.NotNull(query);
            Assert.IsType<Select>(query);

            Select select = (Select)query;

            Assert.Equal("users", select.Table);
            Assert.Equal(select.Columns, ["age", "name"]);
            Assert.Equal("age", select.Where.ColumnName);
            Assert.Equal(">", select.Where.Operator);
            Assert.Equal("30", select.Where.LiteralValue);
        }

        [Fact]
        public void ParseInsert()
        {
            var query = MiniSQLParser.Parse("INSERT INTO users VALUES ('Cardinal', 'Stavanger', 'Norway')");

            Assert.NotNull(query);
            Assert.IsType<Insert>(query);

            Insert insert = (Insert)query;

            Assert.Equal("users", insert.Table);
            Assert.Equal(insert.Values, ["Cardinal", "Stavanger", "Norway"]);
        }

        [Fact]
        public void ParseException()
        {
            var query = MiniSQLParser.Parse(null);
            Assert.Null(query);

            query = MiniSQLParser.Parse("");
            Assert.Null(query);
        }
    }
}
