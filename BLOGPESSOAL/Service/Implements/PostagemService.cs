using BLOGPESSOAL.Data;
using BLOGPESSOAL.Model;
using Microsoft.EntityFrameworkCore;

namespace BLOGPESSOAL.Service.Implements
{
    public class PostagemService : IPostagemService
    {

        private readonly AppDbContext _context;

        public PostagemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Postagem>> GetAll()
        {
            return await _context.Postagens.ToListAsync();
        }
    

        public Task<Postagem?> Create(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Postagem postagem)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            throw new NotImplementedException();
        }

        public Task<Postagem?> Update(Postagem postagem)
        {
            throw new NotImplementedException();
        }
    }
}
