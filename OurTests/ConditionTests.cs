using DbManager;

namespace OurTests
{
    public class ConditionTests
    {
        //TODO DEADLINE 1.A : Create your own tests for Condition
        [Fact]
        public void IsTrueMayorQue()
        {
            var condition = new Condition("Edad", ">", "35");
            var conditionStr = new Condition("Nombre", ">", "cd");

            Assert.True(condition.IsTrue("40", ColumnDefinition.DataType.Int));
            Assert.False(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.True(condition.IsTrue("40.0", ColumnDefinition.DataType.Double));
            Assert.False(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.True(conditionStr.IsTrue("ef", ColumnDefinition.DataType.String));
            Assert.False(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueMenorQue()
        {
            var condition = new Condition("Edad", "<", "35");
            var conditionStr = new Condition("Nombre", "<", "cd");

            Assert.False(condition.IsTrue("40", ColumnDefinition.DataType.Int));
            Assert.True(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.False(condition.IsTrue("40.0", ColumnDefinition.DataType.Double));
            Assert.True(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.False(conditionStr.IsTrue("ef", ColumnDefinition.DataType.String));
            Assert.True(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueIgual()
        {
            var condition = new Condition("Edad", "=", "35");
            var conditionStr = new Condition("Nombre", "=", "cd");

            Assert.True(condition.IsTrue("35", ColumnDefinition.DataType.Int));
            Assert.False(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.True(condition.IsTrue("35.0", ColumnDefinition.DataType.Double));
            Assert.False(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.True(conditionStr.IsTrue("cd", ColumnDefinition.DataType.String));
            Assert.False(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueMayorIgual()
        {
            var condition = new Condition("Edad", ">=", "35");
            var conditionStr = new Condition("Nombre", ">=", "cd");

            Assert.True(condition.IsTrue("35", ColumnDefinition.DataType.Int));
            Assert.True(condition.IsTrue("40", ColumnDefinition.DataType.Int));
            Assert.False(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.True(condition.IsTrue("35.0", ColumnDefinition.DataType.Double));
            Assert.True(condition.IsTrue("40.0", ColumnDefinition.DataType.Double));
            Assert.False(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.True(conditionStr.IsTrue("cd", ColumnDefinition.DataType.String));
            Assert.True(conditionStr.IsTrue("ef", ColumnDefinition.DataType.String));
            Assert.False(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueMenorIgual()
        {
            var condition = new Condition("Edad", "<=", "35");
            var conditionStr = new Condition("Nombre", "<=", "cd");

            Assert.True(condition.IsTrue("35", ColumnDefinition.DataType.Int));
            Assert.False(condition.IsTrue("40", ColumnDefinition.DataType.Int));
            Assert.True(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.True(condition.IsTrue("35.0", ColumnDefinition.DataType.Double));
            Assert.False(condition.IsTrue("40.0", ColumnDefinition.DataType.Double));
            Assert.True(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.True(conditionStr.IsTrue("cd", ColumnDefinition.DataType.String));
            Assert.False(conditionStr.IsTrue("ef", ColumnDefinition.DataType.String));
            Assert.True(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueNoIgual()
        {
            var condition = new Condition("Edad", "!=", "35");
            var conditionStr = new Condition("Nombre", "!=", "cd");

            Assert.False(condition.IsTrue("35", ColumnDefinition.DataType.Int));
            Assert.True(condition.IsTrue("30", ColumnDefinition.DataType.Int));

            Assert.False(condition.IsTrue("35.0", ColumnDefinition.DataType.Double));
            Assert.True(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            Assert.False(conditionStr.IsTrue("cd", ColumnDefinition.DataType.String));
            Assert.True(conditionStr.IsTrue("ab", ColumnDefinition.DataType.String));
        }

        [Fact]
        public void IsTrueCasoExcepcional()
        {
            var condition = new Condition("Edad", "", "35");

            Assert.Equal("Edad", condition.ColumnName); // Correct initialization check

            Assert.False(condition.IsTrue("30", ColumnDefinition.DataType.Int));    // Check for null operator
            Assert.False(condition.IsTrue("30", ColumnDefinition.DataType.String));
            Assert.False(condition.IsTrue("30.0", ColumnDefinition.DataType.Double));

            var tipo = (ColumnDefinition.DataType)999;   // Invalid DataType check

            Assert.False(condition.IsTrue("", tipo));   // Non existing column check
        }
    }
}
