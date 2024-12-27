using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using YazilimAcademy.ABPRaffleApp.Application.GoogleSheets;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.GoogleDrive;
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
    private readonly GoogleSheetsAppService _googleSheetsAppService;
    private readonly IRepository<Participant, Guid> _participantRepository;

    public RaffleAppService(IRepository<Raffle, Guid> repository, GoogleSheetsAppService googleSheetsAppService, IRepository<Participant, Guid> participantRepository)
        : base(repository)
    {
        GetPolicyName = ABPRaffleAppPermissions.Raffles.Default;
        GetListPolicyName = ABPRaffleAppPermissions.Raffles.Default;
        CreatePolicyName = ABPRaffleAppPermissions.Raffles.Create;
        UpdatePolicyName = ABPRaffleAppPermissions.Raffles.Edit;
        DeletePolicyName = ABPRaffleAppPermissions.Raffles.Delete;
        _googleSheetsAppService = googleSheetsAppService;
        _participantRepository = participantRepository;
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

    [UnitOfWork]
    public override async Task<RaffleDto> CreateAsync(CreateUpdateRaffleDto input)
    {
        // 1) Domain entity oluştur
        var raffle = new Raffle(
            id: GuidGenerator.Create(),
            name: input.Name
        )
        {
            Description = input.Description,
            StartDate = input.StartDate.UtcDateTime,
            EndDate = input.EndDate.UtcDateTime,
            IsActive = input.IsActive
        };

        // 2) Veritabanına kaydet (Raffle tablosu)
        raffle = await Repository.InsertAsync(raffle);

        // 3) SpreadsheetId -> "Form responses 1!A2:B" gibi bir range ne olabilir?
        // Örneğin, sabit olarak "Form responses 1!A2:B" veya
        // range'i parametre olarak kullanıcıdan alabilirsiniz.
        // Aşağıda örnek: "Form responses 1!B2:C" diyelim (B sütunu: AdSoyad, C sütunu: Email).
        // Lütfen tablo yapınıza göre ayarlayınız!
        var sheetRange = "Form responses 1!B2:C";

        // 4) Sheets'ten participant verilerini çekip kaydet
        // Bu aşamada raffle.Id'yi vererek Participant.RaffleId alanını dolduruyoruz
        var participants = await _googleSheetsAppService.ReadParticipantsFromSheetAsync(
            spreadsheetId: input.SpreadsheetId,
            sheetRange: sheetRange,
            raffleId: raffle.Id
        );

        // UnitOfWork devreye girer, Raffle & Participants DB'ye commit edilir

        await _participantRepository.InsertManyAsync(participants);

        // 5) RaffleDto döndür (UI'ye veya API'ye)
        // Basit bir örnek
        return new RaffleDto
        {
            Id = raffle.Id,
            Name = raffle.Name,
            Description = raffle.Description,
            StartDate = raffle.StartDate,
            EndDate = raffle.EndDate,
            IsActive = raffle.IsActive,
            ParticipantCount = participants.Count
        };
    }
}