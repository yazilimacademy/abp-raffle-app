using AutoMapper;
using YazilimAcademy.ABPRaffleApp.Books;
using YazilimAcademy.ABPRaffleApp.Raffles;

namespace YazilimAcademy.ABPRaffleApp.Blazor.Client;

public class ABPRaffleAppBlazorAutoMapperProfile : Profile
{
    public ABPRaffleAppBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();

        CreateMap<RaffleDto, CreateUpdateRaffleDto>();

        //Define your AutoMapper configuration here for the Blazor project.
    }
}