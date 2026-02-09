using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1.A : Create your own tests for Row
        [Fact]
        public void SetValue()
        {
            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition edadCol= new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row  = new Row(columna, new List<string>() { "Ana", "30" });

            row.SetValue("Nombre", "Walter");
            row.SetValue("Edad", "20");

            Assert.Equal("Walter", row.GetValue("Nombre"));
            Assert.Equal("20", row.GetValue("Edad"));

        }

        [Fact]
        public void GetValue()
        {
            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row  = new Row(columna, new List<string>() { "Ana", "30" });

            Assert.Equal("Ana", row.GetValue("Nombre"));
            Assert.Equal("30", row.GetValue("Edad"));
            Assert.Null(row.GetValue(""));
        }

        [Fact]
        public void IsTrue()
        {
            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad");

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row  = new Row(columna, new List<string>() { "Ana", "40" });

            Assert.True(row.IsTrue(new Condition("Edad", ">", "35")));
            Assert.False(row.IsTrue(new Condition("Nombre", "=", "Walter")));
        }
    }
}