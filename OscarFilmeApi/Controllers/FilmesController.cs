using Microsoft.AspNetCore.Mvc;
using OscarFilmeApi.Data;
using OscarFilmeApi.Models;

namespace OscarFilmeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

        // A. POST
        [HttpPost]
        public IActionResult Criar(Filme filme)
        {
            if (filme.AnoLancamento < 1929)
                return BadRequest("O ano deve ser maior ou igual a 1929.");

            _context.Filmes.Add(filme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = filme.Id }, filme);
        }

        // B. GET geral
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Filmes.ToList());
        }

        // C. GET vencedores
        [HttpGet("vencedores")]
        public IActionResult GetVencedores()
        {
            var vencedores = _context.Filmes.Where(f => f.Venceu).ToList();
            return Ok(vencedores);
        }

        // D. PUT
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Filme filmeAtualizado)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
                return NotFound();

            filme.Titulo = filmeAtualizado.Titulo;
            filme.Diretor = filmeAtualizado.Diretor;
            filme.Categoria = filmeAtualizado.Categoria;
            filme.AnoLancamento = filmeAtualizado.AnoLancamento;
            filme.Venceu = filmeAtualizado.Venceu;

            if (filme.Venceu)
            {
                Console.WriteLine($"🏆 Temos um novo vencedor: {filme.Titulo}!");
            }

            _context.SaveChanges();

            return NoContent();
        }

        // E. DELETE
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var filme = _context.Filmes.Find(id);

            if (filme == null)
                return NotFound();

            _context.Filmes.Remove(filme);
            _context.SaveChanges();

            return NoContent();
        }

        // EXTRA - Estatísticas
        [HttpGet("estatisticas")]
        public IActionResult Estatisticas()
        {
            var total = _context.Filmes.Count();
            var vencedores = _context.Filmes.Count(f => f.Venceu);

            return Ok(new
            {
                TotalFilmes = total,
                TotalVencedores = vencedores
            });
        }
    }
}