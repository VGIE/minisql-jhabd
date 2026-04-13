using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
            //TODO DEADLINE 4: Initialize member variables
            Username = username;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, UserDoesNotExistError, DeleteUserSuccess
            if (database.SecurityManager.UserByName(Username) == null)
                return Constants.UserDoesNotExistError;

            if (!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            database.SecurityManager.ProfileByUser(Username).Users.Remove(database.SecurityManager.UserByName(Username));

            return Constants.DeleteUserSuccess;
        }
    }
}
