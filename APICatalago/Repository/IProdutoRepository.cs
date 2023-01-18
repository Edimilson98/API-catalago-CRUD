using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);

    Task<IEnumerable<Produto>> GetProdutosPorPreco();
}
