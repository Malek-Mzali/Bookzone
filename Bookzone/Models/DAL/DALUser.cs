using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.Entity.User;
using Ebook.Models;
using Ebook.Models.Entity.Document;
using static System.String;


namespace Bookzone.Models.DAL
{
    public static class DalUser
    { 
        public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Users') " +
                                   "CREATE TABLE [dbo].[Users] ([Id] bigint IDENTITY(1, 1) NOT NULL, [Code] bigint  NULL, [Email] nvarchar(max)  NOT NULL, [EmailConfirmed] bit  NOT NULL, [EmailConfirmationCode] nvarchar(max)  NULL,  [CodeExpirationDate] datetime  NULL, [Password] nvarchar(max)  NOT NULL, [Phone] nvarchar(max) NOT NULL, [Country] nvarchar(max) NOT NULL, [Address] nvarchar(max) NOT NULL, [PostalCode] nvarchar(max)  NOT NULL, [Role] nvarchar(max)  DEFAULT ('Individual') NOT NULL,  [Photo] nvarchar(max) DEFAULT 'default.png' NOT NULL , [Date] datetime DEFAULT GETDATE() NOT NULL , PRIMARY KEY CLUSTERED([Id] ASC));";
                const string sql2 = "If not exists (select * from sysobjects where name = 'Individual') " +
                                    "CREATE TABLE [dbo].[Individual] ([Id] bigint  NOT NULL, [FirstName] nvarchar(max)  NOT NULL, [LastName] nvarchar(max)  NOT NULL,[Gender] nvarchar(max) NOT NULL, [DateofBirth] nvarchar(max) NOT NULL, [Profession] nvarchar(max) NULL, [Organization] nvarchar(max) NULL);";
                const string sql3 = "If not exists (select * from sysobjects where name = 'Editor') " +
                                    "CREATE TABLE [dbo].[Editor] ( [Id] bigint NOT NULL, [Name] nvarchar(max) NOT NULL, [City] nvarchar(max) NOT NULL, [Website] nvarchar(max) NOT NULL, [About] varchar(max)  NULL, [Multiplyer] bigint DEFAULT ((1)) NOT NULL );";
                const string sql4 = "If not exists (select * from sysobjects where name = 'Organization') " +
                                    "CREATE TABLE [dbo].[Organization] ( [Id] bigint NOT NULL, [Name] nvarchar(max) NOT NULL, [ShortName] nvarchar(max)  NULL, [Website] nvarchar(max) NOT NULL, [About] varchar(max)  NULL, [Type] nvarchar(max) NOT NULL, [IpAdress] nvarchar(max) NOT NULL );";
                const string sql5 = "If not exists (select * from sysobjects where name = 'Administrator') " +
                                    "CREATE TABLE [dbo].[Administrator] ( [Id] bigint NOT NULL, [Website] nvarchar(max) NOT NULL, [About] varchar(max)  NULL );";
                const string sql6 = "If not exists (select * from sysobjects where name = 'PurchaseHistory') " +
                                    "CREATE TABLE [dbo].[PurchaseHistory] ( [Id] bigint IDENTITY(1, 1) NOT NULL, [UserId] bigint NOT NULL, [DocumentId] bigint NOT NULL, [DocumentType] nvarchar(max) NOT NULL , [IdEditor] bigint NOT NULL, [Date] datetime DEFAULT GETDATE() NOT NULL  );";
                const string sql7 = "If not exists (select * from sysobjects where name = 'WishListUser') " +
                                    "CREATE TABLE [dbo].[WishListUser] ([UserId] bigint NOT NULL, [DocumentId] bigint NOT NULL, [Id] bigint IDENTITY(1, 1) NOT NULL );";
               
                using (var command = new SqlCommand(sql, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql2, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql3, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql4, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql5, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql6, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql7, cnn))
                {
                    command.ExecuteNonQuery();
                }
                cnn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
                
        public static JsonResponse UpdateUsrProfileByField(string field, string value, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };

