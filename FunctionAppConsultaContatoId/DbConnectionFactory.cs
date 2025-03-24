using FunctionAppConsultaContatoId.Interface;
using MySqlConnector;
using System.Data;

namespace FunctionAppConsultaContatoId
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            string? connectionString = Environment.GetEnvironmentVariable("MYSQLCONNECTIONSTRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("MYSQLCONNECTIONSTRING não está definida nas variáveis de ambiente.");
            }
            return new MySqlConnection(connectionString);
        }
    }
}
