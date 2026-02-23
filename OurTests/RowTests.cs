using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1.A: Create your own tests for Row
        [Fact]
        public void SetValue()
        {
            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad")
            };

            Row row = new(columns, ["Ana", "30"]);

            row.SetValue("Nombre", "Walter");
            row.SetValue("Edad", "20");

            Assert.Equal("Walter", row.GetValue("Nombre"));
            Assert.Equal("20", row.GetValue("Edad"));
        }

        [Fact]
        public void GetValue()
        {
            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad")
            };

            Row row = new(columns, ["Ana", "30"]);

            Assert.Equal("Ana", row.GetValue("Nombre"));
            Assert.Equal("30", row.GetValue("Edad"));
            Assert.Null(row.GetValue(""));
        }

        [Fact]
        public void IsTrue()
        {
            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad")
            };

            Row row = new(columns, ["Ana", "40"]);

            Assert.True(row.IsTrue(new Condition("Edad", ">", "35")));
            Assert.False(row.IsTrue(new Condition("Nombre", "=", "Walter")));
            Assert.False(row.IsTrue(new Condition("", "", "")));
        }

        [Fact]
        public void AsText()
        {
            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad")
            };

            Row row = new(columns, ["Ana", "40"]);

            Assert.Equal("Ana:40", row.AsText());
            Assert.Equal("", new Row(columns, []).AsText());
        }

        [Fact]
        public void Parse()
        {
            var columns = new List<ColumnDefinition>
            {
                new(ColumnDefinition.DataType.String, "Nombre"),
                new(ColumnDefinition.DataType.Int, "Edad") 
            };

            Row row = Row.Parse(columns, "Ana:40");

            Assert.Equal("Ana", row.GetValue("Nombre"));
            Assert.Equal("40", row.GetValue("Edad"));
        }
    }
}
