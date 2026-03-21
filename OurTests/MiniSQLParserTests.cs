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



