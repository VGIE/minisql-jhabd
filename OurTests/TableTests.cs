using DbManager;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table

        [Fact]
        public void GetRow()
        {
            var tabla = Table.CreateTestTable();

            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn1Name);
            ColumnDefinition edadCol= new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row = new Row(columna, new List<string>() { Table.TestColumn1Row1, "30" });
            tabla.AddRow(row);

            var resultado = tabla.GetRow(0);

            Assert.NotNull(resultado);
            Assert.Equal(Table.TestColumn1Row1, resultado.Values[0]);

            resultado = tabla.GetRow(10);

            Assert.Null(resultado);

            resultado = tabla.GetRow(-1);

            Assert.Null(resultado);
        }

        [Fact]
        public void AddRow()
        {
            var tabla = Table.CreateTestTable();
            int rowsAntes = tabla.NumRows();

            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn1Name);
            ColumnDefinition alturaCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn2Name);
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);
            List<ColumnDefinition> columnas = new List<ColumnDefinition>() { nombreCol, alturaCol, edadCol };

            var row = new Row(columnas, new List<string> { "Juan", "1.80", "30" });

            tabla.AddRow(row);

            Assert.Equal(rowsAntes + 1, tabla.NumRows());
            Assert.Equal(row, tabla.GetRow(tabla.NumRows() - 1));
        }

        [Fact]
        public void NumRows()
        {
            var tabla = Table.CreateTestTable();

            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn1Name);
            ColumnDefinition edadCol= new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row = new Row(columna, new List<string>() { Table.TestColumn1Row1, "30" });
            tabla.AddRow(row);

            var resultado = tabla.NumRows();

            Assert.Equal(4, resultado);
        }

        [Fact]
        public void GetColumn()
        {
            var tabla = Table.CreateTestTable();

            var resultado = tabla.GetColumn(0);

            Assert.NotNull(resultado);
            Assert.Equal(Table.TestColumn1Name, resultado.Name);

            resultado = tabla.GetColumn(10);

            Assert.Null(resultado);

            resultado = tabla.GetColumn(-1);

            Assert.Null(resultado);
        }

        [Fact]
        public void NumColumns()
        {
            var tabla = Table.CreateTestTable();

            var resultado = tabla.NumColumns();

            Assert.Equal(3, resultado);
        }

        [Fact]
        public void ColumnByName()
        {
            var tabla = Table.CreateTestTable();

            var resultado = tabla.ColumnByName(Table.TestColumn1Name);

            Assert.NotNull(resultado);

            Assert.Equal(Table.TestColumn1Name, resultado.Name);
        }

        [Fact]
        public void ColumnIndexByName()
        {
            var tabla = Table.CreateTestTable();

            var resultado = tabla.ColumnIndexByName(Table.TestColumn1Name);

            Assert.Equal(0, resultado);

            resultado = tabla.ColumnIndexByName("A");

            Assert.Equal(-1, resultado);
        }

        [Fact]
        public void ToStringTest()
        {
            var tabla = Table.CreateTestTable();

            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn1Name);
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);

            List<ColumnDefinition> columnas = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row = new Row(columnas, new List<string>() { Table.TestColumn1Row1, "30" });
            tabla.AddRow(row);

            var resultado = tabla.ToString();

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Contains(Table.TestColumn1Name, resultado);
            Assert.Contains(Table.TestColumn3Name, resultado);
            Assert.Contains(Table.TestColumn1Row1, resultado);
            Assert.Contains("30", resultado);
            Assert.StartsWith("['", resultado);

            resultado = new Table("a", null).ToString();
            Assert.Equal("", resultado);

            resultado = new Table("a", []).ToString();
            Assert.Equal("", resultado);

            resultado = new Table("a", [new ColumnDefinition(ColumnDefinition.DataType.String, "col1")]).ToString();
            Assert.Equal("['col1']", resultado);
        }

        [Fact]
        public void TableSelectWithoutCondition()
        {
            Table table = Table.CreateTestTable();

            List<string> columnasSeleccionadas = new List<string>
            {
                Table.TestColumn1Name,
                Table.TestColumn3Name
            };

            Table resultTable = table.Select(columnasSeleccionadas, null);

            Assert.NotNull(resultTable);
            Assert.Equal("Result", resultTable.Name);

            Assert.Equal(2, resultTable.NumColumns());
            Assert.Equal(3, resultTable.NumRows());

            Assert.Equal(Table.TestColumn1Name, resultTable.GetColumn(0).Name);
            Assert.Equal(Table.TestColumn3Name, resultTable.GetColumn(1).Name);

            Assert.Equal(Table.TestColumn1Row1, resultTable.GetRow(0).Values[0]);
            Assert.Equal(Table.TestColumn3Row1, resultTable.GetRow(0).Values[1]);

            resultTable = table.Select([], null);
            Assert.Equal(Table.TestColumn1Name, resultTable.GetColumn(0).Name);

            resultTable = table.Select(null, null);
            Assert.Equal(Table.TestColumn1Name, resultTable.GetColumn(0).Name);

            resultTable = table.Select(["*"], null);
            Assert.Equal(Table.TestColumn1Name, resultTable.GetColumn(0).Name);

            resultTable = table.Select(["asdsadasd"], null);
            Assert.Null(resultTable.GetColumn(0));
        }
    }
}