            var tablename = field.Split(".")[0].Replace("Group", "");
            var fieldname = field.Split(".")[1];


            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"UPDATE {tablename} SET {fieldname} = @value  WHERE Id = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", int.Parse(id));
                        command.Parameters.AddWithValue("@value", Uri.UnescapeDataString(value));
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
        public static JsonResponse AddPurchase(string docId, string userId)
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
                    const string sql = "Insert into PurchaseHistory(UserId,DocumentId,DocumentType, IdEditor) values (@UserId, @DocumentId, @DocumentType, @IdEditor)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@DocumentId", docId);
                        command.Parameters.AddWithValue("@DocumentType", BllDocument.GetDocumentById("id", docId).DocumentGroup.DocumentType);
                        command.Parameters.AddWithValue("@IdEditor", BllDocument.GetDocumentById("id", docId).DocumentGroup.IdEditor);
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
        public static JsonResponse ValidatePurchase(string docId, string userId)
        {
            var message = new JsonResponse
            {
                Success = false,
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "Select * From PurchaseHistory Where UserId=@UserId and DocumentId=@DocumentId";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@DocumentId", docId);
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
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

            return message;        }
        private static IEnumerable<int> GetAllPurchasedDocuments(string userId)
        {

            var resultId = new List<int>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "Select * From PurchaseHistory Where UserId=@UserId ORDER BY PurchaseHistory.[Date]  DESC ";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                resultId.Add(int.Parse(dataReader["DocumentId"].ToString() ?? string.Empty));
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

            return resultId;
            
        }
        public static IEnumerable<DocumentInfo> GetAllOwnedDocuments(string userid)
       {
           
           return GetAllPurchasedDocuments(userid).Select(document => BllDocument.GetDocumentById("Id", document.ToString())).ToList();
       } 
        public static Organization GetOrganizationByIp(string ip)
       {
           var organization = new Organization();
           try
           {
               using (var connection = DbConnection.GetConnection())
               {
                   connection.Open();
                   const string sql = "Select * From Organization ";
                   using (var command = new SqlCommand(sql, connection))
                   {
                       command.CommandType = CommandType.Text;
                       using (var dataReader = command.ExecuteReader())
                       {
                           while (dataReader.Read())
                           {
                               var ipOrg = (dataReader["IpAdress"].ToString() ?? Empty).Split(";");
                               foreach (var x in ipOrg)
                               {
                                   if (x == ip)
                                   {
                                       organization.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                       organization.Name = dataReader["Name"].ToString() ?? Empty;
                                       organization.Type = dataReader["Type"].ToString() ?? Empty;
                                       organization.Website = dataReader["Website"].ToString() ?? Empty;
                                       organization.ShortName = dataReader["ShortName"].ToString() ?? Empty;
                                       organization.IpAdress = dataReader["IpAdress"].ToString()?.Split(";").ToList();
                                       organization.About = dataReader["About"].ToString();
                                       break;
                                   };

                               }
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
        public static JsonResponse ValidateStateWishList(string docId, string userId)
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
                   const string sql = "Select * From WishListUser WHERE  UserId=@UserId and DocumentId=@DocumentId";
                   using (var command = new SqlCommand(sql, connection))
                   {
                       command.CommandType = CommandType.Text;
                       command.Parameters.AddWithValue("@UserId", userId);
                       command.Parameters.AddWithValue("@DocumentId", docId);

                       using (var dataReader = command.ExecuteReader())
                       {
                           if (dataReader.Read())
                           {
                               message.Success = true;
                           }

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
        public static IEnumerable<DocumentInfo> GetWishListByUser(string id)
        {
           var resultId = new List<DocumentInfo>();
           try
           {
               using (var connection = DbConnection.GetConnection())
               {
                   connection.Open();
                   const string sql = "Select * From WishListUser Where UserId=@UserId";
                   using (var command = new SqlCommand(sql, connection))
                   {
                       command.CommandType = CommandType.Text;
                       command.Parameters.AddWithValue("@UserId", id);
                       using (var dataReader = command.ExecuteReader())
                       {
                           while (dataReader.Read())
                           {
                               resultId.Add(BllDocument.GetDocumentById("id", dataReader["DocumentId"].ToString()));
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

           return resultId;       
        }
        public static JsonResponse AddToWishList(string docId, string userId)
        {
           var message = new JsonResponse
           {
               Success = false,
               Message = "Erreur"
           };

           if (!ValidateStateWishList(docId, userId).Success)
           {
               try
               {
                   using (var connection = DbConnection.GetConnection())
                   {
                       connection.Open();
                       const string sql = "Insert into WishListUser(UserId,DocumentId) values (@UserId, @DocumentId)";
                       using (var command = new SqlCommand(sql, connection))
                       {
                           command.CommandType = CommandType.Text;
                           command.Parameters.AddWithValue("@UserId", userId);
                           command.Parameters.AddWithValue("@DocumentId", docId);
                        
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
           }

           return message;
        }
        public static JsonResponse RemoveFromWishList(string docId, string userId)
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
                   var sql = "delete from WishListUser where UserId=@UserId and DocumentId=@DocumentId";

                   using (var command = new SqlCommand(sql, connection))
                   {
                       command.CommandType = CommandType.Text;
                       command.Parameters.AddWithValue("@UserId", userId);
                       command.Parameters.AddWithValue("@DocumentId", docId);
                       
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
    }
}