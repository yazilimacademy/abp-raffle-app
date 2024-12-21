using AutoMapper;
using YazilimAcademy.ABPRaffleApp.Books;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Raffles;

namespace YazilimAcademy.ABPRaffleApp;

public class ABPRaffleAppApplicationAutoMapperProfile : Profile
{
    public ABPRaffleAppApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        CreateMap<Raffle, RaffleDto>();
        CreateMap<CreateUpdateRaffleDto, Raffle>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
