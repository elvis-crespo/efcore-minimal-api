using EFC_MinimalApis.Models;

namespace EFC_MinimalApis.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CrearLibro(BookRequest request);
    }
}
