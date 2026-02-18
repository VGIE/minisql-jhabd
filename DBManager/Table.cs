using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DbManager
{
    public class Table
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        private List<Row> Rows = new List<Row>();
        
        public string Name { get; private set; } = null;

        public Table(string name, List<ColumnDefinition> columns)
        {
            Name = name;
            ColumnDefinitions = columns;
            
        }

        public Row GetRow(int i)
        {
            return Rows[i];
            
        }

        public void AddRow(Row row)
        {
            Rows.Add(row);
            
        }

        public int NumRows()
        {
            return Rows.Count;
            
        }

        public ColumnDefinition GetColumn(int i)
        {
            return ColumnDefinitions[i];
        }

        public int NumColumns()
        {
            return ColumnDefinitions.Count;
            
        }
        
        public ColumnDefinition ColumnByName(string column)
        {
            foreach (var col in ColumnDefinitions)
                if (col.Name == column) return col;
            return null;
            
        }
        
        public int ColumnIndexByName(string columnName)
        {
            for (int i=0; i < ColumnDefinitions.Count; i++)
                if (ColumnDefinitions[i].Name == columnName) return i;
            return -1;
        }

        public override string ToString()
        {
            if (NumColumns() == 0) return "";
            string result = "[" + string.Join(",", ColumnDefinitions.ConvertAll(c=> $"'{c.Name}'")) + "]";
            foreach (var row in Rows)
                result += "{" + string.Join(",", row.Values.ConvertAll(v => $"'{v}'")) + "}";
            return result;
        }
        public void DeleteIthRow(int row)
        {
            if (row >= 0 && row < Rows.Count)
                Rows.RemoveAt(row);
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
            var indices = new List<int>();
            for (int i = 0; i < Rows.Count; i++)
                if (Rows[i].IsTrue(condition)) indices.Add(i);
            return indices;         
        }

        public void DeleteWhere(Condition condition)
        {
            var indices = RowIndicesWhereConditionIsTrue(condition);
            for (int i = indices.Count - 1; i >= 0; i--)
                Rows.RemoveAt(indices[i]);           
        }

        public Table Select(List<string> columnNames, Condition condition)
        {
            var cols = columnNames.ConvertAll(n => ColumnByName(n));
            var result = new Table("Result", cols);
            foreach (var row in Rows)
            {
                if (condition != null && !row.IsTrue(condition)) continue;
                var values = columnNames.ConvertAll(n => row.GetValue(n));
                result.Insert(values);  
            }
            return result;
        }

        public bool Insert(List<string> values)
        {
            if (values.Count != NumColumns()) return false;
            Rows.Add(new Row(ColumnDefinitions, values));
            return true;
        }

        public bool Update(List<SetValue> setValues, Condition condition)
        {
            if (condition == null) return false;
            var indices = RowIndicesWhereConditionIsTrue(condition);
            foreach (int i in indices)
                foreach (var sv in setValues)
                    Rows[i].SetValue(sv.ColumnName, sv.Value);
            return true;
        }



        //Only for testing purposes
        public const string TestTableName = "TestTable";
        public const string TestColumn1Name = "Name";
        public const string TestColumn2Name = "Height";
        public const string TestColumn3Name = "Age";
        public const string TestColumn1Row1 = "Rodolfo";
        public const string TestColumn1Row2 = "Maider";
        public const string TestColumn1Row3 = "Pepe";
        public const string TestColumn2Row1 = "1.62";
        public const string TestColumn2Row2 = "1.67";
        public const string TestColumn2Row3 = "1.55";
        public const string TestColumn3Row1 = "25";
        public const string TestColumn3Row2 = "67";
        public const string TestColumn3Row3 = "51";
        public const ColumnDefinition.DataType TestColumn1Type = ColumnDefinition.DataType.String;
        public const ColumnDefinition.DataType TestColumn2Type = ColumnDefinition.DataType.Double;
        public const ColumnDefinition.DataType TestColumn3Type = ColumnDefinition.DataType.Int;
        public static Table CreateTestTable(string tableName = TestTableName)
        {
            Table table = new Table(tableName, new List<ColumnDefinition>()
            {
                new ColumnDefinition(TestColumn1Type, TestColumn1Name),
                new ColumnDefinition(TestColumn2Type, TestColumn2Name),
                new ColumnDefinition(TestColumn3Type, TestColumn3Name)
            });
            table.Insert(new List<string>() { TestColumn1Row1, TestColumn2Row1, TestColumn3Row1 });
            table.Insert(new List<string>() { TestColumn1Row2, TestColumn2Row2, TestColumn3Row2 });
            table.Insert(new List<string>() { TestColumn1Row3, TestColumn2Row3, TestColumn3Row3 });
            return table;
        }

        public void CheckForTesting(List<List<string>> rows)
        {
            if (rows.Count != NumRows())
                throw new Exception($"The table has {NumRows()} rows and {rows.Count} were expected");
            int rowIndex = 0;
            foreach (List<string> row in rows)
            {
                if (GetRow(rowIndex).Values.Count != row.Count)
                    if (rows.Count != NumRows())
                        throw new Exception($"The {rowIndex}-th row has {GetRow(rowIndex).Values.Count} values and {row.Count} were expected");

                for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
                {
                    if (GetRow(rowIndex).Values[columnIndex] != row[columnIndex])
                        if (rows.Count != NumRows())
                            throw new Exception($"The [{rowIndex},{columnIndex}] element is {GetRow(rowIndex).Values[columnIndex]} instead of {row[columnIndex]}");
                }

                rowIndex++;
            }
        }
    }
}