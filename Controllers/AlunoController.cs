using Cursos.API.Context;
using Cursos.API.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Cursos.API.Controllers;

[Route("api/v1/aluno")]
public class AlunoController : Controller
{
    private readonly AppDbContext _context;

    public AlunoController(AppDbContext context)
    {
        _context = context;
    }

    // === GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Aluno>>> ObterAlunos()
    {
        var alunos = await _context.Alunos.ToListAsync();
        if (alunos == null)
            return NotFound("Não existe nenhum aluno!");

        return Ok(alunos);
    }

    [HttpGet("{alunoId}")]
    public async Task<ActionResult<Aluno>> ObterAluno(int alunoId)
    {
        var aluno = await _context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == alunoId);
        if (aluno == null)
            return NotFound("Aluno não existe ou não foi encontrado.");

        return Ok(aluno);
    }

    // === POST
    public record AlunoPost(string nomeAluno, string email, DateTime dataNascimento);
    [HttpPost]
    public async Task<ActionResult> NovoAluno([FromBody] AlunoPost post)
    {
        if (post == null)
            return BadRequest("Informe uma data Valida!");

        if (string.IsNullOrWhiteSpace(post.nomeAluno))
            return BadRequest("Informe um nome válido para o Aluno(a)!");

        if (string.IsNullOrWhiteSpace(post.email))
            return BadRequest("Informe um email válido!");

        if (post.dataNascimento == DateTime.MinValue)
            return BadRequest("Informe uma data Valida!");

        if (await _context.Alunos.AnyAsync(x => x.Email == post.email))
            return BadRequest("Já existe um aluno ultilizando esse email!");

        Aluno aluno = new(post.nomeAluno, post.email, post.dataNascimento);

        if (aluno.IsValidEmail(post.email) == false)
            return BadRequest("Informe um email válido!");

        if (aluno.EhMaiorDeIdade() == false)
            return BadRequest("Não é possível cadastrar aluno menor de idade!");

        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterAluno), new { alunoId = aluno.AlunoId }, aluno);
    }

    // === PUT
    public record Put(string nomeAluno, string email);
    [HttpPut("{alunoId}")]
    public async Task<ActionResult> EditarAluno(int alunoId, Put put)
    {
        var aluno = await _context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == alunoId);

        if (aluno == null)
            return NotFound("Aluno(a) não existe ou não foi encontrado.");

        int cont = 0;

        if (string.IsNullOrWhiteSpace(put.nomeAluno))
            return BadRequest("Informe um nome válido para o Aluno(a)!");

        if (string.IsNullOrWhiteSpace(put.email))
            return BadRequest("Informe um email válido!");

        if (aluno.NomeAluno != put.nomeAluno)
        {
            aluno.AlterarNome(put.nomeAluno);
            cont++;
        }

        if (aluno.Email != put.email)
        {
            aluno.AlterarEmail(put.email);
            cont++;
        }

        if (cont == 0)
            return BadRequest("Nenhuma alteração foi realizada!");

        if (aluno.IsValidEmail(put.email) == false)
            return BadRequest("Informe um email válido!");

        if (aluno.EhMaiorDeIdade() == false)
            return BadRequest("Não é possível cadastrar aluno menor de idade!");

        _context.Entry(aluno).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(aluno);
    }

    // === DELETE
    [HttpDelete("{alunoId}")]
    public async Task<ActionResult> ExcluirAluno(int alunoId)
    {
        var aluno = await _context.Alunos.FirstOrDefaultAsync(x => x.AlunoId == alunoId);
        if (aluno == null)
            return NotFound("Aluno(a) não existe ou não foi encontrado.");

        _context.Alunos.Remove(aluno);
        _context.SaveChanges();

        return Ok(aluno);
    }


}
