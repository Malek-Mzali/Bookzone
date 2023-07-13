using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Bookzone.Extensions;
using Bookzone.Models.Entity.User;
using Ebook.Models;
using NeoSmart.Utils;
using static System.String;

namespace Bookzone.Models.DAL
{
    public static class DalAccount
    {
        
        private static bool CheckFieldUnicity(string field, string value)
        {
            {
                var nbOccurs = 0;
                try
                {
                    using (var cnn = DbConnection.GetConnection())
                    {
                        var strSql = "select * from Users where [" + field + "]=@value";
                        var cmd = new SqlCommand(strSql, cnn);
                        cmd.Parameters.AddWithValue("@value", value);
                        var da = new SqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        nbOccurs = dt.Rows.Count;
                    }
                }
                catch
                {
                    // ignored
                }

                return nbOccurs > 0;
            }
        }

        public static bool CheckFieldUnicityIndv(string field, string value)
        {
            {
                var nbOccurs = 0;
                try
                {
                    using (var cnn = DbConnection.GetConnection())
                    {
                        var strSql = "select * from Individual where [" + field + "]=@value";
                        var cmd = new SqlCommand(strSql, cnn);
                        cmd.Parameters.AddWithValue("@value", value);
                        var da = new SqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        nbOccurs = dt.Rows.Count;
                    }
                }
                catch
                {
                    // ignored
                }

                return nbOccurs > 0;
            }
        }

        private static string EncryptPassword(string str)
        {
            var encrptKey = "2022;[xxxxxx)qsdmOwcxcOLouka";
            byte[] iv = {18, 52, 86, 120, 144, 171, 205, 239};
            var byKey = Encoding.UTF8.GetBytes(encrptKey[..8]);
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.UTF8.GetBytes(str);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(byKey, iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        private static string DecryptPassword(string str)
        {
            str = str.Replace(" ", "+");
            const string decryptKey = "2022;[xxxxxx)qsdmOwcxcOLouka";
            byte[] byKey;
            byte[] iv = {18, 52, 86, 120, 144, 171, 205, 239};

            byKey = Encoding.UTF8.GetBytes(decryptKey[..8]);
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(byKey, iv), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        private static bool IsEncrypted(string str)
        {
            try
            {
                if (!IsNullOrEmpty(DecryptPassword(str)))
                    return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        private static string GenerateNewRandom()
        {
            generate:
            var generator = new Random();
            var r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1) r = GenerateNewRandom();

            if (CheckFieldUnicity("code", r)) goto generate;
            return r;
        }

        private static void SendActivationEmail(string email, string code)
        {

            var emailAuth = new EmailAuth(code);
            var to = new MailAddress(email);
            var from = new MailAddress(emailAuth.From);
            var message = new MailMessage(from, to);
            message.Subject = emailAuth.Subject;
            message.Body = emailAuth.Body;


            var htmlView = AlternateView.CreateAlternateViewFromString(
                emailAuth.Body, null, "text/html"
            );
            message.AlternateViews.Add(htmlView);
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(emailAuth.Address, emailAuth.Password);
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }            
            }

        }
        

        
        private static void SendResetEmail(string email, string code)
        {
            var emailAuth = new EmailAuth($"https://localhost:44349/Account/ResetPassword?code={code}");

            var to = new MailAddress(email);
            var from = new MailAddress(emailAuth.From);
            var message = new MailMessage(from, to);
            emailAuth.Subject = "Reset Password";
            message.Subject = emailAuth.Subject;
            message.Body = emailAuth.BodyReset;


            var htmlView = AlternateView.CreateAlternateViewFromString(
                emailAuth.BodyReset, null, "text/html"
            );
            message.AlternateViews.Add(htmlView);
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(emailAuth.Address, emailAuth.Password);
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }            
            }
            


        }


        public static Users GetUserBy(string field, string value)
        {
            var u = new Users();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "select * from Users where [" + field + "]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                u.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                u.Code = int.Parse(dataReader["Code"].ToString() ?? Empty);
                                u.Email = dataReader["Email"].ToString();
                                u.EmailConfirmed = (bool) dataReader["EmailConfirmed"];
                                u.EmailConfirmationCode = dataReader["EmailConfirmationCode"].ToString() ?? Empty;
                                if (!IsNullOrEmpty(dataReader["CodeExpirationDate"].ToString()))
                                    u.CodeExpirationDate =
                                        DateTime.Parse(dataReader["CodeExpirationDate"].ToString() ?? Empty);
                                u.Password = DecryptPassword(dataReader["Password"].ToString());
                                u.Phone = dataReader["Phone"].ToString();
                                u.Country = dataReader["Country"].ToString();
                                u.Address = dataReader["Address"].ToString();
                                u.PostalCode = dataReader["PostalCode"].ToString();
                                u.Role = dataReader["Role"].ToString();
                                u.Photo = dataReader["Photo"].ToString();
                                u.Date = DateTime.Parse(dataReader["Date"].ToString() ?? string.Empty);

                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return u;
        }

