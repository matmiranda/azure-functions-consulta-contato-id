using System.Data;

namespace FunctionAppConsultaContatoId.Interface
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
