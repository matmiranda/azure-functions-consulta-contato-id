using FunctionAppConsultaContatoId.Entity;
using FunctionAppConsultaContatoId.Response;

namespace FunctionAppConsultaContatoId.Mapper
{
    public static class ContatoMapper
    {
        public static ContatosGetResponse ContatoPorId(ContatoEntity contatoEntity)
        {
            return new ContatosGetResponse
            {
                Id = contatoEntity.Id,
                Nome = contatoEntity.Nome,
                Telefone = contatoEntity.Telefone,
                Email = contatoEntity.Email,
                DDD = contatoEntity.DDD,
                Regiao = contatoEntity.Regiao.ToString()
            };
        }
    }
}
