using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.Abp.Guids;
using YazilimAcademy.ABPRaffleApp.Domain.Participants;
using YazilimAcademy.ABPRaffleApp.Domain.Shared;

namespace YazilimAcademy.ABPRaffleApp.Application.GoogleSheets
{
    // ITransientDependency => Her istekte yeni instance
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

        /// <summary>
        /// Tıpkı GoogleDriveAppService gibi, JSON dosyasından credential alıp
        /// Google Sheets servisini hazırlar.
        /// </summary>
        private SheetsService CreateSheetsServiceFromJson()
        {
            // Service Account veya OAuth credential JSON dosyanızın konumu
            var credentialPath = "C:\\Users\\alper\\Desktop\\orbital-lantern-443920-g2-0c500a9e7d1e.json";

            // GoogleCredential oluştur
            var credential = GoogleCredential.FromFile(credentialPath)
                                             .CreateScoped(SheetsService.Scope.Spreadsheets);

            // SheetsService örneği
            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "ABPRaffleApp"
            });

            return sheetsService;
        }

        /// <summary>
        /// Spreadsheet ID verilince, içindeki tüm sheet isimlerini döndürür.
        /// Ör: Form responses 1, Sheet1 vs...
        /// </summary>
        public async Task<List<string>> GetSheetTitlesAsync(string spreadsheetId)
        {
            var service = CreateSheetsServiceFromJson();

            var spreadsheet = await service.Spreadsheets.Get(spreadsheetId).ExecuteAsync();
            if (spreadsheet.Sheets == null)
            {
                return new List<string>();
            }

            var sheetTitles = spreadsheet.Sheets
                .Where(s => s.Properties?.Title != null)
                .Select(s => s.Properties.Title)
                .ToList();

            return sheetTitles;
        }

        /// <summary>
        /// Belirli bir sheetRange (örn "Form responses 1!A2:B") üzerinden
        /// FullName & Email kolonlarını okuyup Participant oluşturur.
        /// </summary>
        [UnitOfWork]
        public async Task<List<Participant>> ReadParticipantsFromSheetAsync(
            string spreadsheetId,
            string sheetRange, // "Form responses 1!A2:B"
            Guid raffleId
        )
        {
            var service = CreateSheetsServiceFromJson();

            // Google Sheets verilerini al
            var request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange);
            var response = await request.ExecuteAsync();
            var values = response.Values;

            if (values == null || values.Count == 0)
            {
                return new List<Participant>();
            }

            var participants = new List<Participant>();

            foreach (var row in values)
            {
                // row[0] => FullName, row[1] => Email 
                // Bu endeksleri tablo yapınıza göre uyarlayın
                var fullNameString = row.Count > 0 ? row[0]?.ToString()?.Trim() : null;
                var emailString = row.Count > 1 ? row[1]?.ToString()?.Trim()
                 : null;

                if (string.IsNullOrWhiteSpace(fullNameString) || string.IsNullOrWhiteSpace(emailString))
                {
                    continue; // geçerli veri yok
                }

                var fullName = FullName.Create(fullNameString);
                var email = new Email(emailString);

                var participant = new Participant(
                    id: _guidGenerator.Create(),
                    raffleId: raffleId,
                    fullName: fullName,
                    email: email
                );

                await _participantRepository.InsertAsync(participant);
                participants.Add(participant);
            }

            // UnitOfWork => transaction commit
            return participants.DistinctBy(p => p.Email.Value).ToList();
        }
    }
}
