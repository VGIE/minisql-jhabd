using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise

            Profile userProfile = ProfileByUser(m_username);

            if (userProfile != null && userProfile.Name == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one

            if (username != null)
            {
                User user = UserByName(username);
                if (Encryption.Encrypt(password) == user.EncryptedPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (!IsUserAdmin()) return;
            Profile profile = ProfileByName(profileName);
            if (profile == null) return;
            profile.GrantPrivilege(table, privilege);
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (!IsUserAdmin()) return;
            Profile profile = ProfileByName(profileName);
            if (profile == null) return;
            profile.RevokePrivilege(table, privilege);
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            if (table == null) return false;
            Profile profile = ProfileByUser(username);
            if (profile == null) return false;
            if (profile.Name == "Admin") return true;
            return profile.IsGrantedPrivilege(table, privilege);
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            
            if (IsUserAdmin()) 
            {
                if (profile != null)
                {
                    Profiles.Add(profile);
                }
            }
        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null
            foreach (Profile profile in Profiles)
            {
                foreach (User user in profile.Users)
                {
                    if (username.Equals(user.Username))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            if (profileName == null)
                return null;

            foreach (Profile profile in Profiles)
            {
                if (profileName.Equals(profile.Name))
                {
                    return profile;
                }
            }

            return null;

        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null

            if (username == null)
            {
                return null;
            }
            foreach (Profile profile in Profiles)
            {
                foreach (User user in profile.Users)
                {
                    if (username.Equals(user.Username))
                    {
                        return profile;
                    }
                }
            }
            return null;

        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            if (!IsUserAdmin()) return false;
            Profile profile = ProfileByName(profileName);
            if (profile == null) return false;
            Profiles.Remove(profile);
            return true;
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username

            Manager manager = new Manager(username);
            string filePath = databaseName + "_Security.txt";

            if (!File.Exists(filePath))
            {
                return manager;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    int numProfiles = int.Parse(reader.ReadLine());

                    for (int p = 0; p < numProfiles; p++)
                    {
                        Profile profile = new Profile();
                        profile.Name = reader.ReadLine();

                        int numUsers = int.Parse(reader.ReadLine());

                        for (int u = 0; u < numUsers; u++)
                        {
                            string uname = reader.ReadLine();
                            string encryptedPass = reader.ReadLine();

                            User user = new User(uname, "");
                            user.EncryptedPassword = encryptedPass;

                            profile.Users.Add(user);
                        }

                        int numTablasConPrivilegios = int.Parse(reader.ReadLine());

                        for (int t = 0; t < numTablasConPrivilegios; t++)
                        {
                            string nombreTabla = reader.ReadLine();
                            int numPrivilegios = int.Parse(reader.ReadLine());

                            for (int pr = 0; pr < numPrivilegios; pr++)
                            {
                                string nombrePrivilegio = reader.ReadLine();

                                Privilege privilegioCargado = (Privilege)Enum.Parse(typeof(Privilege), nombrePrivilegio);
                                profile.GrantPrivilege(nombreTabla, privilegioCargado);
                            }
                        }
                        manager.Profiles.Add(profile);
                    }
                }
                return manager;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.

            string filePath = databaseName + "_Security.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(Profiles.Count);

                foreach (Profile profile in Profiles)
                {
                    writer.WriteLine(profile.Name);

                    writer.WriteLine(profile.Users.Count);

                    foreach (User user in profile.Users)
                    {
                        writer.WriteLine(user.Username);
                        writer.WriteLine(user.EncryptedPassword);
                    }

                    writer.WriteLine(profile.PrivilegesOn.Count);

                    foreach (var tablaYPrivilegios in profile.PrivilegesOn)
                    {
                        writer.WriteLine(tablaYPrivilegios.Key); 
                        writer.WriteLine(tablaYPrivilegios.Value.Count); 

                        foreach (Privilege privilegio in tablaYPrivilegios.Value)
                        {
                            writer.WriteLine(privilegio.ToString());
                        }
                    }
                }
            }
        }
    }
}
