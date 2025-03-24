using Dapper;
using FunctionAppConsultaContatoId.Entity;
using FunctionAppConsultaContatoId.Interface;
using FunctionAppConsultaContatoId.Mapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunctionAppConsultaContatoId
{
    public class GetContatoFunction
    {
        private readonly ILogger<GetContatoFunction> _logger;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IContatoRepository _contatoRepository;

        public GetContatoFunction(ILogger<GetContatoFunction> logger, IDbConnectionFactory dbConnectionFactory, IContatoRepository contatoRepository)
        {
            _logger = logger;
            _dbConnectionFactory = dbConnectionFactory;
            _contatoRepository = contatoRepository;
        }

        [Function("GetContatoFunction")]
        public async Task<IActionResult> Run(
                    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contato/{id}")] HttpRequest req, int id)
        {
            _logger.LogInformation("Iniciando execução da função GetContatoFunction.");

            try
            {
                using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
                dbConnection.Open();
                _logger.LogInformation("Conexão com o banco de dados estabelecida.");
                var contatoEntity = await _contatoRepository.GetContatoByIdAsync(dbConnection, id);                
                if (contatoEntity != null)
                {
                    _logger.LogInformation($"Contato encontrado: {contatoEntity.Nome}");
                    var response = ContatoMapper.ContatoPorId(contatoEntity);
                    return new OkObjectResult(response);
                }
                else
                {
                    _logger.LogWarning($"Contato com ID {id} não encontrado.");
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar a função: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
