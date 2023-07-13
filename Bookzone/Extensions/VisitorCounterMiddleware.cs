using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bookzone.Models;
using Microsoft.AspNetCore.Http;

namespace Bookzone.Extensions
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;

        }

        public async Task Invoke(HttpContext context)
        {
            string visitorId = context.Request.Cookies["VisitorId"];
            
            if (visitorId == null)
            {
                var abc = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                context.Response.Cookies.Append("VisitorId", abc, new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    Secure = true,
                });
                 visitorId = abc;
                 var agent = context.Request.Headers["User-Agent"].ToString();
                 try
                {
                    using (var connection = DbConnection.GetConnection())
                    {
                        connection.Open();
                        var sql1 = "Select VisitorId,Date From Visitors Where VisitorId = @VisitorId and  Agent=@Agent";
                        using (var command = new SqlCommand(sql1, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@VisitorId", visitorId);
                            command.Parameters.AddWithValue("@Agent", agent);
                            using (var dataReader = command.ExecuteReader())
                            {
                                if (!dataReader.Read())
                                {
                                    connection.Close();
                                    connection.Open();
                                    var sql2 = "Insert Into Visitors(VisitorId, Date, Agent) values (@VisitorId, @Date, @Agent)";
                                    using (var command2 = new SqlCommand(sql2, connection))
                                    {
                                        command2.CommandType = CommandType.Text;
                                        command2.Parameters.AddWithValue("@VisitorId", visitorId);
                                        command2.Parameters.AddWithValue("@Date", DateTime.Today.DayOfWeek.ToString());
                                        command2.Parameters.AddWithValue("@Agent", agent);
                                        command2.ExecuteNonQuery();
                                    }  
                                    connection.Close();
                                }
                                else
                                {
                                    if (dataReader["Date"].ToString() != DateTime.Today.DayOfWeek.ToString())
                                    {
                                        connection.Close();
                                        connection.Open();
                                        var sql2 = "Update  Visitors SET Date=@Date where VisitorId=@VisitorId and Agent=@Agent";
                                        using (var command2 = new SqlCommand(sql2, connection))
                                        {
                                            command2.CommandType = CommandType.Text;
                                            command2.Parameters.AddWithValue("@VisitorId", visitorId);
                                            command2.Parameters.AddWithValue("@Date", DateTime.Today.DayOfWeek.ToString());
                                            command2.Parameters.AddWithValue("@Agent", agent);
                                            command2.ExecuteNonQuery();
                                        }
                                        connection.Close();
                                    }
                                }

                            }                    
                        }
                        
                    }
                }
            catch
                {
                    // ignored
                }
            }
            await _requestDelegate(context);
            
        }
    }
}