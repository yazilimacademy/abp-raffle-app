using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Results;
using YazilimAcademy.ABPRaffleApp.Domain.Raffles;
using YazilimAcademy.ABPRaffleApp.Permissions;
using Volo.Abp.Application.Dtos;
using YazilimAcademy.ABPRaffleApp.Application.GoogleSheets;

namespace YazilimAcademy.ABPRaffleApp.Raffles
{
    public class RaffleAppService :
        CrudAppService<
            Raffle,
            RaffleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateRaffleDto>,
        IRaffleAppService
    {
        private readonly IRepository<Participant, Guid> _participantRepository;
        private readonly IRepository<RaffleResult, Guid> _raffleResultRepository;
        private readonly GoogleSheetsAppService _googleSheetsAppService;

        public RaffleAppService(
            IRepository<Raffle, Guid> repository,
            IRepository<Participant, Guid> participantRepository,
            IRepository<RaffleResult, Guid> raffleResultRepository,
            GoogleSheetsAppService googleSheetsAppService)
            : base(repository)
        {
            GetPolicyName = ABPRaffleAppPermissions.Raffles.Default;
            GetListPolicyName = ABPRaffleAppPermissions.Raffles.Default;
            CreatePolicyName = ABPRaffleAppPermissions.Raffles.Create;
            UpdatePolicyName = ABPRaffleAppPermissions.Raffles.Edit;
            DeletePolicyName = ABPRaffleAppPermissions.Raffles.Delete;

            _participantRepository = participantRepository;
            _raffleResultRepository = raffleResultRepository;
            _googleSheetsAppService = googleSheetsAppService;
        }

        // Existing code omitted for brevity...

        /// <summary>
        /// Draws winners and backup winners from the given Raffle.
        /// We pick 3 winners and 3 backup by default (you can make them parameters).
        /// Persists the results into RaffleResult, returns them in a DTO.
        /// </summary>
        [UnitOfWork]
        public async Task<DrawResultDto> DrawAsync(Guid raffleId, int winnerCount = 3, int backupCount = 3)
        {
            // 1) Load all participants for this raffle
            var participants = await _participantRepository.GetListAsync(x => x.RaffleId == raffleId);
            if (!participants.Any())
            {
                return new DrawResultDto(new List<ParticipantDto>(), new List<ParticipantDto>());
            }

            // 2) Shuffle participants randomly
            var shuffled = participants
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            // 3) Pick N winners, M backups
            var winners = shuffled.Take(winnerCount).ToList();
            var backups = shuffled.Skip(winnerCount).Take(backupCount).ToList();

            // 4) Create RaffleResults 
            //    (IsWinner = true for the first group, false for the backup group)
            var index = 1;

            var winnerResults = new List<RaffleResult>();

            foreach (var w in winners)
            {
                var resultEntity = new RaffleResult(
                    id: GuidGenerator.Create(),
                    raffleId: raffleId,
                    participantId: w.Id,
                    isWinner: true,
                    order: index
                );
                winnerResults.Add(resultEntity);
                index++;
            }
            await _raffleResultRepository.InsertManyAsync(winnerResults);

            var backupResults = new List<RaffleResult>();
            index = 1;
            foreach (var b in backups)
            {
                var resultEntity = new RaffleResult(
                    id: GuidGenerator.Create(),
                    raffleId: raffleId,
                    participantId: b.Id,
                    isWinner: false, // it's a backup
                    order: index
                );
                backupResults.Add(resultEntity);
                index++;
            }
            await _raffleResultRepository.InsertManyAsync(backupResults);


            // 5) Return a DTO with the selected participants
            //    (We map Participant -> ParticipantDto or something similar)
            var winnerDtos = winners.Select(p => new ParticipantDto
            {
                Id = p.Id,
                FullName = p.FullName.ToString(),
                Email = p.Email
            }).ToList();

            var backupDtos = backups.Select(p => new ParticipantDto
            {
                Id = p.Id,
                FullName = p.FullName.ToString(),
                Email = p.Email
            }).ToList();

            return new DrawResultDto(winnerDtos, backupDtos);
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

        // Return info about the draw


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
}
