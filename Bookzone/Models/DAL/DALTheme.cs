using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.Entity.Theme;
using Bookzone.Models.Entity.User;
using Ebook.Models;
using static System.String;


namespace Bookzone.Models.DAL
{
    public static class DalTheme
    {
        public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Theme') " +
                                    "CREATE TABLE [dbo].[Theme] ( [Id] BIGINT IDENTITY(1, 1) NOT NULL, [Title] nvarchar(max) NOT NULL, [ShortTitle] nvarchar(max)  NULL, [Description] nvarchar(max)  NULL, [Icon] nvarchar(max) default('icon-sciences-hu') NOT NULL, CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                const string sql2 = "If not exists (select * from sysobjects where name = 'DocumentTheme') " +
                                   "CREATE TABLE [dbo].[DocumentTheme] ( [Id] BIGINT IDENTITY(1, 1) NOT NULL, [IdDocument] BIGINT NOT NULL, [IdTheme] BIGINT NOT NULL, CONSTRAINT [PK_DocumentTheme] PRIMARY KEY CLUSTERED ([Id] ASC) );";

                using (var command = new SqlCommand(sql, cnn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(sql2, cnn))
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

                 
        public static IEnumerable<Theme> GetAllThemes()
        {
            var lstTheme = new List<Theme>();

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Theme";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Theme()
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    Title = dataReader["Title"].ToString() ?? Empty,
                                    ShortTitle = dataReader["ShortTitle"].ToString(),
                                    Description = dataReader["Description"].ToString(),
                                    Icon = dataReader["Icon"].ToString(),

                                };
                                lstTheme.Add(u);
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

            return lstTheme;
        }

        public static JsonResponse NewTheme(Theme theme)
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
                        "Insert into Theme(Title,ShortTitle, Description, Icon) values (@Title, @ShortTitle, @Description, @Icon);SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (!IsNullOrEmpty(theme.Title))
                        {
                            command.Parameters.AddWithValue("@Title", theme.Title);
                        }
                        
                        if (!IsNullOrEmpty(theme.ShortTitle))
                            command.Parameters.AddWithValue("@ShortTitle",
                                theme.ShortTitle);
                        else
                            command.Parameters.AddWithValue("@ShortTitle", DBNull.Value);

                        
                        if (!IsNullOrEmpty(theme.Description))
                            command.Parameters.AddWithValue("@Description",
                                theme.Description);
                        else
                            command.Parameters.AddWithValue("@Description", DBNull.Value);
                        
                        if (!IsNullOrEmpty(theme.Icon))
                            command.Parameters.AddWithValue("@Icon",
                                theme.Icon);
                        else
                            command.Parameters.AddWithValue("@Icon", "icon-sciences-hu");

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

            
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }


            return message;
        }

        public static JsonResponse DeleteThemeBy(string field, string value)
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
                    var sql = $"delete from Theme where [{field}]=@Field";

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

        public static JsonResponse UpdateTheme(Theme theme)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var oldInfo = GetThemeBy("id", theme.Id.ToString());

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE THeme SET Title = @Title, ShortTitle = @ShortTitle, Description = @Description, Icon=@Icon WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", theme.Id);

                        if (!IsNullOrEmpty(theme.Title))
                            command.Parameters.AddWithValue("@Title", theme.Title);

                        if (!IsNullOrEmpty(theme.ShortTitle))
                            command.Parameters.AddWithValue("@ShortTitle",
                                theme.ShortTitle);
                        else
                            command.Parameters.AddWithValue("@ShortTitle", DBNull.Value);

                        
                        if (!IsNullOrEmpty(theme.Description))
                            command.Parameters.AddWithValue("@Description",
                                theme.Description);
                        else
                            command.Parameters.AddWithValue("@Description", DBNull.Value);
                        
                        if (!IsNullOrEmpty(theme.Icon))
                            command.Parameters.AddWithValue("@Icon",
                                theme.Icon);
                        else
                            command.Parameters.AddWithValue("@Icon", DBNull.Value);
                        
                        
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


        public static Theme GetThemeBy(string field, string value)
        {
            var theme = new Theme();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Theme where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
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


        public static JsonResponse UpdateThemeByField(string fieldname, string value, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };

            var tablename = "Theme";
             

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

        public static IEnumerable<Editor> GetAllEditorByTheme(string id)
        {
            var editorLst = new List<Editor>();
            var op =  BllCollection.GetAllCollectionByTheme(id);
            foreach (var editor in op)
            {
                var result = BllAccount.GetEditorBy("id", editor.IdEditor.ToString());

                if (!editorLst.Any(p => p.Id == result.Id))
                {
                    editorLst.Add(result);
                }
            }

            return editorLst;
        }
    }
}