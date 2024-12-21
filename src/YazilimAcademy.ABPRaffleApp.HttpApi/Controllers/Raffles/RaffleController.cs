using Asp.Versioning;
using System;
using System.Threading.Tasks;
using YazilimAcademy.ABPRaffleApp.Raffles;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace YazilimAcademy.ABPRaffleApp.Controllers.Raffles
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Raffle")]
    [Route("api/app/raffles")]
    public class RaffleController : AbpController
    {
        protected IRaffleAppService _raffleAppService;

        public RaffleController(IRaffleAppService raffleAppService)
        {
            _raffleAppService = raffleAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<RaffleDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _raffleAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("active")]
        public virtual Task<PagedResultDto<RaffleDto>> GetActiveRaffleAsync(PagedAndSortedResultRequestDto input)
        {
            return _raffleAppService.GetActiveRaffleAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<RaffleDto> GetAsync(Guid id)
        {
            return _raffleAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<RaffleDto> CreateAsync(CreateUpdateRaffleDto input)
        {
            return _raffleAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<RaffleDto> UpdateAsync(Guid id, CreateUpdateRaffleDto input)
        {
            return _raffleAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _raffleAppService.DeleteAsync(id);
        }
    }
}
