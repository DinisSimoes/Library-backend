using Asp.Versioning;
using Library.API.Responses;
using Library.Application.Books.Commands;
using Library.Application.Books.Queries;
using Library.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers.v1
{
    /// <summary>
    /// Controlador para gerenciar livros.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/books")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor do controlador de livros.
        /// </summary>
        /// <param name="mediator">Instância do mediator para enviar comandos e queries.</param>
        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os livros.
        /// </summary>
        /// <returns>Uma lista de livros.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());
            return Ok(new ApiResponse<object>(result));
        }

        /// <summary>
        /// Retorna um livro específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <returns>Um livro ou 404 se não encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id));
            if (result is null) throw new NotFoundException("Livro não encontrado");
            return Ok(new ApiResponse<object>(result));
        }

        /// <summary>
        /// Cria um novo livro.
        /// </summary>
        /// <param name="command">Comando para criação do livro.</param>
        /// <returns>201 Created com o ID do livro criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new ApiResponse<Guid>(id));
        }

        /// <summary>
        /// Atualiza um livro existente.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <param name="command">Comando com os dados atualizados.</param>
        /// <returns>204 No Content se atualizado com sucesso, 400 se o ID não coincidir.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBookCommand command)
        {
            if (id != command.Id) 
                return BadRequest(new ApiResponse<string>("ID do livro não confere com o corpo da requisição."));

            var result = await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Remove um livro existente.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <returns>204 No Content se removido com sucesso.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));
            return NoContent();
        }
    }
}
