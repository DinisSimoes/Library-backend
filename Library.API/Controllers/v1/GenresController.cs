using Asp.Versioning;
using Library.API.Responses;
using Library.Application.Exceptions;
using Library.Application.Genres;
using Library.Application.Genres.Commands;
using Library.Application.Genres.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Library.API.Controllers.v1
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas a gêneros.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/genres")]
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor do <see cref="GenresController"/>.
        /// </summary>
        /// <param name="mediator">Instância do MediatR para envio de comandos e consultas.</param>
        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém todos os gêneros.
        /// </summary>
        /// <returns>Lista de gêneros.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GenreDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _mediator.Send(new GetAllGenresQuery());
            return Ok(new ApiResponse<IEnumerable<GenreDto>>(genres));
        }

        /// <summary>
        /// Obtém um gênero pelo Id.
        /// </summary>
        /// <param name="id">Id do gênero.</param>
        /// <returns>O gênero solicitado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var genre = await _mediator.Send(new GetGenreByIdQuery(id));
            if (genre is null)
                throw new NotFoundException("Gênero não encontrado");

            return Ok(new ApiResponse<GenreDto>(genre));
        }

        /// <summary>
        /// Cria um novo gênero.
        /// </summary>
        /// <param name="command">Dados para criação do gênero.</param>
        /// <returns>Id do gênero criado.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateGenreCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<Guid>(id));
        }

        /// <summary>
        /// Atualiza um gênero existente.
        /// </summary>
        /// <param name="id">Id do gênero a ser atualizado.</param>
        /// <param name="command">Dados para atualização do gênero.</param>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreCommand command)
        {
            if (id != command.Id)
                return BadRequest(new ApiResponse<string>("ID do gênero não confere com o corpo da requisição."));

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Remove um gênero.
        /// </summary>
        /// <param name="id">Id do gênero a ser removido.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteGenreCommand(id));
            return NoContent();
        }
    }
}
