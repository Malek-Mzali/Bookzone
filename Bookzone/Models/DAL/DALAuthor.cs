using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bookzone.Extensions;
using Bookzone.Models.Entity.Author;
using Ebook.Models;
using static System.String;


namespace Bookzone.Models.DAL
{
    public static class DalAuthor
    {
        public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Author') " +
                                    "CREATE TABLE [dbo].[Author] ( [Id] BIGINT IDENTITY(1, 1) NOT NULL, [Name] nvarchar(max)  NOT NULL, [Biography] nvarchar(max)  NULL, [Photo] nvarchar(max) DEFAULT('default.png') NOT NULL,CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                
                using (var command = new SqlCommand(sql, cnn))
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

                 
        public static IEnumerable<Author> GetAllAuthor()
        {
            var lstAuthor = new List<Author>();

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Author";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Author()
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    Name = dataReader["Name"].ToString(),
                                    Biography = dataReader["Biography"].ToString(),
                                    Photo = dataReader["Photo"].ToString(),
                                };
                                lstAuthor.Add(u);
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

            return lstAuthor;
        }

        public static JsonResponse NewAuthor(Author author)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var exist = GetAuthorBy("Name", author.Name);
                if (exist.Id >0)
                {
                    message.Extra = exist.Id.ToString();
                    message.Success = true;
                }
                else
                {
                    using (var connection = DbConnection.GetConnection())
                    {
                        connection.Open();

                        const string sql =
                            "Insert into Author(Name, Biography) values (@Name, @Biography);SELECT SCOPE_IDENTITY();";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                        
                
                            if (!IsNullOrEmpty(author.Name))
                            {
                                command.Parameters.AddWithValue("@Name", author.Name);
                            }
                        
                        
                            if (!IsNullOrEmpty(author.Biography))
                                command.Parameters.AddWithValue("@Biography",
                                    author.Biography);
                            else
                                command.Parameters.AddWithValue("@Biography", DBNull.Value);



                            var id = Convert.ToInt32(command.ExecuteScalar());

                            if (id>0)
                            {
                                message.Extra = id.ToString();
                                message.Success = true;
                            }

                        
                            connection.Close();
                        }
                    }
                }

            }

            
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }


            return message;
        }

        public static JsonResponse DeleteAuthorBy(string field, string value)
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
                    var sql = $"delete from Author where [{field}]=@Field";

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


                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse UpdateAuthor(Author author)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Author SET Name=@Name, Biography=@Biography WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", author.Id);
                        
                        if (!IsNullOrEmpty(author.Name))
                        {
                            command.Parameters.AddWithValue("@Name", author.Name);
                        }
                        
                        
                        if (!IsNullOrEmpty(author.Biography))
                            command.Parameters.AddWithValue("@Biography",
                                author.Biography);
                        else
                            command.Parameters.AddWithValue("@Biography", DBNull.Value);
                        

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }


            return message;
        }


        public static Author GetAuthorBy(string field, string value)
        {
            var author = new Author();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Author where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                author.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                author.Name = dataReader["Name"].ToString();
                                author.Biography = dataReader["Biography"].ToString();
                                author.Photo = dataReader["Photo"].ToString();
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

            return author;
            
        }
        
        public static JsonResponse UpdateCollectionByField(string fieldname, string value, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };

            var tablename = "Author";
             

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

    }
}