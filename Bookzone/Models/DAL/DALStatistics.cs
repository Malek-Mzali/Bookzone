#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bookzone.Models.BLL;
using Ebook.Models;
using Ebook.Models.Entity.Document;

namespace Bookzone.Models.DAL
{
    public class DALStatistics
    {
                public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Visitors') " +
                                    "CREATE TABLE [dbo].[Visitors] (  [Id] BIGINT IDENTITY(1, 1) NOT NULL, [VisitorId]  nvarchar(max)  NOT NULL, [Date] nvarchar(max) NOT NULL, [Agent] nvarchar(max) NOT NULL , CONSTRAINT [PK_Visitors] PRIMARY KEY CLUSTERED ([Id] ASC) );";

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

        public static int GetTotalUsers()
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT COUNT(*) FROM Users";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                         count = (int) command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;
            
        }

        public static int GetTodayUsers()
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT COUNT(*) FROM Users where FORMAT([Date], 'dd/MM/yyyy') = FORMAT(getdate(), 'dd/MM/yyyy')";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        count = (int) command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;
        }

        public static int GetTotalDocuments(string? identityName)
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT COUNT(*) FROM Document";
                    if (!string.IsNullOrEmpty(identityName))
                    {
                        sql = $"SELECT COUNT(*) FROM Document WHERE IdEditor = {identityName}";
                    }
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        count = (int) command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;
        }

        public static int GetTotalSales(string? identityName)
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT COUNT(*) FROM PurchaseHistory";
                    if (!string.IsNullOrEmpty(identityName))
                    {
                         sql = $"SELECT COUNT(*) FROM PurchaseHistory WHERE IdEditor = {identityName}";
                    }
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        count = (int) command.ExecuteScalar();
                        
                        
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;
        }
        public static int GetTodaySales(string? identityName)
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT COUNT(*) FROM PurchaseHistory where FORMAT([Date], 'dd/MM/yyyy') = FORMAT(getdate(), 'dd/MM/yyyy')";
                    if (!string.IsNullOrEmpty(identityName))
                    {
                        sql = $"SELECT COUNT(*) FROM PurchaseHistory where FORMAT([Date], 'dd/MM/yyyy') = FORMAT(getdate(), 'dd/MM/yyyy') and IdEditor = {identityName}";
                    }
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        count = (int) command.ExecuteScalar();
                        

                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;
        }

        public static List<Tuple<int, DocumentInfo>> TopSoldDocuments(string? identityName)
        {
            var list = new List<Tuple<int, DocumentInfo>>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT Top(5) DocumentId, COUNT(DocumentId) AS Count FROM PurchaseHistory GROUP BY DocumentId ORDER BY Count DESC";
                    if (!string.IsNullOrEmpty(identityName))
                    {
                         sql = $"SELECT Top(5) DocumentId, COUNT(DocumentId) AS Count FROM PurchaseHistory Where IdEditor = {identityName} GROUP BY DocumentId ORDER BY Count DESC";

                    }
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                
                                var item = new Tuple<int, DocumentInfo>(int.Parse(dataReader["Count"].ToString() ?? string.Empty), BllDocument.GetDocumentById("id", dataReader["DocumentId"].ToString()));
                                list.Add(item);

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

            return list;
        }


        public static List<int> GetSalesPerMonth(string documentType, string? identityName)
        {
            var list = new List<int>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    for (int month = 1; month <= 12; month++)
                    {
                        var sql = $"SELECT DocumentType, COUNT(*) AS Count FROM PurchaseHistory WHERE MONTH([Date]) = {month} AND [DocumentType]='{documentType}' GROUP BY [DocumentType] ";
                        if (!string.IsNullOrEmpty(identityName))
                        {
                            sql = $"SELECT DocumentType, COUNT(*) AS Count FROM PurchaseHistory WHERE MONTH([Date]) = {month} AND [DocumentType]='{documentType}' and  IdEditor = {identityName} GROUP BY [DocumentType] ";
                            
                        }
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                            using (var dataReader = command.ExecuteReader())
                            {
                                if (dataReader.Read())
                                {
                                    list.Add(int.Parse(dataReader["Count"].ToString() ?? string.Empty));
                                }
                                else
                                {                                    
                                    list.Add(0);
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

            return list;
            
        }

        public static int GetTotalUserType(string type)
        {
            var count = 0;
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"SELECT COUNT(*) FROM {type}";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        count = (int) command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return count;        }

        public static Dictionary<string,int> GetVisitorCounter()
        {
            var list = new Dictionary<string,int>()
            {
                {"Monday", 0},
                {"Tuesday", 0},
                {"Wednesday", 0},
                {"Thursday", 0},
                {"Friday", 0},
                {"Saturday", 0},
                {"Sunday", 0},
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT Date,Count(VisitorId) AS Count from Visitors GROUP BY Date";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                list[dataReader["Date"].ToString() ?? string.Empty] =
                                    int.Parse(dataReader["Count"].ToString() ?? string.Empty);
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

            return list;        
        }


    }
}