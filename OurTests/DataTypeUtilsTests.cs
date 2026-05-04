using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class DataTypeUtilsTests
    {
        [Fact]
        public void FromMiniSQLName()
        {
            var typename = DataTypeUtils.FromMiniSQLName("INT");
            Assert.Equal(ColumnDefinition.DataType.Int, typename);

            typename = DataTypeUtils.FromMiniSQLName("DOUBLE");
            Assert.Equal(ColumnDefinition.DataType.Double, typename);

            typename = DataTypeUtils.FromMiniSQLName("TEXT");
            Assert.Equal(ColumnDefinition.DataType.String, typename);

            Assert.Throws<Exception>(() => DataTypeUtils.FromMiniSQLName("BOOLEAN"));
        }

        [Fact]
        public void FromMiniTypeName()
        {
            var typename = DataTypeUtils.FromMiniTypeName("Int");
            Assert.Equal(ColumnDefinition.DataType.Int, typename);

            typename = DataTypeUtils.FromMiniTypeName("Double");
            Assert.Equal(ColumnDefinition.DataType.Double, typename);

            typename = DataTypeUtils.FromMiniTypeName("String");
            Assert.Equal(ColumnDefinition.DataType.String, typename);

            Assert.Throws<Exception>(() => DataTypeUtils.FromMiniTypeName("Boolean"));
        }
    }
}
