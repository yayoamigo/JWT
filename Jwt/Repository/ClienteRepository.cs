using Jwt.DBcontext;
using Jwt.Entity;
using Jwt.RepositoryContracts;

namespace Jwt.ServiceContracts
{
    public class ClienteRepository : IClienteRepository
    {

        private readonly JWTContext _db;

        public ClienteRepository(JWTContext jwtContext)
        {
            _db = jwtContext;
        }
        


        public Cliente Create(Cliente cliente)
        {
            try
            {
                _db.Add(cliente);
                _db.SaveChanges();

                return cliente;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void Delete(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            _db.Remove(cliente);
            _db.SaveChanges();
        }


        public List<Cliente> GetAll()
        {
            return _db.Set<Cliente>().ToList();
        }


        public Cliente GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            var cliente = _db.Set<Cliente>().FirstOrDefault(c => c.Email == email);

            if (cliente == null)
            {
                return null;
            }
            else
            {
                return cliente;
            }
        }

    }
}
