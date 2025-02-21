using Cursos.API.Context;
using Microsoft.AspNetCore.Mvc;

namespace Cursos.API.Controllers;

[Route("api/v1/matricula")]
public class MatriculaController : Controller
{
    private readonly AppDbContext _context;

    public MatriculaController(AppDbContext context)
    {
        _context = context;
    }




}
