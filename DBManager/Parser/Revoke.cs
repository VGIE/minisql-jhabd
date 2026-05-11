using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            if(!database.IsUserAdmin())
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;

            Security.Privilege a;
            Profile p = database.SecurityManager.ProfileByName(ProfileName);
            try { a = PrivilegeUtils.FromPrivilegeName(PrivilegeName);}
            catch(Exception)
            {
                return Constants.PrivilegeDoesNotExistError;
            }
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 

            if(database.SecurityManager.ProfileByName(ProfileName) == null)
            return Constants.SecurityProfileDoesNotExistError;

            database.SecurityManager.RevokePrivilege(ProfileName, TableName, a);
            return Constants.RevokePrivilegeSuccess;
            
        }

    }
}
