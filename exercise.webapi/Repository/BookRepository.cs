using exercise.webapi.Data;
using exercise.webapi.DTOs;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class BookRepository : IBookRepository
    {
        DataContext _db;
        
        public BookRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId)
        {
            return await _db.Books.Where(b => b.AuthorId == authorId).ToListAsync();
        }

        public async Task<int> GetLastId()
        {
            var books =  _db.Books.OrderByDescending(b => b.Id).ToList();
            return books.FirstOrDefault().Id;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _db.Books.Include(b => b.Author).ToListAsync();

        }
        

        public async Task<Book> GetBook(int id)
        {
            return await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
           

        }

        public async Task<Book> UpdateBook(Book entity, BookPut model)
        {
            if (model.Title != null && model.Title != "") entity.Title = model.Title;
            if (model.AuthorId != null && model.AuthorId != 0) entity.AuthorId = model.AuthorId.Value;
            

            await _db.SaveChangesAsync();
            return await GetBook(entity.Id);
        }

        

        public async Task<Book> DeleteBook(Book book)
        {
            _db.Books.Remove(book);

            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<Book> AddBook(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        }
    }
}
