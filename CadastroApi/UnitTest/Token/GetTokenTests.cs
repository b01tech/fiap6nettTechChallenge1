﻿using CadastroApi.Application;
using CadastroApi.Controllers;
using CadastroApi.Models;
using CadastroApi.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest;

public class GetTokenTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TokenController _controller;

    public GetTokenTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new TokenController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetToken_InformadoDadosValidos_DeverRetornarOk()
    {
        var nome = "user";
        var senha = "password";
        var expectedToken = "generatedToken";

        _usuarioRepositoryMock
            .Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Usuario());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ListarTokenQueryHandler>(), default))
            .ReturnsAsync(expectedToken);

        var request = new Usuario { Nome = nome, Senha = senha };

        var result = await _controller.GetToken(request.Nome, request.Senha);

        var okResult = Assert.IsType<UnauthorizedResult>(result);
    }


    [Fact]
    public async Task GetToken_InformadoDadosInvalidos_DeverRetornarUnauthorized()
    {
        var nome = "testUser";
        var senha = "testPassword";

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<ListarTokenQueryHandler>(), default))
            .ReturnsAsync((string?)null);

        var request = new Usuario { Nome = nome, Senha = senha };

        var result = await _controller.GetToken(request.Nome, request.Senha);

        Assert.IsType<UnauthorizedResult>(result);
    }
}
