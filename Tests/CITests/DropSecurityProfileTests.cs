using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;

namespace SecurityParsingTests
{
    public class DropSecurityProfileTests
    {
        
        [Fact]
        public void Correct()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP     SECURITY PROFILE      profile") as DropSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("DROP SECURITY     PROFILE OtherProfile") as DropSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("Drop SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("drop security profile OtherProfile") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE pro-file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE Pro file") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            DropSecurityProfile query = MiniSQLParser.Parse("DROP SECURITY PROFILE ") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE") as DropSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP SECURITY PROFILE profile") as DropSecurityProfile;
            Assert.NotNull(query);
        }

       [Fact]
        public void UserIsNotAdmin()
        {
            var dbAdmin = Database.CreateTestDatabase();

            var perfil = new Profile();
            perfil.Name = "Perfil";
            var usuario = new User("Pepe", "67");
            perfil.Users.Add(usuario);
            dbAdmin.SecurityManager.AddProfile(perfil);

            dbAdmin.Save("TestNoAdminDB");
            var db = Database.Load("TestNoAdminDB", "Pepe", "67");

            var query = new DropSecurityProfile("Perfil");
            string resultado = query.Execute(db);
            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, resultado);
        }

        [Fact]
        public void ProfileDoesNotExist()
        {
            var db = Database.CreateTestDatabase();

            var query = new DropSecurityProfile("churumbel");
            string resultado = query.Execute(db);

            Assert.Equal(Constants.SecurityProfileDoesNotExistError, resultado);
        }

        [Fact]
        public void DropProfile()
        {
            var db = Database.CreateTestDatabase(); 

            var perfilPrueba = new Profile();
            perfilPrueba.Name = "Perfil";
            db.SecurityManager.AddProfile(perfilPrueba);

            var query = new DropSecurityProfile("Perfil");
            string resultado = query.Execute(db);

            Assert.Equal(Constants.DropSecurityProfileSuccess, resultado);

            Assert.Null(db.SecurityManager.ProfileByName("Perfil"));
        }
        
    }
}
