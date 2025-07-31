using Asp.Versioning;
using Library.API.Responses;
using Library.Application.Authors.Commands;
using Library.Application.Authors.Queries;
using Library.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.v1
{
    /// <summary>
    /// Controlador para gerenciar autores.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/authors")]
    [ApiVersion("1.0")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor do AuthorsController.
        /// </summary>
        /// <param name="mediator">Instância do mediator.</param>
        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os autores.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _mediator.Send(new GetAllAuthorsQuery());
            return Ok(new ApiResponse<object>(authors));
        }

        /// <summary>
        /// Retorna um autor pelo seu ID.
        /// </summary>
        /// <param name="id">ID do autor</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (author == null) throw new NotFoundException("Autor não encontrado");
            return Ok(new ApiResponse<object>(author));
        }

        /// <summary>
        /// Cria um novo autor.
        /// </summary>
        /// <param name="command">Dados do autor</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorCommand command)
        {
            var id = await _mediator.Send(command);
            var location = Url.Action(nameof(GetById), new { id });
            return Created(location!, new ApiResponse<Guid>(id));
        }

        /// <summary>
        /// Atualiza os dados de um autor existente.
        /// </summary>
        /// <param name="id">ID do autor</param>
        /// <param name="command">Dados atualizados</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAuthorCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return Ok(new ApiResponse<string>("Autor atualizado com sucesso."));
        }

        /// <summary>
        /// Remove um autor.
        /// </summary>
        /// <param name="id">ID do autor</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteAuthorCommand(id));
            return Ok(new ApiResponse<string>("Autor deletado com sucesso."));
        }
    }
}
