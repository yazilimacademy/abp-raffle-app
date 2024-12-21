using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Permissions;

namespace YazilimAcademy.ABPRaffleApp.Raffles;

public class RaffleAppService :
    CrudAppService<
        Raffle, //The Book entity
        RaffleDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateRaffleDto>, //Used to create/update a book
    IRaffleAppService //implement the IBookAppService
{
    public RaffleAppService(IRepository<Raffle, Guid> repository)
        : base(repository)
    {
        GetPolicyName = ABPRaffleAppPermissions.Raffles.Default;
        GetListPolicyName = ABPRaffleAppPermissions.Raffles.Default;
        CreatePolicyName = ABPRaffleAppPermissions.Raffles.Create;
        UpdatePolicyName = ABPRaffleAppPermissions.Raffles.Edit;
        DeletePolicyName = ABPRaffleAppPermissions.Raffles.Delete;
    }

    public async Task<PagedResultDto<RaffleDto>> GetActiveRaffleAsync(PagedAndSortedResultRequestDto input)
    {
        // Apply filtering, sorting, and paging using ABP's repository methods
        var query = await Repository.GetQueryableAsync();
        query = query
            .Where(x => x.IsActive)
            .OrderBy(x => x.CreationTime)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        // Get total count for pagination
        var totalCount = await Repository.CountAsync(x => x.IsActive);

        // Execute query and get results
        var raffles = await AsyncExecuter.ToListAsync(query);

        // Map entities to DTOs
        var raffleDtos = ObjectMapper.Map<List<Raffle>, List<RaffleDto>>(raffles);

        return new PagedResultDto<RaffleDto>(totalCount, raffleDtos);
    }
}