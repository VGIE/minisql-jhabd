using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"^SELECT\s+(?<columns>(\*|\w+(?:,\w+)*))\s+FROM\s+(?<table>\w+)(\s+WHERE\s+(?<column>\w+)\s*(?<op><=|>=|!=|=|<|>)\s*(?<value>[\w\.]+))?$";

            const string insertPattern = @"^INSERT\s+INTO\s+(?<table>\w+)\s+VALUES\s*\(\s*(?<values>'[^']*'(?:,'[^']*')*)\s*\)$";

            const string dropTablePattern = @"DROP\s+TABLE\s+(?<table>[a-zA-Z0-9_]+)\s*;?\s*$";

            //Note: The parsing of CREATE TABLE should accept empty columns "()"`
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"(?i)^CREATE\s+TABLE\s+(?<table>[a-zA-Z0-9_]+)\s*\((?<columns>[^\)]*)\)\s*;?\s*$";

            const string updateTablePattern = @"(?i)^UPDATE\s+(?<table>[a-zA-Z0-9_]+)\s+SET\s+(?<assignments>.+?)(?:\s+WHERE\s+(?<column>\w+)\s*(?<op><=|>=|!=|=|<|>)\s*(?<value>[^\s;]+))?\s*;?\s*$";;

            const string deletePattern = null;


            //TODO DEADLINE 4
            const string createSecurityProfilePattern = null;

            const string dropSecurityProfilePattern = null;

            const string grantPattern = null;

            const string revokePattern = null;

            const string addUserPattern = null;

            const string deleteUserPattern = null;


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.
            if (miniSQLQuery == null)
                return null;

            Match match = Regex.Match(miniSQLQuery, selectPattern);

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                var columns = CommaSeparatedNames(match.Groups["columns"].Value);

                Condition condition = null;

                if (match.Groups["column"].Success)
                    condition = new Condition(match.Groups["column"].Value, match.Groups["op"].Value, match.Groups["value"].Value);

                return new Select(table, columns, condition);
            }

            match = Regex.Match(miniSQLQuery, insertPattern);

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                var values = CommaSeparatedNames(match.Groups["values"].Value);

                for (int i = 0; i < values.Count; i++)
                {
                    values[i] = values[i].Trim().Trim('\'', '"');
                }

                return new Insert(table, values);
            }

            
            match = Regex.Match(miniSQLQuery, createTablePattern);

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                var columnsText = match.Groups["columns"].Value;
                List<string> textosColumnas = new List<string>();

                if (!string.IsNullOrWhiteSpace(columnsText))
                {
                    textosColumnas = CommaSeparatedNames(columnsText);
                }

                List<ColumnDefinition> columns = new List<ColumnDefinition>();

                foreach (string textoColumna in textosColumnas)
                    {
                        string[] partes = textoColumna.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

                        if (partes.Length >= 2)
                        {
                            string nombre = partes[0]; 
                            string tipoTexto = partes[1]; 

                            ColumnDefinition.DataType tipo = System.Enum.Parse<ColumnDefinition.DataType>(tipoTexto, true);

                            columns.Add(new ColumnDefinition(tipo, nombre));
                        }
                    }

                return new CreateTable(table, columns);

            }

            match = Regex.Match(miniSQLQuery, updateTablePattern); 

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                var assignmentsText = match.Groups["assignments"].Value;
                Condition condition = null;

                if (match.Groups["column"].Success)
                {
                    condition = new Condition(match.Groups["column"].Value, match.Groups["op"].Value, match.Groups["value"].Value);
                }

                List<SetValue> columnsToUpdate = new List<SetValue>();
                string[] asignaciones = assignmentsText.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string asig in asignaciones)
                {
                    string[] partes = asig.Split('=');

                    if (partes.Length == 2)
                    {
                        string colName = partes[0].Trim();
                        string colValue = partes[1].Trim(' ', '\''); 

                        columnsToUpdate.Add(new SetValue(colName, colValue));
                    }
                }
                return new Update(table, columnsToUpdate, condition);
            }

            match = Regex.Match(miniSQLQuery, dropTablePattern);

            if (match.Success)
            {
                var table = match.Groups["table"].Value;
                return new DropTable(table);
            }


            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            return null;
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
    }
}
