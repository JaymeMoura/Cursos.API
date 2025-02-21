using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Cursos.API.Domain;

public class Aluno
{
    public int AlunoId { get; private set; }
    public string? NomeAluno { get; private set; }
    public string? Email { get; private set; }

    public DateTime DataNascimento { get; private set; }

    public Aluno(string? nomeAluno, string? email, DateTime dataNascimento)
    {
        NomeAluno = nomeAluno;
        Email = email;
        DataNascimento = dataNascimento;
    }

    public bool EhMaiorDeIdade() 
        => (DateTime.Today.Year - DataNascimento.Year) >= 18;

    public void AlterarNome(string nomeAluno)
        => NomeAluno = nomeAluno.Trim();

    public void AlterarEmail(string email)
        => Email = email.Trim();

    public bool IsValidEmail(string strIn)
    {
        return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }
}
