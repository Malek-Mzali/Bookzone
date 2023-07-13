using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookzone.Models
{
    public static class DbConnection
    {
        private static readonly string DbConnnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
            .GetSection("DatabaseConfig")["DbConnnectionString"];
        public static SqlConnection GetConnection()
        {   
            return new SqlConnection(DbConnnectionString);
        }
    }
}
