using AutoMapper;
using Volo.Abp.AutoMapper;
using YazilimAcademy.ABPRaffleApp.Books;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;
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
        CreateMap<RaffleDto, CreateUpdateRaffleDto>();

        CreateMap<Participant, ParticipantDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        CreateMap<CreateUpdateParticipantDto, FullName>()
            .ConstructUsing(src => FullName.Create($"{src.FirstName} {src.LastName}"));
    }
}
