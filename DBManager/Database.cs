using DbManager.Parser;
using DbManager.Security;
using System.Collections.Generic;
using System;
using System.IO;


namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager { get; private set; }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database()
        {
        }

        public Database(string adminUsername, string adminPassword)
        {
            //DEADLINE 1.B: Initalize the member variables
            this.m_username = adminUsername;
            Manager m = new Manager(adminUsername);
            this.SecurityManager = m;
            this.LastErrorMessage = "";


            
        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database
            
            return false;
            
        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name
            
            return null;
            
        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition)
        {
            //DEADLINE 1.B: Create and new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            
            if(ColumnDefinition == null || ColumnDefinition.Count == 0)
            {
                LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                return false;
            }
            
            foreach(Table tabla in Tables)
            {
                if(tabla.Name==tableName)
                {
                    LastErrorMessage = Constants.TableAlreadyExistsError;
                    return false;
                }
            }
            
            LastErrorMessage = Constants.CreateTableSuccess;
            Table nueva = new Table(tableName,ColumnDefinition);
            Tables.Add(nueva);
            return true; 
            
        }

        public bool DropTable(string tableName)
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)

            Table tablaEliminada = null;

            foreach(Table tabla in Tables)
            {
                if(tabla.Name==tableName)
                {
                  tablaEliminada = tabla;
                }
            }

            if(tablaEliminada==null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            else
            {
                LastErrorMessage = Constants.DropTableSuccess;
                Tables.Remove(tablaEliminada);
                return true;
            }
        }

        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            
            Table tablaEncontrada = null;

            foreach(Table tabla in Tables)
            {
                if(tabla.Name==tableName)
                {
                  tablaEncontrada = tabla;
                }
            }

            if(tablaEncontrada==null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            else
            {
                LastErrorMessage = Constants.InsertSuccess;
                tablaEncontrada.Insert(values);
                return true;
            }
        }

        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table
            Table tableC = null;
            foreach (Table table in Tables) // provisional loop to find the table, we will change it later when we have the method TableByName implemented
            {
               if(table.Name == tableName)
               {
                  tableC = table;
               }
            }

            if (tableC == null) // provisional check to see if the table exists, we will change it later when we have the method TableByName implemented
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }

            for (int i = 0; i < columns.Count; i++)
            {
                if (tableC.ColumnByName(columns[i]) == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return null;
                }
            }

            return tableC.Select(columns, condition);
        }

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
            foreach (Table table in Tables) // provisional loop to find the table, we will change it later when we have the method TableByName implemented
            {
                if (table.Name == tableName)
                {
                    if (table.ColumnByName(columnCondition.ColumnName) == null)
                    {
                        LastErrorMessage = Constants.ColumnDoesNotExistError;
                        return false;
                    }
                        table.DeleteWhere(columnCondition);
                        LastErrorMessage = Constants.DeleteSuccess;
                        return true;
                }
            }
            LastErrorMessage = Constants.TableDoesNotExistError;
            return false;
        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
            foreach (Table table in Tables) // provisional loop to find the table, we will change it later when we have the method TableByName implemented
            {
                if (table.Name == tableName)
                {
                    if (table.ColumnByName(columnCondition.ColumnName) == null)
                    {
                        LastErrorMessage = Constants.ColumnDoesNotExistError;
                        return false;
                    }

                    foreach (var setValue in columnNames)
                    {
                        if (table.ColumnByName(setValue.ColumnName) == null)
                        {
                            LastErrorMessage = Constants.ColumnDoesNotExistError;
                            return false;
                        }
                    }

                    table.Update(columnNames, columnCondition);
                    LastErrorMessage = Constants.UpdateSuccess;
                    return true;
                }
            }
            LastErrorMessage = Constants.TableDoesNotExistError;
            return false;
        }

        public bool Save(string databaseName)
        {
            //DEADLINE 1.C: Save this database to disk with the given name
            //If everything goes ok, return true, false otherwise.
            //DEADLINE 5: Save the SecurityManager so that it can be loaded with the database in Load()
            try
                {
                    string filePath = databaseName + ".txt";

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine(Tables.Count);

                        foreach (Table table in Tables)
                        {
                            writer.WriteLine(table.Name);

                            writer.WriteLine(table.NumColumns());
                            for (int i = 0; i < table.NumColumns(); i++)
                            {
                                writer.WriteLine(table.GetColumn(i).AsText());
                            }

                            writer.WriteLine(table.NumRows());
                            for (int i = 0; i < table.NumRows(); i++)
                            {
                                writer.WriteLine(table.GetRow(i).AsText());
                            }
                        }
                    }
                    return true;
                }

            catch (Exception)
            {
                return false;
            }
        }

        public static Database Load(string databaseName, string username, string password)
        {
            //DEADLINE 1.C: Load the (previously saved) database of name databaseName
            //If everything goes ok, return the loaded database (a new instance), null otherwise.
            //DEADLINE 5: When the Database object is created, set the username (create a new method if you must)
            //After loading the database, load the SecurityManager and check the password is correct. If it's not, return null. If it is return the database
            try
                {
                    string filePath = databaseName + ".txt";

                    if (!File.Exists(filePath))
                    {
                        return null;
                    }

                    Database db = new Database();

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        int numberOfTables = int.Parse(reader.ReadLine());

                        for (int t = 0; t < numberOfTables; t++)
                        {
                            string tableName = reader.ReadLine();

                            int numColumns = int.Parse(reader.ReadLine());
                            List<ColumnDefinition> columns = new List<ColumnDefinition>();
                            for (int c = 0; c < numColumns; c++)
                            {
                                string columnText = reader.ReadLine();
                                columns.Add(ColumnDefinition.Parse(columnText));
                            }

                            Table newTable = new Table(tableName, columns);

                            int numRows = int.Parse(reader.ReadLine());
                            for (int r = 0; r < numRows; r++)
                            {
                                string rowText = reader.ReadLine();
                                newTable.AddRow(Row.Parse(columns, rowText));
                            }

                            db.Tables.Add(newTable);
                        }
                    }

                    return db; 
                }
            catch (Exception)
            {
                return null;
            }
        }

        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }

        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }


        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }
    }
}
