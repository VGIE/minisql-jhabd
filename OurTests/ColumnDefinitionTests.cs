using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1.A : Create your own tests for ColumnDefinition
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

        [Fact]
        public void Parse()
        {
            ColumnDefinition columna = ColumnDefinition.Parse("Edad->Int");

            Assert.Equal("Edad", columna.Name);
            Assert.Equal(ColumnDefinition.DataType.Int, columna.Type);
        }
    }
}