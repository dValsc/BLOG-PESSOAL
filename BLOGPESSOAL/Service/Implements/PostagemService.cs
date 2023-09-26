using BLOGPESSOAL.Data;
using BLOGPESSOAL.Model;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

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
    

        public async Task<Postagem?> Create(Postagem postagem)
        {
            await _context.Postagens.AddAsync(postagem);
            await _context.SaveChangesAsync();

            return postagem;
        }

        public async Task Delete(Postagem postagem)
        {
            _context.Remove(postagem);

            await _context.SaveChangesAsync();
        }

        public async Task<Postagem?> GetById(long id)
        {
            try
            {
                var postagem = await _context.Postagens.FirstAsync(i => i.Id == id);
                return postagem;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            var postagem = await _context.Postagens.Where(t => t.Titulo.Contains(titulo)).ToListAsync();
            return postagem;
        }

        public async Task<Postagem?> Update(Postagem postagem)
        {
            var postagemUpdate = await _context.Postagens.FindAsync(postagem.Id);

            if (postagemUpdate == null)
                return null;

            _context.Entry(postagemUpdate).State = EntityState.Detached;
            _context.Entry(postagem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return postagem;
        }
    }
}
