using System.Text.RegularExpressions;
using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class MiniSQLParserTests
    {
        [Fact]
        public void ParseSelect()
        {
            var query = MiniSQLParser.Parse("SELECT age,name FROM users WHERE age > 'abra bacadabra'");

            Assert.NotNull(query);
            Assert.IsType<Select>(query);

            Select select = (Select)query;

            Assert.Equal("users", select.Table);
            Assert.Equal(select.Columns, ["age", "name"]);
            Assert.Equal("age", select.Where.ColumnName);
            Assert.Equal(">", select.Where.Operator);
            Assert.Equal("abra bacadabra", select.Where.LiteralValue);
        }

        [Fact]
        public void ParseDelete()
        {
            var query = MiniSQLParser.Parse("DELETE FROM a WHERE age='0'");
            Assert.NotNull(query);
            Assert.IsType<Delete>(query);

            Delete delete = (Delete)query;

            Assert.Equal("a", delete.Table);
            Assert.Equal("age", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("0", delete.Where.LiteralValue);

            var query2 = MiniSQLParser.Parse("DELETE       FROM          a      WHERE      age='0'");
            Assert.NotNull(query2);
            Assert.IsType<Delete>(query2);

            delete = (Delete)query2;

            Assert.Equal("a", delete.Table);
            Assert.Equal("age", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("0", delete.Where.LiteralValue);

            var query3 = MiniSQLParser.Parse("DELETE       FROM          a");
            Assert.Null(query3);

            var query4 = MiniSQLParser.Parse("DELETE       FROM     a    WHERE nombre='abra bacadabra'");
            Assert.NotNull(query4);
            Assert.IsType<Delete>(query4);

            delete = (Delete)query4;

            Assert.Equal("a", delete.Table);
            Assert.Equal("nombre", delete.Where.ColumnName);
            Assert.Equal("=", delete.Where.Operator);
            Assert.Equal("abra bacadabra", delete.Where.LiteralValue);

            var query5 = MiniSQLParser.Parse("DELETE       FROM         WHERE nombre='abra bacadabra'");
            Assert.Null(query5);
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
            var query = MiniSQLParser.Parse("CREATE TABLE manin (name TEXT,edad INT)");

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
        public void ParseUpdate()
        {
            var query = MiniSQLParser.Parse("UPDATE usuarios SET edad='67',nombre='John Pork' WHERE id='1';");
            Assert.NotNull(query);
            Assert.IsType<Update>(query);

            Update update = (Update)query;

            Assert.Equal("usuarios", update.Table);

            Assert.Equal(2, update.Columns.Count); 

            Assert.Equal("edad", update.Columns[0].ColumnName); 
            Assert.Equal("67", update.Columns[0].Value);

            Assert.Equal("nombre", update.Columns[1].ColumnName);
            Assert.Equal("John Pork", update.Columns[1].Value);

            Assert.NotNull(update.Where);
            Assert.Equal("id", update.Where.ColumnName);
            Assert.Equal("=", update.Where.Operator);
            Assert.Equal("1", update.Where.LiteralValue);
        }

        [Fact]
        public void ParseDropTable()
        {
            var query = MiniSQLParser.Parse("DROP TABLE usuarios;");

            Assert.NotNull(query);
            Assert.IsType<DropTable>(query);

            DropTable dropTable = (DropTable)query;

            Assert.Equal("usuarios", dropTable.Table);
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


//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⣠⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⣄⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⣤⡶⣾
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⢀⣴⣿⡿⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⢿⣷⢄⠙⣙⢛⣒⣀⣀⣀⣀⣀⣀⣀⡀⡀⡀⡀⡀⣀⣀⢀⡀⡀⣀⣼⢞⣗⣿
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠠⢰⡿⢿⡟⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠄⡀⡀⡀⡀⢿⣿⡥⠙⣷⣶⣶⣶⣾⣷⣿⣶⣷⣶⣲⣿⣿⣉⣢⡪⣕⡄⣓⣼⣶⣿⣏⣛
//⠉⠉⠉⠉⠉⠉⠉⠉⠉⠉⠠⢀⡟⡀⡟⡀⡀⡀⡀⣀⡠⣄⢲⣲⣖⢒⠲⠣⡆⠐⡀⡀⡀⡀⡀⡀⡃⢈⢨⣅⡤⡲⢕⡢⠤⡀⡀⡀⢿⣧⢆⠻⣽⣫⣞⣽⣯⣽⣻⣹⣿⡞⣿⣾⣷⣿⣿⣿⣮⣿⠻⣿⡸⡶⣭
//⠈⠉⠉⠉⠉⠉⠉⠉⠉⠁⡀⣾⡀⡼⡀⡀⡀⡠⢡⣈⣴⢲⣶⣾⣶⣮⣟⢞⡹⠑⡀⡀⡀⡀⡀⠂⠱⢗⣿⣷⣶⣾⣷⡖⣦⡩⠄⡀⡀⢻⣆⡀⢿⣿⣿⣿⣿⣿⣿⣿⣿⡿⡟⣿⣟⣏⣿⣿⣿⣸⣿⣾⢟⣽⣥
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⢸⢐⣿⢰⡀⡀⡀⡀⡀⡀⡀⠐⠡⠤⠤⠚⠃⠈⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠉⠉⠑⠒⠢⠈⡀⠈⡀⡀⡀⡀⢻⢢⢸⣾⢗⣞⣺⠯⣹⡿⣻⠿⣗⣾⡾⢅⣴⡶⠏⠽⠜⢾⡗⢟⣥
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⢘⣸⡇⠃⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡈⡀⠈⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⣗⠈⠿⠿⣿⣿⣛⡛⠾⢟⣯⣾⣓⡶⣼⣷⠾⣛⢛⡭⢛⢩⣭⢞
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⣾⡀⡀⡀⡀⡀⡀⡀⠄⠈⡀⡀⡀⠠⡀⡀⡀⡀⡀⡀⡀⡀⠠⡐⢐⠆⠠⡀⡀⡀⡀⡀⡀⠄⡀⡀⡀⡀⡀⡀⡀⢿⡀⡀⡀⡀⡀⡀⡀⡀⣀⣒⣫⣴⠯⠭⠶⠿⣋⣶⣷⣿⢍⢞⣡
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡠⠄⡀⡀⡀⡀⡀⢄⡐⠐⣁⣀⡁⢨⠕⡘⠒⣈⣨⣅⠰⠄⢀⡀⡀⠂⢄⡀⡀⡀⡀⡀⡀⡀⡀⡀⠠⠤⣶⣶⣿⣿⣿⣿⣿⣿⣿⣿⡿⢛⣍⢬⣈⢾⣫⡴
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⢈⠁⡀⡁⠲⣄⡀⡀⢆⢪⡀⢿⣿⣿⠟⡨⡔⣈⡀⠿⢿⣿⠏⡅⢌⡢⢀⠊⡀⡀⠂⡀⡀⡀⡀⡀⡀⡀⡀⠈⠁⠈⠩⠿⣿⣿⣿⣿⣿⣿⣿⣩⠮⢖⣬⣿⡭⣥
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠂⡀⡀⡀⡀⠐⡀⡀⠄⠐⠍⠦⣄⣂⠂⠂⡀⠐⠣⡆⠆⠲⠖⠲⠆⠄⣂⣍⣉⠤⠁⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠄⡀⡴⠶⣼⣿⣿⣿⣿⣿⣾⣿⣿⣷⣯⡶⣃⣋
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⢀⡀⠄⢀⢀⠈⢈⠁⠐⡀⡀⠉⠘⠎⠏⠽⡟⢛⣛⣛⣛⡻⢏⠭⠝⡃⡀⡀⡀⡀⠄⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⢉⠉⣼⣭⣿⣿⣿⣿⣿⣾⣽⣿⣿⣶⣿⣿⣴
//⡀⡀⡀⡀⡀⡀⡀⡀⡀⢠⣤⣄⡀⢂⠂⠂⡀⠁⡀⡀⡀⠂⠠⠠⡁⡂⡐⠒⡆⠨⠪⠃⠉⠁⠑⠂⠃⠁⠈⠈⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⡀⠁⠉⠽⢿⣿⣿⣿⣿⣿⣿⣿⡿⣿⣿⣭⣼⣽
//⡀⡀⡀⡀⡀⢀⡀⢀⣀⣾⣿⣿⣦⡀⠔⠢⡅⠈⢑⡣⡔⣂⡄⢈⢱⣇⡆⢜⡈⢀⢘⢑⡄⠠⡑⢄⡀⡀⢂⢄⢨⡈⠁⠂⠠⠐⠠⠁⠄⢀⣄⡀⡀⡀⡀⡀⡀⡀⡀⡀⠈⡀⡀⡀⡀⡓⣿⣧⡿⣿⣷⣒⣉⣉⠖
//⠐⠂⡀⡀⡀⢀⡾⢛⡛⣿⣿⣿⣿⣿⣿⣶⣵⢎⢱⡕⢪⣕⣕⡖⢖⢪⡧⢕⢲⢮⢸⡪⢧⢭⡒⢌⣁⡘⢱⡬⡌⠰⠣⢑⡨⠘⠄⠈⢸⣿⣿⣿⣦⡀⡀⡀⢀⡀⡀⡀⡀⡀⢀⠠⠈⣤⠦⠖⡿⣿⡶⠿⠷⢞⣽
//⣤⣤⣤⡤⠾⡻⢞⣫⣮⣷⣯⣿⣿⣿⣿⣿⣿⣿⣯⣽⣮⣮⣹⣇⣸⣏⢱⢎⣹⡇⢱⡳⠞⢩⡕⣙⢣⡨⢕⡆⡌⢑⡡⣑⠪⠐⡀⢀⡀⠻⣿⣿⣿⣿⣓⣄⡀⡀⡀⡀⡀⡀⠉⢁⣿⣿⣿⢟⣿⢷⣟⢉⠶⢈⠸
//⣿⣿⣿⣿⣿⣿⣿⣿⣻⢿⣟⣏⣻⣿⣿⣿⢿⣿⣿⣿⣿⣿⣯⡿⢿⣿⣾⣧⣱⣳⣮⣕⣪⣬⢇⣸⣇⣸⡵⢸⡇⡘⢂⠌⠪⠠⢱⡢⢸⡆⢈⣿⣿⣿⣿⣤⣷⢧⡀⠤⠄⠌⠉⠲⣟⣛⠺⣷⢿⢞⣿⣛⣾⣫⣿
//⣿⣛⣿⣿⣿⣿⣿⣿⣿⣿⣽⣯⣽⣽⣿⣿⣿⢿⣷⣾⡿⣹⣽⣿⣿⣷⡾⣧⣭⣭⡎⢪⡪⢕⡪⢪⢕⡑⣊⡊⢑⡡⠌⡂⡀⠌⡌⠸⡣⢼⣾⣟⣿⣿⣿⣋⣧⣿⡝⣦⡀⠐⡶⣹⣿⣛⣛⣫⣋⣿⣛⡿⣻⣷⣾
//⣻⣿⣿⣿⣮⣿⣿⣿⣿⣿⣿⣷⣶⣾⣿⣿⣿⡿⢷⣛⣻⣟⣻⣟⣻⢿⣿⣿⣦⣅⣉⠝⡋⡉⢙⠋⢚⠓⠒⠕⡨⢌⣂⡀⡀⡀⣈⣰⡶⠏⠱⣺⢯⣿⣿⣿⣿⠿⢿⢼⢍⣿⣿⣷⣿⣿⣟⣻⠛⣛⣿⣿⣿⣿⣿
//⣿⣿⣯⣽⣿⣿⣿⣿⣿⣿⣿⣿⣯⡿⣭⡶⣫⣝⢯⣳⡽⢯⡵⢷⣿⢿⣯⢿⡮⣟⡿⣿⣿⣶⣛⢿⣻⢷⡶⢶⡯⢟⡻⡻⢟⢏⢱⡮⢾⣞⡷⠞⢩⣕⢳⢇⡪⣹⢯⢽⢞⣚⣱⢎⡙⢾⢣⣧⡿⠿⢃⠾⠷⠿⡿
//⣿⣷⣿⡿⣷⣷⣿⢿⣿⣿⣿⡿⣵⣫⡽⣗⢽⡯⡽⣻⣗⣞⡳⢮⣗⡮⣳⢾⡯⢽⡿⣎⢿⣿⣿⣿⣶⣭⡚⢟⣻⣟⣛⡳⡿⢿⡽⠮⢇⡩⣕⡭⡳⣗⣯⣭⢮⣞⣝⣝⣝⡵⡳⡫⢝⣕⣪⠻⠶⢿⢥⣓⠞⡻⣻
//⣿⣿⣟⢿⣾⣿⣿⢿⣼⣿⣿⣼⢷⣗⡮⣿⠷⠛⠋⠉⠉⣉⣉⠉⠉⠙⠛⠷⣾⣝⡳⢽⣧⣿⣿⣿⣿⣿⣿⣧⢺⡳⡳⠻⣛⣙⣣⡆⢸⢝⣝⣝⣮⡄⡀⡀⢘⢎⣎⣕⢳⣳⡼⣪⡮⡗⡳⣫⡎⣛⡱⣰⣄⠁⣈
//⣿⣿⣟⣿⣷⣟⣿⣿⣿⣿⣿⣷⣾⣗⢽⣿⣀⣀⣠⣾⡿⡺⢽⡯⣧⣄⣀⣀⡿⡯⣳⣗⣾⣿⣿⣿⣿⣟⣿⣿⡆⣻⢟⣟⣛⢻⠷⢇⣎⣺⢽⢞⡳⡝⢦⡀⠙⢧⣱⣓⣪⣗⠷⡗⢜⡮⢮⢮⡝⣒⣛⣛⡋⣉⣽
//⣽⣿⣿⣿⣯⣿⣿⣿⣿⣿⣿⣜⣯⡯⣺⣞⡮⣝⡮⢽⡯⣳⡮⣺⣟⢯⣺⡯⢽⡽⢟⣳⣡⣿⣿⣟⣿⣿⢿⡿⣷⡟⠳⣾⡧⣺⣗⣞⠣⢵⡫⢝⣝⡮⡗⢏⡳⢦⣀⡀⡀⡀⠈⣗⣜⣣⢞⣫⢥⣽⢷⢏⣿⡻⣯
//⣿⣿⣷⣽⡿⣿⣿⣿⣿⣟⣿⣿⣷⣓⢽⡾⣝⣺⣳⢾⡷⢽⣗⢽⡯⣺⣗⣺⣗⡾⣛⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣝⣻⣎⣱⣚⣯⣟⣯⣑⡪⢮⢗⣫⣗⣏⣣⢮⢗⣏⣝⡫⡹⡝⢝⡳⢪⣶⣽⣎⢺⣚⢆⢛⡳
//⣿⣟⣿⡿⣟⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣶⣭⣛⠷⠾⢿⣻⣾⡾⡽⢟⣙⣩⣶⣿⣿⣿⣿⣿⣿⢿⢿⣿⣿⣿⡇⣗⣝⣻⣲⣾⣯⣧⣫⣿⣽⣮⣵⣝⡫⡕⠳⡫⠧⢝⡣⢎⣚⣩⣼⢝⣿⣿⣿⣤⢡⠈⠃⠊⠈
//⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣻⢷⣿⡿⣯⣷⣻⣿⡿⣿⣷⣿⣿⣟⡇⣞⢳⣎⣝⣪⣽⣾⡿⣽⣽⣿⣝⡫⣹⣻⣿⣿⣿⣿⣿⢹⣏⡾⣿⠶⣶⡞⣿⣿⣿⣟⠉⢪⡦
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⢩⣭⢻⢛⠛⢛⣛⡏⡏⡟⣛⢿⣛⠻⣿⣷⣟⣻⣿⣿⣿⣿⣿⣷⢿⣿⣌⢽⣝⠷⣮⣵⣯⣿⣽⣦⢞⡟⡙⡧⢚⢻⢛⠻⠟⠻⠟⡛⣟⢻⣘⣿⣿⣯⣉⠧⢬⠄⡈⢉
//⣿⣿⣿⣿⣿⣿⣿⣻⣿⣿⣿⣿⣿⣿⣿⣶⣶⣿⣶⣶⣶⣶⣷⣷⣷⣿⣾⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⣟⣷⣿⣿⠸⠷⠷⠪⡳⣟⣧⢾⣽⣾⡾⡿⢼⡦⢾⣦⣶⣶⡔⣀⣴⣿⣶⡇⣿⣿⠿⣉⣷⢿⣿⣮⣗
//⣿⣿⣿⣿⣿⡿⣽⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣾⣿⣿⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡟⡺⣏⣼⢻⡫⢽⢷⣮⣭⢖⣹⡇⡵⢺⠧⣿⣿⣿⣿⣿⣿⢺⣿⣷⢿⣿⡿⣶⣿⡩⣴⣩⣘
//⢿⠿⢿⡿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠻⢿⣿⣿⣿⣿⣿⣿⣿⢿⠿⠿⡿⡽⢿⠿⡯⢿⢿⢻⠿⠿⠿⠿⢿⠻⠿⣧⠙⠜⠑⠼⠵⠯⠛⠟⠟⠻⠷⠽⠷⠮⠓⣿⣿⣿⣿⢻⢻⣿⠿⠿⠻⠶⠷⠶⠾⡬⠁⠕⠍



