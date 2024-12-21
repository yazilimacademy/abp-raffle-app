using System;
using YazilimAcademy.ABPRaffleApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace YazilimAcademy.ABPRaffleApp.Books;

public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    public BookAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {
        GetPolicyName = ABPRaffleAppPermissions.Books.Default;
        GetListPolicyName = ABPRaffleAppPermissions.Books.Default;
        CreatePolicyName = ABPRaffleAppPermissions.Books.Create;
        UpdatePolicyName = ABPRaffleAppPermissions.Books.Edit;
        DeletePolicyName = ABPRaffleAppPermissions.Books.Delete;
    }
}