using DbManager;

namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1.B : Create your own tests for Database
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

        [Fact]
        public void InsertATablaInexistente()
        {
            var database = Database.CreateTestDatabase();

            String value1 = "klk";
            String value2 = "manin";
            List<String> values = new List<string>{value1,value2};
            String nombre = "tabla";

            bool resultado = database.Insert(nombre, values);

            Assert.False(resultado);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }

        [Fact]
        public void Select()
        {
            var database = Database.CreateTestDatabase();

            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad")
            };

            database.CreateTable("Tabla", columns);
            Assert.Equal(columns.Count, database.Select("Tabla", ["Nombre", "Edad"], null).NumColumns());
            Assert.Null(database.Select("TablAAAAA", ["Nombre", "Edad"], null));
            Assert.Null(database.Select("Tabla", ["Telefono", "Direccion"], null));
        }

        [Fact]
        public void DeleteWhere()
        {
            var database = Database.CreateTestDatabase();

            database.DeleteWhere("TestTable", new("Name", "=", "Rodolfo"));
            Assert.Equal(2, database.Select("TestTable", ["Name", "Age", "Height"], null).NumRows());
            Assert.False(database.DeleteWhere("AAAAAAAAA", new("Name", "=", "Maider")));
            Assert.False(database.DeleteWhere("TestTable", new("Address", "=", "Maider")));
            Assert.False(database.DeleteWhere("TestTable", null));
        }

        [Fact]
        public void Update()
        {
            var database = Database.CreateTestDatabase();

            database.Update("TestTable", [new("Name", "Maider")], new("Name", "=", "Rodolfo"));
            Assert.Equal(2, database.Select("TestTable", ["Name", "Age", "Height"], new("Name", "=", "Maider")).NumRows());
            Assert.False(database.Update("AAAAAAAAA", [new("Name", "Maider")], new("Name", "=", "Maider")));
            Assert.False(database.Update("TestTable", [new("Address", "Maider")], new("Name", "=", "Maider")));
            Assert.False(database.Update("TestTable", [new("Name", "Maider")], new("Address", "=", "Maider")));
            Assert.False(database.Update("TestTable", [new("Name", "Maider")], null));
            Assert.False(database.Update("TestTable", null, new("Name", "=", "Maider")));
            Assert.False(database.Update("TestTable", [], new("Name", "=", "Maider")));
        }

        [Fact]
        public void Save()
        {
            string nombre = "Test";
            string ruta = "Test.txt";
            var db = Database.CreateTestDatabase(); 

            bool resultado = db.Save(nombre);

            Assert.True(resultado);
            Assert.True(File.Exists(ruta));

            //Siempre hacer esto para comprobar ficheros, así no se guardan
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }
                [Fact]
        public void Load()
        {
            string nombre = "Test1";
            string ruta = "Test1.txt";
            var db = Database.CreateTestDatabase(); 

            bool resultado = db.Save(nombre);

            Database dbCargada = Database.Load(nombre, Database.AdminUsername, Database.AdminPassword);

            Assert.NotNull(dbCargada); 

            bool tablaborrada = dbCargada.DropTable(Table.TestTableName);
            Assert.True(tablaborrada);

            //Siempre hacer esto para comprobar ficheros, así no se guardan
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }
    }
}