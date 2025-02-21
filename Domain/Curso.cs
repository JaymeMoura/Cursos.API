namespace Cursos.API.Domain;

public class Curso
{
    public int CursoId { get; private set; }
    public string? NomeCurso { get; private set; }
    public string? Descricao { get; private set; }

    public Curso(string? nomeCurso, string? descricao)
    {
        NomeCurso = nomeCurso;
        Descricao = descricao;
    }

    public void AlterarNome(string nome)
        => NomeCurso = nome;

    public void AlterarDescricao(string descricao)
        => Descricao = descricao;
}
