using FunctionAppConsultaContatoId.Entity;
using System.Data;

namespace FunctionAppConsultaContatoId.Interface
{
    public interface IContatoRepository
    {
        Task<ContatoEntity> GetContatoByIdAsync(IDbConnection dbConnection, int id);
    }
}
