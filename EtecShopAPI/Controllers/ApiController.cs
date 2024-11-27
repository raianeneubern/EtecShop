using EtecShopAPI.Data;
using EtecShopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EtecShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController(AppDbContext db) : ControllerBase
{
    private readonly AppDbContext _db = db; 

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get() => Ok(await _db.Categorias.ToListAsync());

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id) =>
            _db.Categorias.Any(c => c.Id == id) ?
                 Ok(await _db.Categorias.FindAsync(id)) :
                 NotFound("Categorias não encotradas!");

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest("Categoria informada com problemas");
            if (_db.Categorias.Any(c => c.Nome == categoria.Nome))
                return BadRequest($"Já existe uma Categoria com o nome '{categoria.Nome}'");
            await _db.Categorias.AddAsync(categoria);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), categoria.Id, new { categoria });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(int id, [FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid || id != categoria.Id)
                return BadRequest("Categoria informada com problemas");
            if (!_db.Categorias.Any(c => c.Id == id))
                return NotFound("Categoria não encontrada!");
            _db.Categorias.Update(categoria);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            Categoria categoria = _db.Categorias.Find(id);
            if (categoria == null)
                return NotFound("Categoria não encontrada!");
             _db.Categorias.Remove(categoria);
            await _db.SaveChangesAsync();
            return NoContent();
        }
}
