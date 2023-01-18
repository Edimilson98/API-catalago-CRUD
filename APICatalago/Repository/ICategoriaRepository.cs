using APICatalago.Models;
using APICatalago.Pagination;

namespace APICatalago.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<PagedList<Categoria>>
        GetCategorias(CategoriasParameters categoriasParameters);
    Task<IEnumerable<Categoria>> GetCategoriasProdutos();
}
