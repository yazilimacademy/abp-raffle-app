using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public interface IRaffleAppService :
    ICrudAppService< //Defines CRUD methods
        RaffleDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateRaffleDto> //Used to create/update a book
{
    Task<PagedResultDto<RaffleDto>> GetActiveRaffleAsync(PagedAndSortedResultRequestDto input);
}
