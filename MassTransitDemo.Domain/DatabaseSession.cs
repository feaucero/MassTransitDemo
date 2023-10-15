using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MassTransitDemo.Domain
{
    public class DatabaseSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction? Transaction { get; set; }
        public IConfiguration Configuration { get; set; }

        public DatabaseSession(IConfiguration configuration)
        {
            Configuration = configuration;
            Connection = new SqlConnection(Configuration.GetConnectionString("Demo"));
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
