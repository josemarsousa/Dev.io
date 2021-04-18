using AutoMapper;
using DevIO.API.Dtos;
using DevIO.Business.Models;

namespace DevIO.API.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            //CreateMap<Produto, ProdutoDto>().ReverseMap();

            CreateMap<ProdutoDto, Produto>();
            // Para litar o nome do fornecedor
            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));

            CreateMap<ProdutoImagemDto, Produto>().ReverseMap();

        }
    }
}
