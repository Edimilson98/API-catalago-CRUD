using APICatalago.Context;
using APICatalago.DTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalago.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[Controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _context;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
    {
        var categorias = await _context.CategoriaRepository.GetCategoriasProdutos();
        var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

        return categoriasDto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> 
        Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _context.CategoriaRepository.
            GetCategorias(categoriasParameters);

        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
        return categoriasDto;
    }

    [HttpGet("{id}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound("Categoria não encontrada...");
        }

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
        return categoriaDto;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
    {
        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _context.CategoriaRepository.Add(categoria);
        await _context.Commit();

        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(int id, [FromBody] CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            return BadRequest();
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _context.CategoriaRepository.Update(categoria);
        await _context.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada...");
        }
        _context.CategoriaRepository.Delete(categoria);
        await _context.Commit();

        return Ok(categoria);
    }
}
