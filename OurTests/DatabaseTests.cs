using DbManager;

namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        public void CrateTable()
        {
            var database = Database.CreateTestDatabase;

        }
        
        [Fact]
        public void DropTable()
        {
            var db = Database.CreateTestDatabase(); 

            bool resultado = db.DropTable(Table.TestTableName);

            Assert.True(resultado);
            Assert.Equal(Constants.DropTableSuccess, db.LastErrorMessage);

            bool resultadoSegundaVez = db.DropTable(Table.TestTableName);
            Assert.False(resultadoSegundaVez);
        }

        [Fact]
        public void DropTable_Failure_TableDoesNotExist()
        {
            var db = Database.CreateTestDatabase();
            string nombreInventado = "TablaQueNoExiste";

            bool resultado = db.DropTable(nombreInventado);

            Assert.False(resultado);

            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        }

        [Fact]
        public void Insert()
        {

        }
    }
}