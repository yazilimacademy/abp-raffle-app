using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using YazilimAcademy.ABPRaffleApp.GoogleDrive;

namespace YazilimAcademy.ABPRaffleApp.Controllers.GoogleDrives
{
    [RemoteService]
    [Area("app")]
    [ControllerName("GoogleDrive")]
    [Route("api/app/google-drives")]
    public class GoogleDriveController : AbpController
    {
        private readonly IGoogleDriveAppService _googleDriveAppService;

        public GoogleDriveController(IGoogleDriveAppService googleDriveAppService)
        {
            _googleDriveAppService = googleDriveAppService;
        }

        [HttpGet]

        public virtual Task<List<SpreadsheetDto>> GetAllSpreadsheetsAsync()
        {
            return _googleDriveAppService.GetAllSpreadsheetsAsync();
        }
    }
}
