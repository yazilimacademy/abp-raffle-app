using Asp.Versioning;
using System;
using System.Threading.Tasks;
using YazilimAcademy.ABPRaffleApp.Books;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace YazilimAcademy.ABPRaffleApp.Controllers.Books
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Book")]
    [Route("api/app/books")]

    public class BookController : AbpController
    {
        protected IBookAppService _booksAppService;

        public BookController(IBookAppService booksAppService)
        {
            _booksAppService = booksAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _booksAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<BookDto> GetAsync(Guid id)
        {
            return _booksAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        {
            return _booksAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
        {
            return _booksAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _booksAppService.DeleteAsync(id);
        }
    }
}