using DbManager;

namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        public void CreateTable()
        {
            var database = Database.CreateTestDatabase();

            ColumnDefinition col1 = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition col2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");
            List<ColumnDefinition> columnasNuevas = new List<ColumnDefinition>() { col1, col2 };
            String nombreInventado = "TablaNueva";

            bool resultado = database.CreateTable(nombreInventado,columnasNuevas);

            Assert.True(resultado);
            Assert.Equal(Constants.CreateTableSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void CreateTableNombreExistente()
        {
            var database = Database.CreateTestDatabase();

            ColumnDefinition col1 = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition col2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");
            List<ColumnDefinition> columnasNuevas = new List<ColumnDefinition>() { col1, col2 };

            bool resultado = database.CreateTable(Table.TestTableName,columnasNuevas);

            Assert.False(resultado);
            Assert.Equal(Constants.TableAlreadyExistsError, database.LastErrorMessage);
        }

        [Fact]
        public void CreateTableSinColumnas()
        {
            var database = Database.CreateTestDatabase();

            List<ColumnDefinition> columnasNuevas = new List<ColumnDefinition>();
            String nombreInventado = "TablaNueva";

            bool resultado = database.CreateTable(nombreInventado,columnasNuevas);

            Assert.False(resultado);
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, database.LastErrorMessage);
        }

        [Fact]
        public void DropTable()
        {
            var database = Database.CreateTestDatabase(); 

            bool resultado = database.DropTable(Table.TestTableName);

            Assert.True(resultado);
            Assert.Equal(Constants.DropTableSuccess, database.LastErrorMessage);

            bool resultadoSegundaVez = database.DropTable(Table.TestTableName);
            Assert.False(resultadoSegundaVez);
        }

        [Fact]
        public void DropTableTablaInexistente()
        {
            var database = Database.CreateTestDatabase();

            string nombreInventado = "TablaQueNoExiste";

            bool resultado = database.DropTable(nombreInventado);

            Assert.False(resultado);

            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }

        [Fact]
        public void Insert()
        {
            var database = Database.CreateTestDatabase();

            String value1 = "klk";
            String value2 = "manin";
            List<String> values = new List<string>{value1,value2};

            bool resultado = database.Insert(Table.TestTableName, values);

            Assert.True(resultado);
            Assert.Equal(Constants.InsertSuccess, database.LastErrorMessage);
        }
    }
}