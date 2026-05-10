using DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DbManager.Network
{
    public static class XmlDeserializer
    {
        public static bool ParseOpen(string command, out string database, out string username, out string password)
        {
            //TODO DEADLINE 6: Try to parse the xml command using the specified xml format (eGela)
            database = null;
            username = null;
            password = null;

            Match match = Regex.Match(command, @"^<Open Database=""(?<database>[^""]+)"" User=""(?<username>[^""]+)"" Password=""(?<password>[^""]+)""/>$");
            if (!match.Success) return false;

            database = match.Groups["database"].Value;
            username = match.Groups["username"].Value;
            password = match.Groups["password"].Value;
            return true;
        }

        public static bool ParseOpenCreateAnswer(string answer, out string error)
        {
            //TODO DEADLINE 6: Try to parse the answer to an Open/Create command.
            error = null;

            if (answer == XmlSerializer.OpenCreateSuccess) return true;

            Match match = Regex.Match(answer, @"^<Error>(?<error>.*)</Error>$");
            if (!match.Success) return false;

            error = match.Groups["error"].Value;
            return false;
        }

        public static bool ParseCreate(string command, out string database, out string username, out string password)
{
            //TODO DEADLINE 6: Try to parse a Create xml command using the specified xml format (eGela)
            database = null;
            username = null;
            password = null;

            Match match = Regex.Match(command, @"^<Create Database=""(?<database>[^""]+)"" User=""(?<username>[^""]+)"" Password=""(?<password>[^""]+)""/>$");
            if (!match.Success) return false;

            database = match.Groups["database"].Value;
            username = match.Groups["username"].Value;
            password = match.Groups["password"].Value;
            return true;
        }

        public static bool ParseQuery(string answer, out string query)
        {
            //TODO DEADLINE 6: Try to parse a Query xml command using the specified xml format (eGela)
            query = null;

            Match match = Regex.Match(answer, @"^<Query>(?<query>.*)</Query>$");
            if (!match.Success) return false;

            query = match.Groups["query"].Value;
            return true;
        }

        public static bool ParseQueryAnswer(string answer, out string answerContent)
        {
            //TODO DEADLINE 6: Try to parse the answer to a Query command.
            answerContent = null;

            Match match = Regex.Match(answer, @"^<Answer><Error>(?<error>.*)</Error></Answer>$");
            if (match.Success)
            {
                answerContent = match.Groups["error"].Value;
                return false;
            }

            match = Regex.Match(answer, @"^<Answer>(?<answer>.*)</Answer>$");
            if (!match.Success) return false;

            answerContent = match.Groups["answer"].Value;
            return true;
        }

        public static bool IsCloseCommand(string command)
        {
            return command == XmlSerializer.CloseConnection;
        }
    }
}
