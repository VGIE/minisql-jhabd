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

            tabla = new CreateTable(nombre, null);

            result =  tabla.Execute(testDb);

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

        [Fact]
        public void CreateTableRepetidoTest()
        {
            String nombre = "manin";
            List<ColumnDefinition> columns= new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "id"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));

            CreateTable tabla = new CreateTable(nombre,columns);
            Database testDb = new Database("db", "contraseña");
            tabla.Execute(testDb);

            CreateTable tablaErronea = new CreateTable(nombre,columns);
            string resultErroneo =  tablaErronea.Execute(testDb);

            Assert.Equal(Constants.TableAlreadyExistsError, resultErroneo);
        }

        [Fact]
        public void InsertTest() 
        {
            String nombre = "manin";
            Database testDb = new Database("db", "contraseña");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "saludo"));
            CreateTable crearTabla = new CreateTable(nombre, columns);
            crearTabla.Execute(testDb);

            List<String> values = new List<String>();
            values.Add("hola");
            Insert tabla = new Insert(nombre, values);

            string result = tabla.Execute(testDb);

            Assert.Equal(Constants.InsertSuccess, result);
        }

        [Fact]
        public void InsertErroneoTest()
        {
            String nombre = "manin";
            List<String> values= new List<String>();

            Insert tabla = new Insert(nombre,values);
            Database testDb = new Database("db", "contraseña");
            string result =  tabla.Execute(testDb);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void DeleteErroneoTest()
        {
            Database testDb = new Database("db", "contraseña");
            String nombreTabla = "manin";

            Condition condicion = new Condition("id", "=", "1"); 

            Delete deleteQuery = new Delete(nombreTabla, condicion);
            string result = deleteQuery.Execute(testDb);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void DeleteTest()
        {
            Database testDb = new Database("db", "contraseña");
            String nombreTabla = "usuarios";

            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));
            CreateTable crearTabla = new CreateTable(nombreTabla, columnas);
            crearTabla.Execute(testDb); 

            List<String> valores = new List<String>();
            valores.Add("Jon");
            Insert insertQuery = new Insert(nombreTabla, valores);
            insertQuery.Execute(testDb);

            Condition condicion = new Condition("nombre", "=", "Jon");
            Delete deleteQuery = new Delete(nombreTabla, condicion);

            string result = deleteQuery.Execute(testDb);

            Assert.Equal(Constants.DeleteSuccess, result);
        }

        [Fact]
        public void DropTableErroneoTest()
        {
            String nombre = "manin";
            List<ColumnDefinition> columns= new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "id"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));

            DropTable tabla = new DropTable(nombre);
            Database testDb = new Database("db", "contraseña");
            string result =  tabla.Execute(testDb);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }

        [Fact]
        public void DropTableTest()
        {
            String nombre = "manin";
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "id"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));

            Database testDb = new Database("db", "contraseña");

            CreateTable tablacreada = new CreateTable(nombre, columns);

            String creada = tablacreada.Execute(testDb);

            DropTable tabla = new DropTable(nombre);

            string result = tabla.Execute(testDb);

            Assert.Equal(Constants.DropTableSuccess, result);
        }

        [Fact]
        public void ExecuteSelect()
        {
            var database = Database.CreateTestDatabase();
            var table = Table.CreateTestTable();
            Assert.Equal(table.Select(["Name"], null).ToString(), database.ExecuteMiniSQLQuery("SELECT Name FROM TestTable"));
            Assert.Equal(Constants.ColumnDoesNotExistError, database.ExecuteMiniSQLQuery("SELECT Namee FROM TestTable"));
        }

        [Fact]
        public void ExecuteException()
        {
            var database = Database.CreateTestDatabase();
            Assert.Equal(Constants.SyntaxError, database.ExecuteMiniSQLQuery("SELECT FROM"));
        }

        [Fact]
        public void ExecuteUpdate()
        {
            var database = Database.CreateTestDatabase();

            Assert.Equal(Constants.UpdateSuccess, database.ExecuteMiniSQLQuery("UPDATE TestTable SET Name='Walter' WHERE Age>'30'"));
            Assert.Equal(Constants.ColumnDoesNotExistError, database.ExecuteMiniSQLQuery("UPDATE TestTable SET Name='Walter' WHERE A>'30'"));
            Assert.Equal(Constants.TableDoesNotExistError, database.ExecuteMiniSQLQuery("UPDATE TestTablee SET Name='Walter' WHERE Age>'30'"));
            Assert.Equal(Constants.SyntaxError, database.ExecuteMiniSQLQuery("UPDATE TestTable SET Name='Walter' WHERE Age>'30' AND"));
        }
    }
}
