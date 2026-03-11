using DbManager;

namespace OurTests
{
    public class RowTests
    {
        private readonly List<ColumnDefinition> _columns =
        [
            new(ColumnDefinition.DataType.String, "Nombre"),
            new(ColumnDefinition.DataType.Int, "Edad")
        ];

        private readonly Row _row;

        public RowTests()
        {
            _row = new Row(_columns, ["Ana", "40"]);
        }

        //TODO DEADLINE 1.A: Create your own tests for Row
        [Fact]
        public void SetValue()
        {
            _row.SetValue("Nombre", "Walter");
            _row.SetValue("Edad", "30");

            Assert.Equal("Walter", _row.GetValue("Nombre"));
            Assert.Equal("30", _row.GetValue("Edad"));

            _row.Values.RemoveAt(1);

            _row.SetValue("Edad", "50");

            Assert.Equal("50", _row.GetValue("Edad"));
        }

        [Fact]
        public void GetValue()
        {
            Assert.Equal("Ana", _row.GetValue("Nombre"));
            Assert.Equal("40", _row.GetValue("Edad"));
            Assert.Null(_row.GetValue(""));

            Row row = new(_columns, ["Ana"]);
            Assert.Null(row.GetValue("Edad"));
        }

        [Fact]
        public void IsTrue()
        {
            Row row = new(_columns, ["Ana"]);

            Assert.True(_row.IsTrue(new Condition("Edad", ">", "35")));
            Assert.False(_row.IsTrue(new Condition("Nombre", "=", "Walter")));
            Assert.False(_row.IsTrue(new Condition("", "", "")));
            Assert.False(row.IsTrue(new Condition("Edad", ">", "30")));
        }

        [Fact]
        public void AsText()
        {
            Assert.Equal("Ana:40", _row.AsText());
            Assert.Equal("", new Row(_columns, []).AsText());
        }

        [Fact]
        public void Parse()
        {
            Row row = Row.Parse(_columns, "Ana:40");

            Assert.Equal("Ana", row.GetValue("Nombre"));
            Assert.Equal("40", row.GetValue("Edad"));
        }
    }
}
