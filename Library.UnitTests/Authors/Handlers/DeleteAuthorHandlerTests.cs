using Library.Application.Authors.Commands;
using Library.Application.Authors.Handlers;
using Library.Application.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Authors.Handlers
{
    public class DeleteAuthorHandlerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly DeleteAuthorHandler _handler;

        public DeleteAuthorHandlerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _handler = new DeleteAuthorHandler(_authorServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_CallsDeleteAsyncAndReturnsUnit()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorServiceMock.Setup(s => s.DeleteAsync(authorId)).Returns(Task.CompletedTask);

            var command = new DeleteAuthorCommand(authorId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _authorServiceMock.Verify(s => s.DeleteAsync(authorId), Times.Once);
            Assert.Equal(Unit.Value, result);
        }
    }
}
