using System.Collections.Generic;

namespace DbManager.Security
{
    public class Profile
    {
        public const string AdminProfileName = "Admin";
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, List<Privilege>> PrivilegesOn { get; private set; } = new Dictionary<string, List<Privilege>>();

        public bool GrantPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Grant this privilege on this table. Return false if there is an error, true otherwise
            if (!PrivilegesOn.TryGetValue(table, out List<Privilege> privs))
            {
                privs = [];
                PrivilegesOn[table] = privs;
            }

            if (!privs.Contains(privilege))
                privs.Add(privilege);

            return true;    // If the privilege is already granted, we consider it as a success
        }

        public bool RevokePrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Revoke this privilege on this table. Return false if there is an error, true otherwise
            return PrivilegesOn.TryGetValue(table, out List<Privilege> privs) && privs.Remove(privilege);   // If the privilege is not found, we consider it as false
        }

        public bool IsGrantedPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return whether this profile is granted this privilege on this table
            return PrivilegesOn.TryGetValue(table, out List<Privilege> privs) && privs.Contains(privilege);
        }
    }
}
