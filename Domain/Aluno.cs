namespace Cursos.API.Domain;

public class Aluno
{
    public int AlunoId { get; private set; }
    public string? NomeAluno { get; private set; }
    public string? Email { get; private set; }

    public DateTime DataNascimento { get; private set; }

    public bool EhMaiorDeIdade() 
        => (DateTime.Today.Year - DataNascimento.Year) >= 18;
}
