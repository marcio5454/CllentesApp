using Bogus;
using ClientesApp.Domain.Entidades;
using ClientesApp.Infra.Data.SqlServer.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace ClientesApp.Infra.Data.SqlServer.Tests
{
    public class ClienteRepositoryTest
    {
        private readonly Faker<Cliente> _fakerCliente;
        private readonly DataContext _dataContext;
        private readonly ClienteRepository _clienteRepository;

        public ClienteRepositoryTest()
        {
            //configurando o Bogus para construir um cliente
            _fakerCliente = new Faker<Cliente>("pt_BR")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Cpf, f => f.Random.Replace("###########"));

            //configurar o DataContext para utilizar banco de memória no EF
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ClientesAppTestsDB")
                .Options;

            //criando o mock da classe DataContext
            _dataContext = new DataContext(options);

            //instanciando a classe de repositório
            _clienteRepository = new ClienteRepository(_dataContext);
        }

        [Fact(DisplayName = "Adicionar cliente com sucesso no repositório.")]
        public async Task AddAsync_ShouldAddCliente()
        {
            //gerando os dados do cliente
            var cliente = _fakerCliente.Generate();

            //gravando o cliente no banco de dados
            await _clienteRepository.AddAsync(cliente);

            //buscar cliente no banco de dados
            // var _clienteCadastrado = await _clienteRepository.GetByIdAsync(Guid.NewGuid());
            var _clienteCadastrado = await _clienteRepository.GetByIdAsync(cliente.Id);

            //verificar se o cliente foi cadastrado (asserções de teste)
            //_clienteCadastrado?.Should().NotBeNull();

            if (_clienteCadastrado == null)
            {   
                Assert.True(false, "Cliente não cadastrado");
                return;
            }
                
            _clienteCadastrado.Nome.Should().BeSameAs(cliente.Nome);
            _clienteCadastrado.Email.Should().BeSameAs(cliente.Email);
            _clienteCadastrado.Cpf.Should().BeSameAs(cliente.Cpf);
        }
    }
}

