using AutoMapper;
using DevIO.API.Dtos;
using DevIO.API.Extensions;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.API.Controllers
{
    [Authorize] // define a necessidade de autenticação para acessar os métodos
    [Route("api/fornecedor")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedorController(IFornecedorRepository fornecedorRepository,
                                    IMapper mapper,
                                    IFornecedorService fornecedorService,
                                    INotificador notificador,
                                    IEnderecoRepository enderecoRepository,
                                    IUser user) : base(notificador, user)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _enderecoRepository = enderecoRepository;
        }

        //[AllowAnonymous] //permite acesso sem autenticação
        [HttpGet]
        public async Task<IEnumerable<FornecedorDto>> ObterTodos()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorDto>>( await _fornecedorRepository.ObterTodos());

            return fornecedores;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);
            //var fornecedor = _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

            if (fornecedor == null) return NotFound();

            return Ok(fornecedor);
        }

        [ClaimsAuthorize("Fornecedor","Adicionar")] // verifica se tem permissão para realizar o post de acordo com a tabela: AspNetUserClaims. Config em extensions/CustomAuthorize.cs
        [HttpPost]
        public async Task<ActionResult<FornecedorDto>> Adicionar(FornecedorDto fornecedorDto)
        {
            //obtendo o nome do usuário autenticado antes da implementação na MainController
            //if (User.Identity.IsAuthenticated)
            //{
            //    var userName = User.Identity.Name;
            //}
            //obtendo o nome do usuário autenticado depois da implementação na MainController
            //if (UsuarioAutenticado)
            //{
            //    var nomeUser = AppUser.Name;
            //    var nomeEmail = AppUser.GetUserEmail();
            //    var userId = AppUser.GetUserId();
            //    var userId2 = UsuarioId;
            //}

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorDto));
            return CustomResponse(fornecedorDto);

            //Antes de customizar as mensagens
            //var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            //var result = await _fornecedorService.Adicionar(fornecedor);
            //if (!result) return BadRequest();
            //return Ok(fornecedor);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")] //verifica se tem permissão para realizar o put de acordo com a tabela: AspNetUserClaims. Config em extensions/CustomAuthorize.cs
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Atualizar(Guid id, FornecedorDto fornecedorDto)
        {
            if (id != fornecedorDto.id)
            {
                //return BadRequest();
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(fornecedorDto);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorDto));
            return CustomResponse(fornecedorDto);

            //Antes de customizar as mensagens
            //var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            //var result = await _fornecedorService.Atualizar(fornecedor);
            //if (!result) return BadRequest();
            //return Ok(fornecedor);
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")] //verifica se tem permissão para realizar o delete de acordo com a tabela: AspNetUserClaims. Config em extensions/CustomAuthorize.cs
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Excluir(Guid id)
        {
            var fornecedorDto = await ObterFornecedorEndereco(id);
            //var fornecedorDto = _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedorDto == null) return NotFound();

            await _fornecedorService.Remover(id);
            return CustomResponse(fornecedorDto);

            //Antes de customizar as mensagens
            //var result = await _fornecedorService.Remover(id);
            //if (!result) return BadRequest();
            //return Ok(fornecedor);
        }

        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<ActionResult<EnderecoDto>> ObterEnderecoPorId(Guid id)
        {
            var enderecoDto = _mapper.Map<EnderecoDto>(await _enderecoRepository.ObterPorId(id));

            if (enderecoDto == null) return NotFound();

            return Ok(enderecoDto);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")] //verifica se tem permissão para realizar o put de acordo com a tabela: AspNetUserClaims. Config em extensions/CustomAuthorize.cs
        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<ActionResult<EnderecoDto>> AtualizarEndereco(Guid id, EnderecoDto enderecoDto)
        {
            if (id != enderecoDto.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoDto);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _enderecoRepository.Atualizar(_mapper.Map<Endereco>(enderecoDto));
            return CustomResponse(enderecoDto);
        }

        private async Task<FornecedorDto> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        private async Task<FornecedorDto> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }
    }
}
