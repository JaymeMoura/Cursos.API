using Cursos.API.Context;
using Cursos.API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cursos.API.Controllers;

[Route("api/v1/matricula")]
public class MatriculaController : Controller
{
    private readonly AppDbContext _context;

    public MatriculaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{matriculaId}")]
    public async Task<ActionResult> ObterMatricula(int matriculaId)
    {
        Matricula? matricula = await _context.Matriculas.FirstOrDefaultAsync(x => x.MatriculaId == matriculaId);
        if (matricula == null)
            return NotFound("Matricula não existe ou não foi encontrada!");

        return Ok(matricula);
    }

    [HttpPost]
    public async Task<ActionResult> MatricularAluno(int alunoId, int cursoId)
    {
        Aluno? aluno = await _context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == alunoId);
        if (aluno == null)
            return NotFound("Aluno não existe ou não foi encontrado.");

        Curso? curso = await _context.Cursos.FirstOrDefaultAsync(x => x.CursoId == cursoId);
        if (curso == null)
            return NotFound("Curso não encontrado.");

        if (await _context.Matriculas.AnyAsync(x => x.CursoId == cursoId && x.AlunoId == alunoId))
            return NotFound("Aluno já esta matriculado nesse curso!");

        Matricula matricula = new(alunoId, cursoId);

        _context.Matriculas.Add(matricula);
        _context.SaveChanges();

        return CreatedAtAction(nameof(ObterMatricula), new { matriculaId = matricula.MatriculaId}, matricula);
    }


}
