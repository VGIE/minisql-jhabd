using DbManager;

namespace OurTests
{
    public class MiniSQLExecuteTests
    {
        [Fact]
        public void CreateTableVacioTest()
        {
            String nombre = "manin";
            List<ColumnDefinition> columns= new List<ColumnDefinition>();

            CreateTable tabla = new CreateTable(nombre,columns);
            Database testDb = new Database("db", "contraseña");
            string result =  tabla.Execute(testDb);

            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, result);
        }

        [Fact]
        public void CreateTableTest()
        {
            String nombre = "manin";
            List<ColumnDefinition> columns= new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "id"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));

            CreateTable tabla = new CreateTable(nombre,columns);
            Database testDb = new Database("db", "contraseña");
            string result =  tabla.Execute(testDb);

            Assert.Equal(Constants.CreateTableSuccess, result);
        }
    }
}


