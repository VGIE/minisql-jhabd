using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
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
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege

            if(database.SecurityManager.ProfileByName(ProfileName) == null)
            return Constants.SecurityProfileDoesNotExistError;
            
            if(p.IsGrantedPrivilege( TableName, a))
            return Constants.ProfileAlreadyHasPrivilege;
        
            if(p.GrantPrivilege(TableName, a))
            return Constants.GrantPrivilegeSuccess;

          
            return "";            
        }

    }
}
