using DbManager;
using DbManager.Security;

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
        public void AddTable()
        {
            var database = Database.CreateTestDatabase();

            var tabla = new Table("TablaNueva", null);

            Assert.True(database.AddTable(tabla));
            Assert.False(database.AddTable(null));
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

            resultado = database.CreateTable(nombreInventado, null);

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
            String value2 = "4.20";
            String value3 = "67";

            List<String> values = new List<string>{value1,value2,value3};

            bool resultado = database.Insert(Table.TestTableName, values);

            Assert.True(resultado);
            Assert.Equal(Constants.InsertSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void InsertErrors()
        {
            var db = Database.CreateTestDatabase(); 

            List<string> valido = new List<string> { "klk", "4.20", "67" };
            bool errorTabla = db.Insert("TablaInventada", valido);

            Assert.False(errorTabla);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);


            List<string> incompleto = new List<string> { "klk" };
            bool errorValores = db.Insert(Table.TestTableName, incompleto);

            Assert.False(errorValores);
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);
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

            database.AddTuplesForTesting("TestTable", [
                ["Rodolfo", "30", "1.80"],
                ["Maider", "25", "1.65"],
                ["Rodolfo", "40", "1.75"],
            ]);

            database.DeleteWhere("TestTable", new("Name", "=", "Rodolfo"));
            Assert.Equal(3, database.Select("TestTable", ["Name", "Age", "Height"], null).NumRows());
            Assert.False(database.DeleteWhere("AAAAAAAAA", new("Name", "=", "Maider")));
            Assert.False(database.DeleteWhere("TestTable", new("Address", "=", "Maider")));
            Assert.False(database.DeleteWhere("TestTable", null));

            database.CheckForTesting("TestTable", [
                ["Maider", "1.67", "67"],
                ["Pepe", "1.55", "51"],
                ["Maider", "25", "1.65"]
            ]);
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

            Assert.True(database.Update("TestTable", [new("Name", "")], new("Name", "=", "Eneko")));

            database.CheckForTesting("TestTable", [
                ["Maider", "1.62", "25"],
                ["Maider", "1.67", "67"],
                ["Pepe", "1.55", "51"]
            ]);
        }

        [Fact]
        public void Save()
        {
            string nombre = "Test";
            string ruta = "Test.txt";
            string rutaSeguridad = "Test_Security.txt"; 
            var db = Database.CreateTestDatabase();


            var perfilAdmin = new DbManager.Security.Profile();
            perfilAdmin.Name = "admin_perfil";
            db.SecurityManager.AddProfile(perfilAdmin);

            bool resultado = db.Save(nombre);

            Assert.True(resultado);
            Assert.True(File.Exists(ruta));
            Assert.True(File.Exists(rutaSeguridad));

            // Siempre hacer esto para comprobar ficheros, así no se guardan
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            if (File.Exists(rutaSeguridad))
            {
                File.Delete(rutaSeguridad);
            }
        }

        [Fact]
        public void Load()
        {
            string nombre = "Test1";
            string ruta = "Test1.txt";
            string rutaSeguridad = "Test1_Security.txt";
            var db = Database.CreateTestDatabase();

            var perfilPrueba = new DbManager.Security.Profile();
            perfilPrueba.Name = "perfilPrueba";
            db.SecurityManager.AddProfile(perfilPrueba);

            bool resultado = db.Save(nombre);
            Assert.True(resultado); 

            Database dbCargadaMal = Database.Load(nombre, Database.AdminUsername, "contraseñaFalsa");
            Assert.Null(dbCargadaMal);

            Database dbCargada = Database.Load(nombre, Database.AdminUsername, Database.AdminPassword);

            Assert.NotNull(dbCargada);
            Assert.NotNull(dbCargada.SecurityManager);

            Assert.NotNull(dbCargada.SecurityManager.ProfileByName("perfilPrueba"));

            bool tablaborrada = dbCargada.DropTable(Table.TestTableName);
            Assert.True(tablaborrada);

            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            if (File.Exists(rutaSeguridad))
            {
                File.Delete(rutaSeguridad);
            }

            Assert.Null(Database.Load(nombre, Database.AdminUsername, Database.AdminPassword));
        }

        [Fact]
        public void IsUserAdmin()
        {
            var database = Database.CreateTestDatabase();

            Assert.True(database.IsUserAdmin());
        }

        [Fact]
        public void DatabaseSaveLoadException()
        {
            var database = Database.CreateTestDatabase();

            Assert.False(database.Save("!·$%&/()=?¿"));

            File.WriteAllText("DoofenshmirtzSL" + ".txt", "A");

            Assert.Null(Database.Load("DoofenshmirtzSL", "admin", "admin"));
        }

        [Fact]
        public void RegularQueriesNoPrivilege()
        {
            var database = Database.CreateTestDatabase();
            var user = new User("client1", "userPassword");
            var profile = new Profile { Name = "client" };

            database.SecurityManager.AddProfile(profile);
            profile.Users.Add(user);

            database.Save("RegularQueriesNoPrivilegeTest");
            database = Database.Load("RegularQueriesNoPrivilegeTest", "client1", "userPassword");

            Assert.Null(database.Select("TestTable", ["Name", "Age", "Height"], null));
            Assert.False(database.Insert("TestTable", ["Pepe", "51", "1.55"]));
            Assert.False(database.DeleteWhere("TestTable", null));
            Assert.False(database.Update("TestTable", [new("Name", "Maider")], null));
            Assert.False(database.CreateTable("A",null));
            Assert.False(database.DropTable("A"));
        }
    }
}