        public static Individual GetIndvBy(string field, string value)
        {
            var indv = new Individual();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "select * from Individual where [" + field + "]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                indv.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                indv.Firstname = dataReader["Firstname"].ToString() ?? Empty;
                                indv.Lastname = dataReader["Lastname"].ToString() ?? Empty;
                                indv.Gender = dataReader["Gender"].ToString() ?? Empty;
                                indv.DateofBirth = dataReader["DateofBirth"].ToString() ?? Empty;
                                indv.Profession = dataReader["Profession"].ToString() ?? Empty;
                                indv.Organization = dataReader["Organization"].ToString() ?? Empty;
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return indv;
        }

        public static Administrator GetAdministratorBy(string field, string value)
        {
            var admin = new Administrator();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "select * from Administrator where [" + field + "]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                admin.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                admin.Website = dataReader["Website"].ToString() ?? Empty;
                                admin.About = dataReader["About"].ToString() ?? Empty;
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return admin;
        }

        public static Organization GetOrganizationBy(string field, string value)
        {
            var organization = new Organization();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "select * from Organization where [" + field + "]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                organization.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                organization.Name = dataReader["Name"].ToString() ?? Empty;
                                organization.Type = dataReader["Type"].ToString() ?? Empty;
                                organization.Website = dataReader["Website"].ToString() ?? Empty;
                                organization.ShortName = dataReader["ShortName"].ToString() ?? Empty;
                                organization.IpAdress = dataReader["IpAdress"].ToString()?.Split(";").ToList();
                                organization.About = dataReader["About"].ToString();
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return organization;
        }

        public static Editor GetEditorBy(string field, string value)
        {
            var editor = new Editor();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "select * from Editor where [" + field + "]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                editor.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                editor.Name = dataReader["Name"].ToString() ?? Empty;
                                editor.City = dataReader["City"].ToString() ?? Empty;
                                editor.Website = dataReader["Website"].ToString() ?? Empty;
                                editor.Multiplyer = Convert.ToInt32(dataReader["Multiplyer"].ToString() ?? Empty);
                                editor.About = dataReader["About"].ToString() ?? Empty;
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return editor;
        }
        
        public static IEnumerable<UserInfo> GetAllUsersGroupedBy(string role)
        {
            var lstUsers = new List<Users>();

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Users Where Users.Role= @role";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@role", role);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Users
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    Code = int.Parse(dataReader["Code"].ToString() ?? Empty),
                                    Email = dataReader["Email"].ToString(),
                                    EmailConfirmed = (bool) dataReader["EmailConfirmed"],
                                    EmailConfirmationCode = dataReader["EmailConfirmationCode"].ToString(),
                                    Password = dataReader["Password"].ToString(),
                                    Phone = dataReader["Phone"].ToString(),
                                    Country = dataReader["Country"].ToString(),
                                    Address = dataReader["Address"].ToString(),
                                    PostalCode = dataReader["PostalCode"].ToString(),
                                    Role = dataReader["Role"].ToString(),
                                    Photo = dataReader["Photo"].ToString(),
                                    Date = DateTime.Parse(dataReader["Date"].ToString() ?? Empty)
                                };
                                lstUsers.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            var lstUsersInfo = new List<UserInfo>();

            foreach (var user in lstUsers)
                switch (role)
                {
                    case "Individual":
                        lstUsersInfo.Add(new UserInfo
                        {
                            UsersGroup = user,
                            IndividualGroup = GetIndvBy("Id", user.Id.ToString())
                        });
                        break;
                    case "Administrator":
                        lstUsersInfo.Add(new UserInfo
                        {
                            AdministratorGroup = GetAdministratorBy("Id", user.Id.ToString()),
                            UsersGroup = user
                        });
                        break;
                    case "Organization":
                        lstUsersInfo.Add(new UserInfo
                        {
                            OrganizationGroup = GetOrganizationBy("Id", user.Id.ToString()),
                            UsersGroup = user
                        });
                        break;
                    case "Editor":
                        lstUsersInfo.Add(new UserInfo
                        {
                            EditorGroup = GetEditorBy("Id", user.Id.ToString()),
                            UsersGroup = user
                        });
                        break;
                }

