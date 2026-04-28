using System.Collections.Generic;

namespace DbManager
{
    public class Insert: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Values { get; private set; }
        public Insert(string table, List<string> values)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            Values = values;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //InsertSuccess or the last error in the database
            database.Insert(Table, Values);

            return database.LastErrorMessage;
        }
    }
}
