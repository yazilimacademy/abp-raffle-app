using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using YazilimAcademy.ABPRaffleApp.Domain.Shared; // FullName, Email
using YazilimAcademy.ABPRaffleApp.Domain.Participants; // Participant
using Volo.Abp.Guids;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;

namespace YazilimAcademy.ABPRaffleApp.Application.GoogleSheets
{
    public class GoogleSheetsAppService : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Participant, Guid> _participantRepository;

        public GoogleSheetsAppService(
            IGuidGenerator guidGenerator,
            IRepository<Participant, Guid> participantRepository)
        {
            _guidGenerator = guidGenerator;
            _participantRepository = participantRepository;
        }

        private SheetsService CreateSheetsServiceWithApiKey(string apiKey)
        {
            // API Key ile SheetsService
            return new SheetsService(new BaseClientService.Initializer
            {
                ApiKey = apiKey,
                ApplicationName = "ABPRaffleApp"
            });
        }

        /// <summary>
        /// Google Sheets içindeki mevcut sheet (sayfa) listesini döndürür.
        /// Spreadsheet'in ID'si ve API key'i parametre olarak alıyor.
        /// </summary>
        public async Task<List<string>> GetSheetTitlesAsync(string spreadsheetId, string apiKey)
        {
            var service = CreateSheetsServiceWithApiKey(apiKey);

            // Spreadsheet metadata’sini çekiyoruz
            var spreadsheet = await service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();

            // Tüm sheet'lerin Title bilgilerini alıyoruz
            var sheetTitles = spreadsheet.Sheets
                .Select(s => s.Properties.Title)
                .Where(title => !string.IsNullOrWhiteSpace(title))
                .ToList();

            return sheetTitles;
        }

        /// <summary>
        /// Seçili sheet'ten (ör. "Sheet1!A2:B") verileri çekip,
        /// FullName ve Email'i alarak Participant'lara dönüştürür.
        /// </summary>
        [UnitOfWork]
        public async Task<List<Participant>> ReadParticipantsFromSheetAsync(
            string spreadsheetId,
            string sheetName,
            string range,       // A2:B
            Guid raffleId,
            string apiKey
        )
        {
            var service = CreateSheetsServiceWithApiKey(apiKey);

            // Google Sheets aralığı: "SheetName!A2:B"
            var fullRange = $"{sheetName}!{range}";

            var request = service.Spreadsheets.Values.Get(spreadsheetId, fullRange);
            var response = await request.ExecuteAsync();
            var values = response.Values;

            if (values == null || values.Count == 0)
            {
                // Boş veya veri yok
                return new List<Participant>();
            }

            // Her satır: [ FullName, Email ] gibi
            var participants = new List<Participant>();
            foreach (var row in values)
            {
                var fullNameString = row.Count > 0 ? row[0]?.ToString() : "";
                var emailString = row.Count > 1 ? row[1]?.ToString() : "";

                if (string.IsNullOrWhiteSpace(fullNameString) || string.IsNullOrWhiteSpace(emailString))
                {
                    // Eğer geçerli veri yoksa atla
                    continue;
                }

                var fullName = FullName.Create(fullNameString);
                var email = new Email(emailString);

                var participant = new Participant(
                    id: _guidGenerator.Create(),
                    raffleId: raffleId,
                    fullName: fullName,
                    email: email
                );

                // DB'ye ekle
                await _participantRepository.InsertAsync(participant);
                participants.Add(participant);
            }

            // UnitOfWork nedeniyle, metot bitince commit edilecektir
            return participants;
        }
    }
}
