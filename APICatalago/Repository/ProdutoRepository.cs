using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public  async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
    {
        return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId), 
            produtosParameters.PageNumber, produtosParameters.PageSize);
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
    {
        return await Get().OrderBy(c => c.Preco).ToListAsync();
    }
}