            return lstUsersInfo;
        }


        public static JsonResponse CheckUserLogIn(string email, string password)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Verify your infos"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    const string sql = "select COUNT(*) from Users where Email = @email and Password=@password";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", EncryptPassword(password));

                        var userCount = (int) command.ExecuteScalar();
                        if (userCount > 0)
                        {
                            message.Success = true;
                            message.Message = "Login ok";
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse AddUser(Users newU, bool dash)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur d'ajout"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    if (CheckFieldUnicity("Email", newU.Email) == false)
                    {
                        const string sql =
                            "Insert into Users(Code, Email,EmailConfirmed,EmailConfirmationCode, CodeExpirationDate, Password, Phone, Country, Address, PostalCode, Role, Photo)values(@code, @email, @emailConfirmed, @emailConfirmationCode, @CodeExpirationDate, @password, @phone, @country, @address, @postalCode, @Role, @photo);SELECT SCOPE_IDENTITY();";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;

                            command.Parameters.AddWithValue("@code", GenerateNewRandom());

                            if (IsNullOrEmpty(newU.Email))
                                command.Parameters.AddWithValue("@Email", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@Email", newU.Email);

                            var code = UrlBase64.Encode(
                                Encoding.UTF8.GetBytes(newU.Email + "-" + DateTime.Now.ToString("MM/dd/yyyy hh:mm")));
                            
                            if (dash)
                            {
                                command.Parameters.AddWithValue("@emailConfirmed", 1);
                                command.Parameters.AddWithValue("@EmailConfirmationCode", DBNull.Value);
                                command.Parameters.AddWithValue("@CodeExpirationDate", DBNull.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@emailConfirmed", 0);
                                command.Parameters.AddWithValue("@EmailConfirmationCode", code);
                                command.Parameters.AddWithValue("@CodeExpirationDate", DateTime.Now.AddMinutes(15));
                            }


                            if (IsNullOrEmpty(newU.Password))
                                command.Parameters.AddWithValue("@password", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@password", EncryptPassword(newU.Password));

                            if (IsNullOrEmpty(newU.Phone))
                                command.Parameters.AddWithValue("@phone", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@phone", newU.Phone);

                            if (IsNullOrEmpty(newU.Country))
                                command.Parameters.AddWithValue("@country", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@country", newU.Country);

                            if (IsNullOrEmpty(newU.Address))
                                command.Parameters.AddWithValue("@address", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@address", newU.Address);

                            if (IsNullOrEmpty(newU.PostalCode))
                                command.Parameters.AddWithValue("@postalCode", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@postalCode", newU.PostalCode);

                            if (!IsNullOrEmpty(newU.Role))
                                command.Parameters.AddWithValue("@Role", newU.Role);
                            else
                                command.Parameters.AddWithValue("@Role", "Individual");

                            if (!IsNullOrEmpty(newU.Photo))
                                command.Parameters.AddWithValue("@photo", newU.Photo);
                            else
                                command.Parameters.AddWithValue("@photo", "default.png");


                            var id = Convert.ToInt32(command.ExecuteScalar());

                            if (!dash)
                            {
                                SendActivationEmail(newU.Email, code);
                                message.Message = "Verify your email";
                            }
                            else
                            {
                                message.Extra = id.ToString();
                            }

                            message.Success = true;
                        }
                    }
                    else
                    {
                        message.Success = false;
                        message.Message = "Email exist";
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }


        public static JsonResponse DeleteUserBy(string field, string value, string role)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "delete from Users where [" + field + "]=@Field";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    var sql2 = "delete from " + role + " where [" + field + "]=@Field";

                    using (var command = new SqlCommand(sql2, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }
                    var sql3 = "delete  from  DocumentComments where [IdUser]=@Field";

                    using (var command = new SqlCommand(sql3, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    var sql4 = "delete  from  WishListUser where [UserId]=@Field";

                    using (var command = new SqlCommand(sql4, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }


        public static JsonResponse UpdateUser(UserInfo user)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var oldInfo = GetUserBy("id", user.UsersGroup.Id.ToString());

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET Email = @Email, Password = @Password, Country = @Country, Address= @Address, PostalCode = @PostalCode, Role=@Role WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                        if (!IsNullOrEmpty(user.UsersGroup.Email))
                            command.Parameters.AddWithValue("@Email", user.UsersGroup.Email);


                        if (!IsNullOrEmpty(user.UsersGroup.Password))
                            command.Parameters.AddWithValue("@Password",
                                IsEncrypted(user.UsersGroup.Password)
                                    ? user.UsersGroup.Password
                                    : EncryptPassword(user.UsersGroup.Password));

                        if (!IsNullOrEmpty(user.UsersGroup.Country))
                            command.Parameters.AddWithValue("@Country", user.UsersGroup.Country);

                        if (!IsNullOrEmpty(user.UsersGroup.Address))
                            command.Parameters.AddWithValue("@Address", user.UsersGroup.Address);

                        if (!IsNullOrEmpty(user.UsersGroup.PostalCode))
                            command.Parameters.AddWithValue("@PostalCode", user.UsersGroup.PostalCode);

                        if (!IsNullOrEmpty(user.UsersGroup.Role))
                            command.Parameters.AddWithValue("@Role", user.UsersGroup.Role);
                        command.ExecuteNonQuery();
                    }

                    if (oldInfo.Role != user.UsersGroup.Role)
                    {
                        var del = "delete from " + oldInfo.Role + " where id =@Field";

                        using (var command = new SqlCommand(del, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@Field", user.UsersGroup.Id);
                            command.ExecuteNonQuery();
                        }

                        switch (user.UsersGroup.Role)
                        {
                            case "Individual":
                                const string x1 =
                                    "Insert into Individual(Id, FirstName,LastName,Gender, DateofBirth, Profession, Organization) values(@Id, @FirstName, @LastName, @Gender, @DateofBirth, @Profession, @Organization)";

                                using (var command = new SqlCommand(x1, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Firstname))
                                        command.Parameters.AddWithValue("@Firstname", user.IndividualGroup.Firstname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Lastname))
                                        command.Parameters.AddWithValue("@Lastname", user.IndividualGroup.Lastname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Gender))
                                        command.Parameters.AddWithValue("@Gender", user.IndividualGroup.Gender);

                                    if (!IsNullOrEmpty(user.IndividualGroup.DateofBirth))
                                        command.Parameters.AddWithValue("@DateofBirth",
                                            user.IndividualGroup.DateofBirth);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Profession))
                                        command.Parameters.AddWithValue("@Profession", user.IndividualGroup.Profession);
                                    else
                                        command.Parameters.AddWithValue("@Profession", DBNull.Value);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Organization))
                                        command.Parameters.AddWithValue("@Organization",
                                            user.IndividualGroup.Organization);
                                    else
                                        command.Parameters.AddWithValue("@Organization", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Editor":
                                const string x2 =
                                    "Insert into Editor(Id, Name,City,Website, About, Multiplyer) values(@Id, @Name, @City, @Website, @About, @Multiplyer)";
                                using (var command = new SqlCommand(x2, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.EditorGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.EditorGroup.Name);

                                    if (!IsNullOrEmpty(user.EditorGroup.City))
                                        command.Parameters.AddWithValue("@City", user.EditorGroup.City);

                                    if (!IsNullOrEmpty(user.EditorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.EditorGroup.Website);

                                    if (!IsNullOrEmpty(user.EditorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.EditorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.EditorGroup.Multiplyer.ToString()))
                                        command.Parameters.AddWithValue("@Multiplyer", user.EditorGroup.Multiplyer);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Organization":
                                const string x3 =
                                    "Insert into Organization(Id, Name,Shortname,Website, About, Type, IpAdress) values(@Id, @Name, @Shortname, @Website, @About, @Type, @IpAdress)";
                                using (var command = new SqlCommand(x3, connection))
                                {
                                    command.CommandType = CommandType.Text;
                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.OrganizationGroup.Name);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.ShortName))
                                        command.Parameters.AddWithValue("@ShortName", user.OrganizationGroup.ShortName);
                                    else
                                        command.Parameters.AddWithValue("@ShortName", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.OrganizationGroup.Website);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.About))
                                        command.Parameters.AddWithValue("@About", user.OrganizationGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Type))
                                        command.Parameters.AddWithValue("@Type", user.OrganizationGroup.Type);

                                    if (user.OrganizationGroup.IpAdress.Any())
                                        command.Parameters.AddWithValue("@IpAdress",
                                            Join(",", user.OrganizationGroup.IpAdress).Replace("\n", ";"));

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Administrator":
                                const string x4 =
                                    "Insert into Administrator(Id, Website,About) values(@Id, @Website, @About)";
                                using (var command = new SqlCommand(x4, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.AdministratorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.AdministratorGroup.Website);


                                    if (!IsNullOrEmpty(user.AdministratorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.AdministratorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                        }
                    }
                    else
                    {
                        switch (user.UsersGroup.Role)
                        {
                            case "Individual":
                                const string sql2 =
                                    "UPDATE Individual SET Firstname = @Firstname, Lastname = @Lastname, Gender= @Gender, DateofBirth = @DateofBirth, Profession= @Profession, Organization = @Organization WHERE Id = @Id";
                                using (var command = new SqlCommand(sql2, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Firstname))
                                        command.Parameters.AddWithValue("@Firstname", user.IndividualGroup.Firstname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Lastname))
                                        command.Parameters.AddWithValue("@Lastname", user.IndividualGroup.Lastname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Gender))
                                        command.Parameters.AddWithValue("@Gender", user.IndividualGroup.Gender);

                                    if (!IsNullOrEmpty(user.IndividualGroup.DateofBirth))
                                        command.Parameters.AddWithValue("@DateofBirth",
                                            user.IndividualGroup.DateofBirth);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Profession))
                                        command.Parameters.AddWithValue("@Profession", user.IndividualGroup.Profession);
                                    else
                                        command.Parameters.AddWithValue("@Profession", DBNull.Value);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Organization))
                                        command.Parameters.AddWithValue("@Organization",
                                            user.IndividualGroup.Organization);
                                    else
                                        command.Parameters.AddWithValue("@Organization", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Editor":
                                const string sql3 =
                                    "UPDATE Editor SET Name = @Name, City = @City, Website= @Website, About = @About, Multiplyer = @Multiplyer WHERE Id = @Id";
                                using (var command = new SqlCommand(sql3, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.EditorGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.EditorGroup.Name);

                                    if (!IsNullOrEmpty(user.EditorGroup.City))
                                        command.Parameters.AddWithValue("@City", user.EditorGroup.City);

                                    if (!IsNullOrEmpty(user.EditorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.EditorGroup.Website);

                                    if (!IsNullOrEmpty(user.EditorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.EditorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.EditorGroup.Multiplyer.ToString()))
                                        command.Parameters.AddWithValue("@Multiplyer", user.EditorGroup.Multiplyer);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Organization":
                                const string sql4 =
                                    "UPDATE Organization SET Name = @Name, Shortname = @Shortname, Website = @Website, About= @About, Type = @Type, IpAdress = @IpAdress WHERE Id = @Id";
                                using (var command = new SqlCommand(sql4, connection))
                                {
                                    command.CommandType = CommandType.Text;
                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.OrganizationGroup.Name);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.ShortName))
                                        command.Parameters.AddWithValue("@ShortName", user.OrganizationGroup.ShortName);
                                    else
                                        command.Parameters.AddWithValue("@ShortName", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.OrganizationGroup.Website);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.About))
                                        command.Parameters.AddWithValue("@About", user.OrganizationGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Type))
                                        command.Parameters.AddWithValue("@Type", user.OrganizationGroup.Type);

                                    if (user.OrganizationGroup.IpAdress.Any())
                                        command.Parameters.AddWithValue("@IpAdress",
                                            Join(",", user.OrganizationGroup.IpAdress).Replace("\n", ";"));
                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Administrator":
                                const string sql5 =
                                    "UPDATE Administrator SET Website = @Website, About = @About WHERE Id = @Id";
                                using (var command = new SqlCommand(sql5, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.AdministratorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.AdministratorGroup.Website);


                                    if (!IsNullOrEmpty(user.AdministratorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.AdministratorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);
                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                        }
                    }


                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }
        public static JsonResponse UpdateUserProfile(UserInfo user)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var oldInfo = GetUserBy("id", user.UsersGroup.Id.ToString());

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET Email = @Email, Password = @Password, Country = @Country, Address= @Address, PostalCode = @PostalCode WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                        if (!IsNullOrEmpty(user.UsersGroup.Email))
                            command.Parameters.AddWithValue("@Email", user.UsersGroup.Email);


                        if (!IsNullOrEmpty(user.UsersGroup.Password))
                            command.Parameters.AddWithValue("@Password",
                                IsEncrypted(user.UsersGroup.Password)
                                    ? user.UsersGroup.Password
                                    : EncryptPassword(user.UsersGroup.Password));

                        if (!IsNullOrEmpty(user.UsersGroup.Country))
                            command.Parameters.AddWithValue("@Country", user.UsersGroup.Country);

                        if (!IsNullOrEmpty(user.UsersGroup.Address))
                            command.Parameters.AddWithValue("@Address", user.UsersGroup.Address);

                        if (!IsNullOrEmpty(user.UsersGroup.PostalCode))
                            command.Parameters.AddWithValue("@PostalCode", user.UsersGroup.PostalCode);


                        command.ExecuteNonQuery();
                    }


                    {
                        switch (user.UsersGroup.Role)
                        {
                            case "Individual":
                                const string sql2 =
                                    "UPDATE Individual SET Firstname = @Firstname, Lastname = @Lastname, Gender= @Gender, DateofBirth = @DateofBirth, Profession= @Profession, Organization = @Organization WHERE Id = @Id";
                                using (var command = new SqlCommand(sql2, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Firstname))
                                        command.Parameters.AddWithValue("@Firstname", user.IndividualGroup.Firstname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Lastname))
                                        command.Parameters.AddWithValue("@Lastname", user.IndividualGroup.Lastname);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Gender))
                                        command.Parameters.AddWithValue("@Gender", user.IndividualGroup.Gender);

                                    if (!IsNullOrEmpty(user.IndividualGroup.DateofBirth))
                                        command.Parameters.AddWithValue("@DateofBirth",
                                            user.IndividualGroup.DateofBirth);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Profession))
                                        command.Parameters.AddWithValue("@Profession", user.IndividualGroup.Profession);
                                    else
                                        command.Parameters.AddWithValue("@Profession", DBNull.Value);

                                    if (!IsNullOrEmpty(user.IndividualGroup.Organization))
                                        command.Parameters.AddWithValue("@Organization",
                                            user.IndividualGroup.Organization);
                                    else
                                        command.Parameters.AddWithValue("@Organization", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Editor":
                                const string sql3 =
                                    "UPDATE Editor SET Name = @Name, City = @City, Website= @Website, About = @About WHERE Id = @Id";
                                using (var command = new SqlCommand(sql3, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.EditorGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.EditorGroup.Name);

                                    if (!IsNullOrEmpty(user.EditorGroup.City))
                                        command.Parameters.AddWithValue("@City", user.EditorGroup.City);

                                    if (!IsNullOrEmpty(user.EditorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.EditorGroup.Website);

                                    if (!IsNullOrEmpty(user.EditorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.EditorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.EditorGroup.Multiplyer.ToString()))
                                        command.Parameters.AddWithValue("@Multiplyer", user.EditorGroup.Multiplyer);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Organization":
                                const string sql4 =
                                    "UPDATE Organization SET Name = @Name, Shortname = @Shortname, Website = @Website, About= @About WHERE Id = @Id";
                                using (var command = new SqlCommand(sql4, connection))
                                {
                                    command.CommandType = CommandType.Text;
                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Name))
                                        command.Parameters.AddWithValue("@Name", user.OrganizationGroup.Name);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.ShortName))
                                        command.Parameters.AddWithValue("@ShortName", user.OrganizationGroup.ShortName);
                                    else
                                        command.Parameters.AddWithValue("@ShortName", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.OrganizationGroup.Website);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.About))
                                        command.Parameters.AddWithValue("@About", user.OrganizationGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);

                                    if (!IsNullOrEmpty(user.OrganizationGroup.Type))
                                        command.Parameters.AddWithValue("@Type", user.OrganizationGroup.Type);

                                    if (user.OrganizationGroup.IpAdress.Any())
                                        command.Parameters.AddWithValue("@IpAdress",
                                            Join(",", user.OrganizationGroup.IpAdress).Replace("\n", ";"));
                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Administrator":
                                const string sql5 =
                                    "UPDATE Administrator SET Website = @Website, About = @About WHERE Id = @Id";
                                using (var command = new SqlCommand(sql5, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                    if (!IsNullOrEmpty(user.AdministratorGroup.Website))
                                        command.Parameters.AddWithValue("@Website", user.AdministratorGroup.Website);


                                    if (!IsNullOrEmpty(user.AdministratorGroup.About))
                                        command.Parameters.AddWithValue("@About", user.AdministratorGroup.About);
                                    else
                                        command.Parameters.AddWithValue("@About", DBNull.Value);
                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                        }
                    }


                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse ActivateUser(string code)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            var user = GetUserBy("EmailConfirmationCode", code);
            if (IsNullOrEmpty(user.Email)) return null;

            if (DateTime.Compare(DateTime.Now, user.CodeExpirationDate) < 0)
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET EmailConfirmed = @emailConfirmed, EmailConfirmationCode =@EmailConfirmationCode, CodeExpirationDate =@CodeExpirationDate WHERE Email = @Email and EmailConfirmed=0";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@EmailConfirmed", 1);
                        command.Parameters.AddWithValue("@EmailConfirmationCode", DBNull.Value);
                        command.Parameters.AddWithValue("@CodeExpirationDate", DBNull.Value);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Your account is activated";
                        }
                    }

                    connection.Close();
                }
            }
            else
            {
                var codeAgain =
                    UrlBase64.Encode(
                        Encoding.UTF8.GetBytes(user.Email + "-" + DateTime.Now.ToString("MM/dd/yyyy hh:mm")));
                
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET EmailConfirmationCode =@EmailConfirmationCode, CodeExpirationDate=@CodeExpirationDate WHERE Email = @Email and EmailConfirmed=0";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@EmailConfirmationCode", codeAgain);
                        command.Parameters.AddWithValue("@CodeExpirationDate", DateTime.Now.AddMinutes(15));

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Your confirmation code expired, a new one has been sent";
                            SendActivationEmail(user.Email, codeAgain);
                        }
                    }

                    connection.Close();
                }
            }

            return message;
        }

        public static JsonResponse UpdateUserProfile(Individual indvusr, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur d'ajout"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    const string sql =
                        "Insert into Individual(Id, FirstName,LastName,Gender, DateofBirth, Profession, Organization)values(@Id, @FirstName, @LastName, @Gender, @DateofBirth, @Profession, @Organization)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@Id", int.Parse(id));

                        if (IsNullOrEmpty(indvusr.Firstname))
                            command.Parameters.AddWithValue("@Firstname", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Firstname", indvusr.Firstname);
                        if (IsNullOrEmpty(indvusr.Lastname))
                            command.Parameters.AddWithValue("@Lastname", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Lastname", indvusr.Lastname);
                        if (IsNullOrEmpty(indvusr.Gender))
                            command.Parameters.AddWithValue("@Gender", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Gender", indvusr.Gender);
                        if (IsNullOrEmpty(indvusr.DateofBirth))
                            command.Parameters.AddWithValue("@DateofBirth", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@DateofBirth", indvusr.DateofBirth);
                        if (IsNullOrEmpty(indvusr.Profession))
                            command.Parameters.AddWithValue("@Profession", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Profession", indvusr.Profession);
                        if (IsNullOrEmpty(indvusr.Organization))
                            command.Parameters.AddWithValue("@Organization", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Organization", indvusr.Organization);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Successful";
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse NewUser(UserInfo user)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            var operation = AddUser(user.UsersGroup, true);
            if (!operation.Success) return operation;
            try
            {
                user.UsersGroup.Id = int.Parse(operation.Extra);
                message.Extra = operation.Extra;
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    switch (user.UsersGroup.Role)
                    {
                        case "Individual":
                            const string sql =
                                "Insert into Individual(Id, FirstName,LastName,Gender, DateofBirth, Profession, Organization) values(@Id, @FirstName, @LastName, @Gender, @DateofBirth, @Profession, @Organization)";

                            using (var command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                if (!IsNullOrEmpty(user.IndividualGroup.Firstname))
                                    command.Parameters.AddWithValue("@Firstname", user.IndividualGroup.Firstname);

                                if (!IsNullOrEmpty(user.IndividualGroup.Lastname))
                                    command.Parameters.AddWithValue("@Lastname", user.IndividualGroup.Lastname);

                                if (!IsNullOrEmpty(user.IndividualGroup.Gender))
                                    command.Parameters.AddWithValue("@Gender", user.IndividualGroup.Gender);

                                if (!IsNullOrEmpty(user.IndividualGroup.DateofBirth))
                                    command.Parameters.AddWithValue("@DateofBirth",
                                        user.IndividualGroup.DateofBirth);

                                if (!IsNullOrEmpty(user.IndividualGroup.Profession))
                                    command.Parameters.AddWithValue("@Profession", user.IndividualGroup.Profession);
                                else
                                    command.Parameters.AddWithValue("@Profession", DBNull.Value);

                                if (!IsNullOrEmpty(user.IndividualGroup.Organization))
                                    command.Parameters.AddWithValue("@Organization",
                                        user.IndividualGroup.Organization);
                                else
                                    command.Parameters.AddWithValue("@Organization", DBNull.Value);

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;

                        case "Editor":
                            const string sql2 =
                                "Insert into Editor(Id, Name,City,Website, About, Multiplyer) values(@Id, @Name, @City, @Website, @About, @Multiplyer)";
                            using (var command = new SqlCommand(sql2, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                if (!IsNullOrEmpty(user.EditorGroup.Name))
                                    command.Parameters.AddWithValue("@Name", user.EditorGroup.Name);

                                if (!IsNullOrEmpty(user.EditorGroup.City))
                                    command.Parameters.AddWithValue("@City", user.EditorGroup.City);

                                if (!IsNullOrEmpty(user.EditorGroup.Website))
                                    command.Parameters.AddWithValue("@Website", user.EditorGroup.Website);

                                if (!IsNullOrEmpty(user.EditorGroup.About))
                                    command.Parameters.AddWithValue("@About", user.EditorGroup.About);
                                else
                                    command.Parameters.AddWithValue("@About", DBNull.Value);

                                if (!IsNullOrEmpty(user.EditorGroup.Multiplyer.ToString()))
                                    command.Parameters.AddWithValue("@Multiplyer", user.EditorGroup.Multiplyer);

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;

                        case "Organization":


                            const string sql3 =
                                "Insert into Organization(Id, Name,Shortname,Website, About, Type, IpAdress) values(@Id, @Name, @Shortname, @Website, @About, @Type, @IpAdress)";
                            using (var command = new SqlCommand(sql3, connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                if (!IsNullOrEmpty(user.OrganizationGroup.Name))
                                    command.Parameters.AddWithValue("@Name", user.OrganizationGroup.Name);

                                if (!IsNullOrEmpty(user.OrganizationGroup.ShortName))
                                    command.Parameters.AddWithValue("@ShortName", user.OrganizationGroup.ShortName);
                                else
                                    command.Parameters.AddWithValue("@ShortName", DBNull.Value);

                                if (!IsNullOrEmpty(user.OrganizationGroup.Website))
                                    command.Parameters.AddWithValue("@Website", user.OrganizationGroup.Website);

                                if (!IsNullOrEmpty(user.OrganizationGroup.About))
                                    command.Parameters.AddWithValue("@About", user.OrganizationGroup.About);
                                else
                                    command.Parameters.AddWithValue("@About", DBNull.Value);

                                if (!IsNullOrEmpty(user.OrganizationGroup.Type))
                                    command.Parameters.AddWithValue("@Type", user.OrganizationGroup.Type);

                                if (user.OrganizationGroup.IpAdress.Any())
                                    command.Parameters.AddWithValue("@IpAdress",
                                        Join(",", user.OrganizationGroup.IpAdress).Replace("\n", ";"));

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;
                        case "Administrator":
                            const string sql4 =
                                "Insert into Administrator(Id, Website,About) values(@Id, @Website, @About)";
                            using (var command = new SqlCommand(sql4, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@Id", user.UsersGroup.Id);

                                if (!IsNullOrEmpty(user.AdministratorGroup.Website))
                                    command.Parameters.AddWithValue("@Website", user.AdministratorGroup.Website);


                                if (!IsNullOrEmpty(user.AdministratorGroup.About))
                                    command.Parameters.AddWithValue("@About", user.AdministratorGroup.About);
                                else
                                    command.Parameters.AddWithValue("@About", DBNull.Value);

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }

            if (!message.Success) DeleteUserBy("Email", user.UsersGroup.Email, user.UsersGroup.Role);

            return message;
        }

        public static JsonResponse PrepareUserForReset(Users user)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var code = UrlBase64.Encode(
                    Encoding.UTF8.GetBytes(user.Email + "-" + DateTime.Now.ToString("MM/dd/yyyy hh:mm")));
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET EmailConfirmationCode = @EmailConfirmationCode, CodeExpirationDate=@CodeExpirationDate where Id=@Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@EmailConfirmationCode", code);
                        command.Parameters.AddWithValue("@CodeExpirationDate", DateTime.Now.AddMinutes(15));
                        command.Parameters.AddWithValue("@Id", user.Id);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            SendResetEmail(user.Email, code);
                            message.Success = true;
                        }
                    }

                    connection.Close();
                }

            }
            catch (Exception e)
            {
                message.Message = e.Message;
            }
           

            return message;
        }

        public static JsonResponse ResetUserPasswordCheck(string code)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            var user = GetUserBy("EmailConfirmationCode", code);
            if (IsNullOrEmpty(user.Email)) return message;
            if (DateTime.Compare(DateTime.Now, user.CodeExpirationDate) < 0)
            {
                message.Success = true;
            }
            else
            {
                var codeAgain =
                    UrlBase64.Encode(
                        Encoding.UTF8.GetBytes(user.Email + "-" + DateTime.Now.ToString("MM/dd/yyyy hh:mm")));
                
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET EmailConfirmationCode =@EmailConfirmationCode, CodeExpirationDate=@CodeExpirationDate WHERE Id=@Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", user.Id);
                        command.Parameters.AddWithValue("@EmailConfirmationCode", codeAgain);
                        command.Parameters.AddWithValue("@CodeExpirationDate", DateTime.Now.AddMinutes(15));

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Message = "Your Reset code expired, a new one has been sent";
                            SendResetEmail(user.Email, codeAgain);
                        }
                    }

                    connection.Close();
                }
            }

            return message;
        }

        public static JsonResponse ResetUserPassword(string code, string NewPassword)
        {
            var message = new JsonResponse()
            {
                Success = false
            };
            var user = GetUserBy("EmailConfirmationCode", code);
            if (user == null)
            {
                return message;
            }
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Users SET EmailConfirmationCode = @EmailConfirmationCode, CodeExpirationDate=@CodeExpirationDate, Password=@Password where Id=@Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@EmailConfirmationCode", DBNull.Value);
                        command.Parameters.AddWithValue("@CodeExpirationDate", DBNull.Value);
                        command.Parameters.AddWithValue("@Id", user.Id);
                        command.Parameters.AddWithValue("@Password", EncryptPassword(NewPassword));
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                        }
                    }

                    connection.Close();
                }

            }
            catch (Exception e)
            {
                message.Message = e.Message;
            }

            return message;
        }
    }
}