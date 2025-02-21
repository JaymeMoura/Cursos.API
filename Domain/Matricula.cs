namespace Cursos.API.Domain;

public class Matricula
{
    public int MatriculaId { get; private set; }
    public int AlunoId { get; private set; }
    public Aluno Aluno { get; private set; } = null!;

    public int CursoId { get; private set; }
    public Curso Curso { get; private set; } = null!;

    public DateTime DataMatricula {  get; set; }

    public Matricula(int alunoId, int cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        DataMatricula = DateTime.Now;
    }
}
