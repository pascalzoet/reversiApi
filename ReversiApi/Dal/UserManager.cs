using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ReversiApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiApi.Dal
{
    public class UserManager
    {
        //private readonly string _connectionstring = "Server=(localdb)\\MSSQLLocaldb;Database=Reversi;Trusted_Connection=True;";
        private string _connectionstring { get; set; }


        public UserManager(IConfiguration configuration)
        {
            _connectionstring = configuration.GetSection("CustomConfig").GetValue<String>("ConnectionString");
        }

        public async Task<bool> SignIn(HttpContext HttpContext, Player user, bool IsPersisent = false)
        {
            using (var sqlCon = new SqlConnection(_connectionstring))
            {
                bool ingelogd = false;
                string queryString = "SELECT PlayerId, UserName, Email, Password, UserRole, UserToken FROM Player WHERE UserName=@User";

                sqlCon.Open();

                SqlCommand sqlCmd = new SqlCommand(queryString, sqlCon);
                sqlCmd.Parameters.AddWithValue("@User", user.UserName);
                //sqlCmd.Parameters.AddWithValue("@Password", Hashpwd(user.Password));

                SqlDataReader rdr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                var userModel = new Player();

                if (rdr.Read())
                {
                    if (VerifyPassword(user.Password, rdr["Password"].ToString()))
                    {
                        userModel.PlayerId = Convert.ToInt32(rdr["PlayerId"]);
                        userModel.UserName = rdr["UserName"].ToString();
                        userModel.Email = rdr["Email"].ToString();
                        userModel.Password = rdr["Password"].ToString();
                        userModel.UserRole = rdr["UserRole"].ToString();
                        userModel.UserToken = rdr["UserToken"].ToString();
                        ingelogd = true;
                    }
                }

                sqlCon.Close();

                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(userModel), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return ingelogd;
            }
        }

        public bool RegisterAsync(HttpContext HttpContext, Player user, bool IsPersisent = false)
        {
            bool created = false;
            using (var sqlCon = new SqlConnection(_connectionstring))
            {
                string queryString = "INSERT INTO Player (UserName, Email, Password, UserRole, UserToken) VALUES (@UserName, @Email, @Password, @UserRole, @UserToken)";

                sqlCon.Open();

                SqlCommand sqlCmd = new SqlCommand(queryString, sqlCon);
                sqlCmd.Parameters.AddWithValue("@UserName", user.UserName);
                sqlCmd.Parameters.AddWithValue("@Email", user.Email);
                sqlCmd.Parameters.AddWithValue("@Password", Hashpwd(user.Password));
                sqlCmd.Parameters.AddWithValue("@UserRole", "User");
                sqlCmd.Parameters.AddWithValue("@UserToken", GenerateToken());

                SqlDataReader rdr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                created = true;
                sqlCon.Close();
            }
            return created;
        }

        private string Hashpwd(string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            return hash;
        }

        private bool VerifyPassword(string password, string hashPwd)
        {
            bool hashed = BCrypt.Net.BCrypt.Verify(password, hashPwd);
            return hashed;
        }

        private string GenerateToken()
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token;
        }

        public bool UserExist(HttpContext context, Player player)
        {
            bool exist = false;
            using (var sqlCon = new SqlConnection(_connectionstring))
            {

                string queryString = "SELECT PlayerId, UserName, Email, Password, UserRole, UserToken FROM Player WHERE UserName=@User and Email=@Email";

                sqlCon.Open();

                SqlCommand sqlCmd = new SqlCommand(queryString, sqlCon);
                sqlCmd.Parameters.AddWithValue("@User", player.UserName);
                sqlCmd.Parameters.AddWithValue("@Email", player.Email);

                SqlDataReader rdr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (rdr.Read())
                {
                    exist = true;
                }
                sqlCon.Close();
            }
            return exist;
        }

        private IEnumerable<Claim> GetUserClaims(Player player)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, player.PlayerId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, player.UserName));
            claims.Add(new Claim(ClaimTypes.Email, player.Email));
            claims.AddRange(this.GetUserRoleClaims(player));
            return claims;
        }

        public Player GetLoggedinUser(HttpContext context)
        {
            var UserId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            using (var sqlCon = new SqlConnection(_connectionstring))
            {
                string queryString = "SELECT PlayerId, UserName, Email, Password, UserRole, UserToken FROM Player WHERE PlayerId=@Id";

                sqlCon.Open();

                SqlCommand sqlCmd = new SqlCommand(queryString, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", UserId);
                //sqlCmd.Parameters.AddWithValue("@Password", Hashpwd(user.Password));

                SqlDataReader rdr = sqlCmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                var userModel = new Player();

                if (rdr.Read())
                {
                    userModel.PlayerId = Convert.ToInt32(rdr["PlayerId"]);
                    userModel.UserName = rdr["UserName"].ToString();
                    userModel.Email = rdr["Email"].ToString();
                    userModel.UserRole = rdr["UserRole"].ToString();
                    userModel.UserToken = rdr["UserToken"].ToString();
                }

                sqlCon.Close();
                return userModel;
            }
        }

        private IEnumerable<Claim> GetUserRoleClaims(Player player)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, player.PlayerId.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, player.UserRole.ToString()));
            return claims;
        }

        public async void SignOut(HttpContext context)
        {
            await context.SignOutAsync();
        }

        public List<Player> GetAllUsers(bool admin)
        {
            var userList = new List<Player>();
            string query = "SELECT PlayerId, UserName, Email, Password, UserRole, UserToken From Player";

            using (SqlConnection sqlcon = new SqlConnection(_connectionstring))
            {
                sqlcon.Open();
                SqlCommand sqlCmd = new SqlCommand(query, sqlcon);
                SqlDataReader rdr = sqlCmd.ExecuteReader();

                while (rdr.Read())
                {
                    var userModel = new Player();
                    userModel.PlayerId = Convert.ToInt32(rdr["UserId"]);
                    userModel.UserName = rdr["UserName"].ToString();
                    userModel.Email = rdr["Email"].ToString();
                    userModel.Password = rdr["Password"].ToString();
                    userModel.UserRole = rdr["UserRole"].ToString();
                    userModel.UserToken = rdr["UserToken"].ToString();
                    userList.Add(userModel);
                }
                sqlcon.Close();
            }
            return userList;
        }

        public bool IsLoggedIn(HttpContext context)
        {
            var user = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (user == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
