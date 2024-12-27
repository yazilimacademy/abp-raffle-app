using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace YazilimAcademy.ABPRaffleApp.GoogleDrive;

public interface IGoogleDriveAppService : IApplicationService
{
    Task<List<SpreadsheetDto>> GetAllSpreadsheetsAsync();
}

