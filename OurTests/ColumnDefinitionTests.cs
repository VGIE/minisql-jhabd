using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        [Fact]
        public void AsText()
        {
            ColumnDefinition columnaInt = new(ColumnDefinition.DataType.Int, "Edad");
            ColumnDefinition columnaString = new(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition columnaDouble = new(ColumnDefinition.DataType.Double, "Peso");

            Assert.Equal("Edad->Int", columnaInt.AsText());
            Assert.Equal("Nombre->String", columnaString.AsText());
            Assert.Equal("Peso->Double", columnaDouble.AsText());
        }
    }
}