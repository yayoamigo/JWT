using Jwt.Entity;

namespace Jwt.RepositoryContracts
{
    public interface IClienteRepository
    {
        Cliente Create(Cliente cliente);
        Cliente GetByEmail(string email);
        List<Cliente> GetAll();
        void Delete(Cliente cliente);
    }
}
