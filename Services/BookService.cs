using EFC_MinimalApis.Context;
using EFC_MinimalApis.Models;
using EFC_MinimalApis.Services.Interfaces;

namespace EFC_MinimalApis.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationContext _context;

        public BookService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Book> CrearLibro(BookRequest request)
        {
            var book = new Book
            {
                Name = request.Name ?? string.Empty,
                ISBN = request.Isbn ?? string.Empty
            };

            var createBook = await _context.BookEntity.AddAsync(book);

            await _context.SaveChangesAsync();

            return createBook.Entity;
        }
    }
}
