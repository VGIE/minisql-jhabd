using DbManager;
using DbManager.Security;

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

        [Fact]
        public void ExecuteAddUser()
        {
            var database = Database.CreateTestDatabase();

            database.SecurityManager.AddProfile(new() { Name = "Profile" });
            database.SecurityManager.ProfileByName("Profile").Users.Add(new("Client", "Password"));

            Assert.Equal(Constants.AddUserSuccess, database.ExecuteMiniSQLQuery("ADD USER (Walter, Password, Profile)"));
            Assert.Equal(Constants.SecurityProfileDoesNotExistError, database.ExecuteMiniSQLQuery("ADD USER (Walter, Password, NonExistentProfile)"));

            database.Save("AddUserTest");
            database = Database.Load("AddUserTest", "Client", "Password");

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, database.ExecuteMiniSQLQuery("ADD USER (Walter, Password, Profile)"));
        }

        [Fact]
        public void ExecuteDeleteUser()
        {
            var database = Database.CreateTestDatabase();

            database.SecurityManager.AddProfile(new() { Name = "Profile" });
            database.SecurityManager.ProfileByName("Profile").Users.Add(new("ClientA", "Password"));
            database.SecurityManager.ProfileByName("Profile").Users.Add(new("ClientB", "Password"));

            Assert.Equal(Constants.DeleteUserSuccess, database.ExecuteMiniSQLQuery("DELETE USER ClientB"));
            Assert.Equal(Constants.UserDoesNotExistError, database.ExecuteMiniSQLQuery("DELETE USER A"));

            database.Save("DeleteUserTest");
            database = Database.Load("DeleteUserTest", "ClientA", "Password");

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, database.ExecuteMiniSQLQuery("DELETE USER ClientA"));
        }

        [Fact]
        public void ExecuteCreateSecurityProfile()
        {
            var database = Database.CreateTestDatabase();

            Assert.Equal(Constants.CreateSecurityProfileSuccess, database.ExecuteMiniSQLQuery("CREATE SECURITY PROFILE Profile"));

            database.SecurityManager.ProfileByName("Profile").Users.Add(new("Client", "Password"));

            database.Save("CreateSecurityProfileTest");
            database = Database.Load("CreateSecurityProfileTest", "Client", "Password");

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, database.ExecuteMiniSQLQuery("CREATE SECURITY PROFILE AnotherProfile"));
        }

        
       [Fact]
        public void UserIsNotAdmin()
        {
            var dbAdmin = Database.CreateTestDatabase();

            var perfil = new Profile();
            perfil.Name = "Perfil";
            var usuario = new User("Pepe", "67");
            perfil.Users.Add(usuario);
            dbAdmin.SecurityManager.AddProfile(perfil);

            dbAdmin.Save("TestNoAdminDB");
            var db = Database.Load("TestNoAdminDB", "Pepe", "67");

            var query = new DropSecurityProfile("Perfil");
            string resultado = query.Execute(db);
            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, resultado);
        }

        [Fact]
        public void ProfileDoesNotExist()
        {
            var db = Database.CreateTestDatabase();

            var query = new DropSecurityProfile("churumbel");
            string resultado = query.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, resultado);
        }

        [Fact]
        public void DropProfile()
        {
            var db = Database.CreateTestDatabase(); 

            var perfilPrueba = new Profile();
            perfilPrueba.Name = "Perfil";
            db.SecurityManager.AddProfile(perfilPrueba);

            var query = new DropSecurityProfile("Perfil");
            string resultado = query.Execute(db);

            Assert.Equal(Constants.DropSecurityProfileSuccess, resultado);

            Assert.Null(db.SecurityManager.ProfileByName("Perfil"));

            Assert.Equal(db.ExecuteMiniSQLQuery("DROP SECURITY PROFILE Perfil"), Constants.SecurityProfileDoesNotExistError);
        }

        [Fact]
        public void GrantPrivilege()
        {
            var database = Database.CreateTestDatabase();

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, database.ExecuteMiniSQLQuery("GRANT UPDATE ON TestTable TO Profile"));

            var userProfile = new Profile() { Name = "Profile" };
            var user = new User("Client", "Password");
            userProfile.Users.Add(user);
            database.SecurityManager.AddProfile(userProfile);

            var query = new Grant("ASJCNASKJc", "TestTable", "Profile");
            Assert.Equal(Constants.PrivilegeDoesNotExistError, query.Execute(database));


            Assert.Equal(Constants.GrantPrivilegeSuccess, database.ExecuteMiniSQLQuery("GRANT UPDATE ON TestTable TO Profile"));
            Assert.Equal(Constants.ProfileAlreadyHasPrivilege, database.ExecuteMiniSQLQuery("GRANT UPDATE ON TestTable TO Profile"));

            database.Save("GrantPrivilegeSecTest");
            database = Database.Load("GrantPrivilegeSecTest", "Client", "Password");

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, database.ExecuteMiniSQLQuery("GRANT SELECT ON TestTable TO Profile"));
        }

        [Fact]
        public void RevokePrivilege()
        {
            var database = Database.CreateTestDatabase();

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, database.ExecuteMiniSQLQuery("REVOKE UPDATE ON TestTable TO Profile"));

            var userProfile = new Profile() { Name = "Profile" };
            var user = new User("Client", "Password");
            userProfile.Users.Add(user);
            database.SecurityManager.AddProfile(userProfile);

            var query = new Revoke("ASJCNASKJc", "TestTable", "Profile");
            Assert.Equal(Constants.PrivilegeDoesNotExistError, query.Execute(database));


            database.ExecuteMiniSQLQuery("GRANT UPDATE ON TestTable TO Profile");

            Assert.Equal(Constants.RevokePrivilegeSuccess, database.ExecuteMiniSQLQuery("REVOKE UPDATE ON TestTable TO Profile"));

            database.Save("RevokePrivilegeSecTest");
            database = Database.Load("RevokePrivilegeSecTest", "Client", "Password");

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, database.ExecuteMiniSQLQuery("REVOKE SELECT ON TestTable TO Profile"));
        }
    }
}
