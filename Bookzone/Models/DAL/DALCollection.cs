using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bookzone.Extensions;
using Bookzone.Models.Entity.Collection;
using Bookzone.Models.Entity.Theme;
using Ebook.Models;
using static System.String;


namespace Bookzone.Models.DAL
{
    public static class DalCollection
    {
        public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Collection') " +
                                    "CREATE TABLE [dbo].[Collection] ( [Id] BIGINT IDENTITY(1, 1) NOT NULL, [IdEditor] BIGINT NOT NULL, [IdTheme] BIGINT NOT NULL, [Title] nvarchar(max)  NOT NULL, [ShortTitle] nvarchar(max)  NULL, [Description] nvarchar(max)  NULL,CONSTRAINT [PK_Collection] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                
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

                 
        public static IEnumerable<Collection> GetAllCollection()
        {
            var lstCollection = new List<Collection>();

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Collection";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Collection()
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                    IdTheme = int.Parse(dataReader["IdTheme"].ToString() ?? Empty),
                                    Title = dataReader["Title"].ToString(),
                                    ShortTitle = dataReader["ShortTitle"].ToString(),
                                    Description = dataReader["Description"].ToString(),
                                };
                                lstCollection.Add(u);
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

            return lstCollection;
        }

        public static IEnumerable<Collection> GetCollectionForEditor(string value)
        {
            var lstCollection = new List<Collection>();

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Collection Where IdEditor =@IdEditor";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@IdEditor", value);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Collection()
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                    IdTheme = int.Parse(dataReader["IdTheme"].ToString() ?? Empty),
                                    Title = dataReader["Title"].ToString(),
                                    ShortTitle = dataReader["ShortTitle"].ToString(),
                                    Description = dataReader["Description"].ToString(),
                                };
                                lstCollection.Add(u);
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

            return lstCollection;
        }

        
        public static JsonResponse NewCollection(Collection collection)
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
                        "Insert into Collection(IdEditor, IdTheme, Title,ShortTitle, Description) values (@IdEditor, @IdTheme, @Title, @ShortTitle, @Description);SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        
                        if (!IsNullOrEmpty(collection.IdEditor.ToString()))
                        {
                            command.Parameters.AddWithValue("@IdEditor", collection.IdEditor);
                        }                       
                        if (!IsNullOrEmpty(collection.IdTheme.ToString()))
                        {
                            command.Parameters.AddWithValue("@IdTheme", collection.IdTheme);
                        }
                        
                        if (!IsNullOrEmpty(collection.Title))
                        {
                            command.Parameters.AddWithValue("@Title", collection.Title);
                        }
                        
                        if (!IsNullOrEmpty(collection.ShortTitle))
                            command.Parameters.AddWithValue("@ShortTitle",
                                collection.ShortTitle);
                        else
                            command.Parameters.AddWithValue("@ShortTitle", DBNull.Value);

                        
                        if (!IsNullOrEmpty(collection.Description))
                            command.Parameters.AddWithValue("@Description",
                                collection.Description);
                        else
                            command.Parameters.AddWithValue("@Description", DBNull.Value);
                        



                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                        }

                        
                        connection.Close();
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

        public static JsonResponse DeleteCollectionBy(string field, string value)
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
                    var sql = $"delete from Collection where [{field}]=@Field";

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

        public static JsonResponse UpdateCollection(Collection collection)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var oldInfo = GetCollectionBy("id", collection.Id.ToString());

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Collection SET IdEditor=@IdEditor, IdTheme=@IdTheme, Title = @Title, ShortTitle = @ShortTitle, Description = @Description WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", collection.Id);
                        
                        if (!IsNullOrEmpty(collection.IdEditor.ToString()))
                        {
                            command.Parameters.AddWithValue("@IdEditor", collection.IdEditor);
                        }                       
                        if (!IsNullOrEmpty(collection.IdTheme.ToString()))
                        {
                            command.Parameters.AddWithValue("@IdTheme", collection.IdTheme);
                        }
                        
                        if (!IsNullOrEmpty(collection.Title))
                        {
                            command.Parameters.AddWithValue("@Title", collection.Title);
                        }
                        
                        if (!IsNullOrEmpty(collection.ShortTitle))
                            command.Parameters.AddWithValue("@ShortTitle",
                                collection.ShortTitle);
                        else
                            command.Parameters.AddWithValue("@ShortTitle", DBNull.Value);

                        
                        if (!IsNullOrEmpty(collection.Description))
                            command.Parameters.AddWithValue("@Description",
                                collection.Description);
                        else
                            command.Parameters.AddWithValue("@Description", DBNull.Value);
                        

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                        
                    }

                    if (oldInfo.IdTheme != collection.IdTheme)
                    {
                        var newDocumentTheme =
                            DalDocument.GetAllDocumentGroupedBy("IdCollection",collection.Id.ToString());
                        foreach (var document in newDocumentTheme)
                        {
                            const string sql4 =
                                "Update DocumentTheme SET IdTheme=@IdTheme WHERE IdDocument=@IdDocument";
                            using (var command = new SqlCommand(sql4, connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@IdTheme", collection.IdTheme);
                                command.Parameters.AddWithValue("@IdDocument", document.DocumentGroup.Id);
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }
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


        public static Collection GetCollectionBy(string field, string value)
        {
            var collection = new Collection();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Collection where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                collection.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                collection.IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty);
                                collection.IdTheme = int.Parse(dataReader["IdTheme"].ToString() ?? Empty);
                                collection.Title = dataReader["Title"].ToString();
                                collection.ShortTitle = dataReader["ShortTitle"].ToString();
                                collection.Description = dataReader["Description"].ToString();
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

            return collection;
            
        }
        
        public static JsonResponse UpdateCollectionByField(string fieldname, string value, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };

            var tablename = "Collection";
             

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

        public static Theme GetThemeByCollection(string themeId)
        {
            var collection = GetCollectionBy("Id", themeId);
            var theme = new Theme();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"Select * From Theme WHERE Id = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", collection.IdTheme);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                theme.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                theme.Title = dataReader["Title"].ToString();
                                theme.ShortTitle = dataReader["ShortTitle"].ToString();
                                theme.Description = dataReader["Description"].ToString();
                                theme.Icon = dataReader["Icon"].ToString();
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

            return theme;
        }

        public static IEnumerable<Collection> GetAllCollectionByTheme(string themeId)
        {
            var collectionLst = new List<Collection>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"Select * From Collection WHERE IdTheme = @IdTheme";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@IdTheme", themeId);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var Id = dataReader["Id"].ToString() ?? Empty;
                                collectionLst.Add(GetCollectionBy("Id", Id));

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

            return collectionLst;
            
        }
    }
}