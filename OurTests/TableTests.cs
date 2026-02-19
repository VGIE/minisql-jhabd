using DbManager;
using Xunit;
using System.Collections.Generic;

namespace OurTests
{
    public class TableTests
    {
        [Fact]
        public void GetRow()
        {
            var tabla = Table.CreateTestTable();

            ColumnDefinition nombreCol = new ColumnDefinition(ColumnDefinition.DataType.String, Table.TestColumn1Name);
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);

            List<ColumnDefinition> columna = new List<ColumnDefinition>() { nombreCol, edadCol };

            Row row = new Row(columna, new List<string>() { Table.TestColumn1Row1, "30" });
            tabla.AddRow(row);

            var resultado = tabla.GetRow(0);
            
            Assert.NotNull(resultado);
            Assert.Equal(Table.TestColumn1Row1, resultado.Values[0]);
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
            ColumnDefinition edadCol = new ColumnDefinition(ColumnDefinition.DataType.Int, Table.TestColumn3Name);

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
        }

        [Fact]
        public void Insert_AddsRow()
        {
            var tabla = Table.CreateTestTable();
            tabla.Insert(new List<string> { "Nuevo", "1.80", "30" });
            Assert.Equal(4, tabla.NumRows());
        }

        [Fact]
        public void DeleteIthRow_RemovesCorrectRow()
        {
            var tabla = Table.CreateTestTable();
            tabla.DeleteIthRow(0);
            Assert.Equal(2, tabla.NumRows());
            Assert.Equal(Table.TestColumn1Row2, tabla.GetRow(0).GetValue(Table.TestColumn1Name));
        }

        [Fact]
        public void Select_WithCondition_ReturnsFilteredRows()
        {
            var tabla = Table.CreateTestTable();
            var condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);
            var result = tabla.Select(new List<string> { Table.TestColumn1Name }, condition);
            Assert.Equal(1, result.NumRows());
        }
    }
}