using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters)
    {
        return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome),
                            categoriasParameters.PageNumber, 
                            categoriasParameters.PageSize);
    }

    public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return await Get().Include(x => x.Produtos).ToListAsync();
    }
}
