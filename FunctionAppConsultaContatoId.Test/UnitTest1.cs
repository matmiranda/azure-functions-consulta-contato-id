using Dapper;
using FunctionAppConsultaContatoId.Entity;
using FunctionAppConsultaContatoId.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;

namespace FunctionAppConsultaContatoId.Test
{
    public class GetContatoFunctionTests
    {
        private Mock<ILogger<GetContatoFunction>> _loggerMock;
        private Mock<IDbConnectionFactory> _dbConnectionFactoryMock;
        private Mock<IContatoRepository> _contatoRepositoryMock;
        private GetContatoFunction _function;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetContatoFunction>>();
            _dbConnectionFactoryMock = new Mock<IDbConnectionFactory>();
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _function = new GetContatoFunction(_loggerMock.Object, _dbConnectionFactoryMock.Object, _contatoRepositoryMock.Object);
        }

        [Test]
        public async Task Run_DeveRetornarOk_QuandoContatoEncontrado()
        {
            // Arrange: Simula uma requisição com ID válido
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.RouteValues["id"] = "1"; // ID válido

            var contatoEntity = new ContatoEntity
            {
                Id = 1,
                Nome = "Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11,
                Regiao = Enum.RegiaoEnum.Sudeste
            };

            var dbConnectionMock = new Mock<IDbConnection>();

            _dbConnectionFactoryMock.Setup(factory => factory.CreateConnection())
                                    .Returns(dbConnectionMock.Object);

            _contatoRepositoryMock.Setup(repo => repo.GetContatoByIdAsync(It.IsAny<IDbConnection>(), It.IsAny<int>()))
                                  .ReturnsAsync((ContatoEntity?)contatoEntity);

            // Act
            var result = await _function.Run(request, 1); // Passa um ID válido

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task Run_DeveRetornarNotFound_QuandoContatoNaoEncontrado()
        {
            // Arrange: Simula uma requisição com ID válido
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.RouteValues["id"] = "1"; // ID válido

            var dbConnectionMock = new Mock<IDbConnection>();

            _dbConnectionFactoryMock.Setup(factory => factory.CreateConnection())
                                    .Returns(dbConnectionMock.Object);

            _contatoRepositoryMock.Setup(repo => repo.GetContatoByIdAsync(It.IsAny<IDbConnection>(), It.IsAny<int>()))
                                  .ReturnsAsync((ContatoEntity?)null); // Simula que o contato não foi encontrado

            // Act
            var result = await _function.Run(request, 1); // Passa um ID válido

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Run_DeveRetornar500_QuandoStringDeConexaoNaoDefinida()
        {
            // Arrange: Simula uma requisição válida, mas sem conexão com banco
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.RouteValues["id"] = "1";

            // Simula que a variável de ambiente está vazia
            System.Environment.SetEnvironmentVariable("MYSQLCONNECTIONSTRING", null);

            // Act
            var result = await _function.Run(request, 1); // Passa um ID válido

            // Assert
            Assert.IsInstanceOf<StatusCodeResult>(result);
            var statusResult = result as StatusCodeResult;
            Assert.IsNotNull(statusResult);
            Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
