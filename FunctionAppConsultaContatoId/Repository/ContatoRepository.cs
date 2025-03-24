using Dapper;
using FunctionAppConsultaContatoId.Entity;
using FunctionAppConsultaContatoId.Interface;
using System.Data;

namespace FunctionAppConsultaContatoId.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        public async Task<ContatoEntity?> GetContatoByIdAsync(IDbConnection dbConnection, int id)
        {
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.Id = @id";
            return await dbConnection.QueryFirstOrDefaultAsync<ContatoEntity?>(sql, new { Id = id });
        }
    }
}
