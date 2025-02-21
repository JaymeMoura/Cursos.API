using Cursos.API.Context;
using Microsoft.AspNetCore.Mvc;

namespace Cursos.API.Controllers;

[Route("api/v1/curso")]
public class CursosController : Controller
{
    private readonly AppDbContext _context;

    public CursosController(AppDbContext context)
    {
        _context = context;
    }




}
