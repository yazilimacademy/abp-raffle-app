using AutoMapper;
using YazilimAcademy.ABPRaffleApp.Books;

namespace YazilimAcademy.ABPRaffleApp.Blazor.Client;

public class ABPRaffleAppBlazorAutoMapperProfile : Profile
{
    public ABPRaffleAppBlazorAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        
        //Define your AutoMapper configuration here for the Blazor project.
    }
}