using Microsoft.AspNetCore.Mvc;
using WorkersApp.Controllers.Response;
using WorkersApp.Data;
using WorkersApp.Models;

namespace WorkersApp.Controllers;

public class TrabajadorController : Controller
{
    private readonly AppDbContext _context;

    public TrabajadorController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string sexo)
    {
        var trabajadores = _context.Trabajadores.ToList();

        if (!string.IsNullOrEmpty(sexo))
        {
            trabajadores = trabajadores.Where(t => t.Sexo.ToString() == sexo).ToList();
        }

        var departamentos = _context.Departamento.ToList();
        var provincias = _context.Provincia.ToList();
        var distritos = _context.Distrito.ToList();

        var response = trabajadores.Select(trabajador =>
        {
            var departamento = departamentos.FirstOrDefault(d => d.Id == trabajador.IdDepartamento);
            var provincia = provincias.FirstOrDefault(p => p.Id == trabajador.IdProvincia);
            var distrito = distritos.FirstOrDefault(d => d.Id == trabajador.IdDistrito);

            return new TrabajadorResponse
            {
                Id = trabajador.Id,
                TipoDocumento = trabajador.TipoDocumento,
                NumeroDocumento = trabajador.NumeroDocumento,
                Nombres = trabajador.Nombres,
                Sexo = trabajador.Sexo,
                Departamento = departamento?.NombreDepartamento ?? "Desconocido",
                Provincia = provincia?.NombreProvincia ?? "Desconocida",
                Distrito = distrito?.NombreDistrito ?? "Desconocido"
            };
        }).ToList();

        return View(response);
    }


    public IActionResult GetById(int id)
    {

        if (id <= 0)
        {
            return BadRequest("ID inválido.");
        }

        var trabajador = _context.Trabajadores.FirstOrDefault(t => t.Id == id);
        if (trabajador == null)
        {
            return View(new List<TrabajadorResponse>());
        }

        var departamento = _context.Departamento.FirstOrDefault(d => d.Id == trabajador.IdDepartamento);
        var provincia = _context.Provincia.FirstOrDefault(p => p.Id == trabajador.IdProvincia);
        var distrito = _context.Distrito.FirstOrDefault(d => d.Id == trabajador.IdDistrito);

        var response = new TrabajadorResponse
        {
            Id = trabajador.Id,
            TipoDocumento = trabajador.TipoDocumento,
            NumeroDocumento = trabajador.NumeroDocumento,
            Nombres = trabajador.Nombres,
            Sexo = trabajador.Sexo,
            Departamento = departamento?.NombreDepartamento ?? "",
            Provincia = provincia?.NombreProvincia ?? "",
            Distrito = distrito?.NombreDistrito ?? ""
        };

        return View(response);
    }

    public IActionResult CreateWorker()
    {
        ViewBag.Departamentos = _context.Departamento.ToList();
        ViewBag.Provincias = _context.Provincia.ToList();
        ViewBag.Distritos = _context.Distrito.ToList();

        return View();
    }

    public IActionResult Edit(int id)
    {
        var trabajador = _context.Trabajadores.FirstOrDefault(t => t.Id == id);
        if (trabajador == null)
            return NotFound();

        ViewBag.Departamentos = _context.Departamento.ToList();
        ViewBag.Provincias = _context.Provincia.ToList();
        ViewBag.Distritos = _context.Distrito.ToList();

        return View(trabajador);
    }

    public IActionResult Delete(int id)
    {
        var trabajador = _context.Trabajadores.FirstOrDefault(t => t.Id == id);
        if (trabajador == null)
            return NotFound();

        return View(trabajador);
    }

    [HttpPost]
    public IActionResult Edit(Trabajador trabajador)
    {
        if (trabajador == null)
            return NotFound();

        if (ModelState.IsValid)
        {
            var original = _context.Trabajadores.FirstOrDefault(t => t.Id == trabajador.Id);
            if (original == null)
                return NotFound();

            original.Sexo = trabajador.Sexo;
            original.IdDepartamento = trabajador.IdDepartamento;
            original.IdProvincia = trabajador.IdProvincia;
            original.IdDistrito = trabajador.IdDistrito;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Departamentos = _context.Departamento.ToList();
        ViewBag.Provincias = _context.Provincia.ToList();
        ViewBag.Distritos = _context.Distrito.ToList();
        return View(trabajador);
    }

    [HttpPost]
    public IActionResult CreateWorker(Trabajador trabajador)
    {
        if (trabajador == null)
        {
            ModelState.AddModelError("", "Debe de haber datos del trabajador!.");
            return View(trabajador);

        }

        if (ModelState.IsValid)
        {
            _context.Trabajadores.Add(trabajador);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(trabajador);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var trabajador = _context.Trabajadores.FirstOrDefault(t => t.Id == id);
        if (trabajador == null)
            return NotFound();

        _context.Trabajadores.Remove(trabajador);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
