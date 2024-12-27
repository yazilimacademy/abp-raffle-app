using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using File = Google.Apis.Drive.v3.Data.File;
using Volo.Abp.DependencyInjection;
using YazilimAcademy.ABPRaffleApp.GoogleDrive;

namespace YazilimAcademy.ABPRaffleApp.Application.GoogleDrives;

public class GoogleDriveAppService : IGoogleDriveAppService, ISingletonDependency
{
    /// <summary>
    /// Drive API servisini oluşturur.
    /// Service Account veya "client_secret.json" OAuth kimlik bilgilerini kullanabilirsiniz.
    /// Burada örnek olarak dosyadan credential okuyoruz.
    /// </summary>
    private DriveService CreateDriveService()
    {
        // 1) Credential yükle (service account veya user creds)
        // client_secret.json dosyasının konumu proje yapılandırmanıza göre değişebilir.
        var credential = GoogleCredential
            .FromFile("C:\\Users\\alper\\Desktop\\orbital-lantern-443920-g2-0c500a9e7d1e.json")
            .CreateScoped(DriveService.Scope.DriveReadonly);

        // 2) DriveService örneği oluştur
        var driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "ABPRaffleApp"
        });

        return driveService;
    }

    /// <summary>
    /// Kullanıcının/sunucu hesabının Drive'ında bulunan
    /// tüm Google Spreadsheet dosyalarını (ID & Ad) listeleyen metot.
    /// </summary>
    public async Task<List<SpreadsheetDto>> GetAllSpreadsheetsAsync()
    {
        // Drive servisini oluştur
        var service = CreateDriveService();

        // Drive API "Files.List()" oluştur
        var request = service.Files.List();

        // Sadece Google Spreadsheet (E-Tablolar) dosyalarını getir
        request.Q = "mimeType = 'application/vnd.google-apps.spreadsheet'";

        // Sonuçlarda hangi alanları almak istediğimizi belirtelim
        request.Fields = "files(id,name)";

        // İsteği çalıştır
        var result = await request.ExecuteAsync();

        // files içinde ID ve Name değerlerini tutan bir liste döner
        var files = result.Files;
        if (files == null)
        {
            files = new List<File>();
        }

        // DTO (Data Transfer Object) oluşturup return edelim
        var spreadsheetList = files
            .Select(file => new SpreadsheetDto(file.Id, file.Name))
            .ToList();

        return spreadsheetList;
    }
}