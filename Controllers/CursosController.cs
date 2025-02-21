using Cursos.API.Context;
using Cursos.API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cursos.API.Controllers;

[Route("curso")]
public class CursosController : Controller
{
    private readonly AppDbContext _context;

    public CursosController(AppDbContext context)
    {
        _context = context;
    }

    // === GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Curso>>> ObterCursos()
    {
        var cursos = await _context.Cursos.ToListAsync();
        if (cursos == null) 
            return NotFound("Não existe nenhum curso!");

        return Ok(cursos);
    }

    [HttpGet("{cursoId}")]
    public async Task<ActionResult<Curso>> ObterCurso(int cursoId)
    {
        var curso = await _context.Cursos.FirstOrDefaultAsync(x => x.CursoId == cursoId);
        if (curso == null)
            return NotFound("Curso não encontrado.");

        return Ok(curso);
    }

    // === POST
    public record Post(string nome, string descricao);
    [HttpPost]
    public async Task<ActionResult> NovoCurso([FromBody] Post post)
    {
        if (string.IsNullOrWhiteSpace(post.nome))
            return BadRequest("Informe um nome válido!");

        if (string.IsNullOrWhiteSpace(post.descricao))
            return BadRequest("Informe uma descrição válida!");

        Curso curso = new Curso(post.nome, post.descricao);

        _context.Cursos.Add(curso);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterCurso), new { id = curso.CursoId }, curso);
    }

    // === PUT
    [HttpPut("{cursoId}")]
    public async Task<ActionResult> EditarCurso(int cursoId, Post post)
    {
        var curso = await _context.Cursos.FirstOrDefaultAsync(x => x.CursoId == cursoId);
        if (curso == null)
            return NotFound("Curso não encontrado.");
        int cont = 0;

        if (curso.NomeCurso != post.nome)
        {
            curso.AlterarNome(post.nome);
            cont++;
        }

        if (curso.Descricao != post.descricao)
        {
            curso.AlterarDescricao(post.descricao);
            cont++;
        }

        if (cont == 0)
            return BadRequest("Nenhuma alteração foi realizada!");

        _context.Entry(curso).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(curso);
    }

    // === DELETE
    [HttpDelete("{cursoId}")]
    public async Task<ActionResult> ExcluirCurso(int cursoId)
    {
        var curso = await _context.Cursos.FirstOrDefaultAsync(x => x.CursoId == cursoId);
        if (curso == null)
            return NotFound("Curso não encontrado.");

        _context.Cursos.Remove(curso);
        _context.SaveChanges();

        return Ok(curso);
    }

}
