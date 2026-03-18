using DbManager;

namespace OurTests
{
    public class MiniSQLParserTests
    {
        [Fact]
        public void ParseSelect()
        {
            var query = MiniSQLParser.Parse("SELECT age,name FROM users WHERE age > 30");

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
            var query = MiniSQLParser.Parse("INSERT INTO users VALUES ('Cardinal','Stavanger','Norway')");

            Assert.NotNull(query);
            Assert.IsType<Insert>(query);

            Insert insert = (Insert)query;

            Assert.Equal("users", insert.Table);
            Assert.Equal(insert.Values, ["Cardinal", "Stavanger", "Norway"]);
        }

        [Fact]
        public void ParseCreateTable()
        {
            var query = MiniSQLParser.Parse("CREATE TABLE manin (name String , edad int)");

            Assert.NotNull(query);
            Assert.IsType<CreateTable>(query);

            CreateTable createTable = (CreateTable)query;

            Assert.Equal("manin", createTable.Table);
            Assert.Equal(2, createTable.ColumnsParameters.Count);
            Assert.Equal("name", createTable.ColumnsParameters[0].Name);
            Assert.Equal("String", createTable.ColumnsParameters[0].Type.ToString()); 
            Assert.Equal("edad", createTable.ColumnsParameters[1].Name);
            Assert.Equal("Int", createTable.ColumnsParameters[1].Type.ToString());
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
