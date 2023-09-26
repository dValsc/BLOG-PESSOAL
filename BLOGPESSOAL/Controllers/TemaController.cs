using BLOGPESSOAL.Model;
using BLOGPESSOAL.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BLOGPESSOAL.Controllers
{
    [Route("~/temas")]
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaService _temaService;
        private readonly IValidator<Tema> _temaValidator;

        public TemaController(ITemaService temaService, IValidator<Tema> temaValidator)
        {
            _temaService = temaService;
            _temaValidator = temaValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _temaService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var tema = await _temaService.GetById(id);

            if (tema == null)
                return NotFound("Tema não encontrado!");

            return Ok(tema);
        }

        [HttpGet("descricao/{descricao}")]
        public async Task<ActionResult> GetByDescricao(string descricao)
        {
            return Ok(await _temaService.GetByDescricao(descricao));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tema tema)
        {
            var validationResult = await _temaValidator.ValidateAsync(tema);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            await _temaService.Create(tema);
            return CreatedAtAction(nameof(GetById), new { id = tema.Id }, tema);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Tema tema)
        {
            if (tema.Id <= 0)
                return BadRequest("Id do tema inválido");

            var validationResult = await _temaValidator.ValidateAsync(tema);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var temaUpdate = await _temaService.Update(tema);

            if (temaUpdate == null)
                return NotFound("Tema não encontrado");

            return Ok(temaUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var tema = await _temaService.GetById(id);

            if (tema == null)
                return NotFound("Tema não encontrado");

            await _temaService.Delete(tema);
            return NoContent();
        }

    }
}
