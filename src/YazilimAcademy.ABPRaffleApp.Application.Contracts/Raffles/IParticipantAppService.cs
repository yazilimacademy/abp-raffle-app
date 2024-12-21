using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public interface IParticipantAppService :
    ICrudAppService<
        ParticipantDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateParticipantDto>
{
    Task<PagedResultDto<ParticipantDto>> GetParticipantsByRaffleAsync(Guid raffleId, PagedAndSortedResultRequestDto input);
}