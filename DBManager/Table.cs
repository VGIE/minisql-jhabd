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
            //TODO DEADLINE 1.A: Initialize member variables
            Name = name;
            ColumnDefinitions = columns;
            
        }

        public Row GetRow(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th row

            if (i >= 0 && i < Rows.Count)
            {
                return Rows[i];
            }
            else
            {
                return null;
   
            }    
            
        }

        public void AddRow(Row row)
        {
            //TODO DEADLINE 1.A: Add a new row
            if (row != null)
            {
                Rows.Add(row);
            }
        }

        public int NumRows()
        {
            //TODO DEADLINE 1.A: Return the number of rows
            int numero = Rows.Count;
            return numero;
            
        }

        public ColumnDefinition GetColumn(int i)
        {
            //TODO DEADLINE 1.A: Return the i-th column
            
            if (i >= 0 && i < ColumnDefinitions.Count)
            {
                return ColumnDefinitions[i];
            }
            else
            {
                return null;
   
            }               
        }

        public int NumColumns()
        {
            //TODO DEADLINE 1.A: Return the number of columns
            
            int numero = ColumnDefinitions.Count;
            return numero;            
        }
        
        public ColumnDefinition ColumnByName(string column)
        {
            //TODO DEADLINE 1.A: Return the number of columns
            
            if(column != null)
            {
                foreach (ColumnDefinition col in ColumnDefinitions)
                {
                    if (col.Name == column)
                    {
                        return col;
                    }
                }
            }

            return null;          
        }
        
        public int ColumnIndexByName(string columnName)
        {
            //TODO DEADLINE 1.A: Return the zero-based index of the column named columnName
            
            if(columnName != null)
            {
                for (int i = 0; i < ColumnDefinitions.Count; i++)
                {
                    if (string.Equals(ColumnDefinitions[i].Name, columnName))
                    {
                        return i;
                    }
                }
            }
            return -1;        
        }


        public override string ToString()
        {
            //TODO DEADLINE 1.A: Return the table as a string. The format is specified in the documentation
            //Valid examples:
            //"['Name']{'Adolfo'}{'Jacinto'}" <- one column, two rows
            //"['Name','Age']{'Adolfo','23'}{'Jacinto','24'}" <- two columns, two rows
            //"" <- no columns, no rows
            //"['Name']" <- one column, no rows
            
            string result;

            if(ColumnDefinitions == null || ColumnDefinitions.Count == 0)
            {
                result = "";
                return result;
            }
            
            result = "['";
            for(int i = 0; i < ColumnDefinitions.Count; i++)
            {
                result += ColumnDefinitions[i].Name;
                if(i < ColumnDefinitions.Count - 1)
                {
                    result += "','";
                }
            }
            result += "']";
            
            if(Rows != null && Rows.Count > 0)
            {
                foreach(Row row in Rows)
                {
                    result += "{'";
                    for(int i = 0; i < row.Values.Count; i++)
                    {
                        result += row.Values[i];
                        if(i < row.Values.Count - 1)
                        {
                            result += "','";
                        }
                    }
                    result += "'}";
                }
            }
            
            return result;
        }

        public void DeleteIthRow(int row)
        {
            //TODO DEADLINE 1.A: Delete the i-th row. If there is no i-th row, do nothing

            if (row >= 0 && row < Rows.Count)
            {
            Rows.RemoveAt(row);
            }
        }

        private List<int> RowIndicesWhereConditionIsTrue(Condition condition)
        {
            //TODO DEADLINE 1.A: Returns the indices of all the rows where the condition is true. Check Row.IsTrue()
            var indices = new List<int>();
            for (int i = 0; i < Rows.Count; i++)
                if (Rows[i].IsTrue(condition)) indices.Add(i);
            return indices;
        }

        public void DeleteWhere(Condition condition)
        {
            //TODO DEADLINE 1.A: Delete all rows where the condition is true. Check RowIndicesWhereConditionIsTrue()
            var indices = RowIndicesWhereConditionIsTrue(condition);
            for (int i= indices.Count - 1; i >= 0; i--)
                Rows.RemoveAt(indices[i]);
            
        }

        public Table Select(List<string> columnNames, Condition condition)
        {
            //TODO DEADLINE 1.A: Return a new table (with name 'Result') that contains the result of the select. The condition
            //may be null (if no condition, all rows should be returned). This is the most difficult method in this class
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
            //TODO DEADLINE 1.A: Insert a new row with the values given. If the number of values is not correct, return false. True otherwise
            if (values.Count != NumColumns()) return false;
            Rows.Add(new Row(ColumnDefinitions, values));
            
            return true;
        }

        public bool Update(List<SetValue> setValues, Condition condition)
        {
            //TODO DEADLINE 1.A: Update all the rows where the condition is true using all the SetValues (ColumnName-Value). If condition is null,
            //return false, otherwise return true
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