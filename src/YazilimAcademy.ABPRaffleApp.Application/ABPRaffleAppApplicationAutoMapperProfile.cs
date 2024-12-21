using AutoMapper;
using YazilimAcademy.ABPRaffleApp.Books;

namespace YazilimAcademy.ABPRaffleApp;

public class ABPRaffleAppApplicationAutoMapperProfile : Profile
{
    public ABPRaffleAppApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
