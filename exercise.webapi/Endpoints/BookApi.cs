using exercise.webapi.DTOs;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace exercise.webapi.Endpoints
{
    public static class BookApi
    {
        public static void ConfigureBooksApi(this WebApplication app)
        {
            app.MapGet("/books", GetBooks);
            app.MapGet("/books{id}", GetBookById);
            app.MapPut("/books{book_id}", Update);
            app.MapDelete("/books{id}", Delete);
            app.MapPost("/books", CreateBook);
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> CreateBook(IAuthorRepository authorRepository, IBookRepository bookRepository, BookPost model)
        {
            
            Book entity = new Book() { Id = await bookRepository.GetLastId()+1, Title = model.Title, AuthorId = model.AuthorId, Author=await authorRepository.GetAuthorById(model.AuthorId) };

            if (entity.Author is null) return TypedResults.NotFound("No Author with that Id exists.");

            await bookRepository.AddBook(entity);

            return TypedResults.Ok(new BookGet(entity));
            
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> Delete(IBookRepository bookRepository, int id)
        {
            Book entity = await bookRepository.GetBook(id);
            if (entity is null) return TypedResults.NotFound("No book with that Id exists.");
            await bookRepository.DeleteBook(entity);

            return TypedResults.Ok(new BookGet(entity));
        }

        private static async Task<IResult> GetBooks(IBookRepository bookRepository)
        {
            List<BookGet> bookGets = new List<BookGet>();
            var books = await bookRepository.GetAllBooks(); 
            foreach (var b in books)
            {
                bookGets.Add(new BookGet(b.Title, new AuthorNoList(b.Author)));
            }
            return TypedResults.Ok(bookGets);
        }

        private static async Task<IResult> GetBookById(IBookRepository bookRepository, int id)
        {
            var item = await bookRepository.GetBook(id);

            /*
            List<BookGet> bookGets = new List<BookGet>();
            var books = await bookRepository.GetAllBooks();
            */
            BookGet bg = new BookGet(item.Title, new AuthorNoList(item.Author));
            
            return TypedResults.Ok(bg);
        }

        private static async Task<IResult> Update(IBookRepository repository, int book_id, BookPut model)
        {
            var entity = await repository.GetBook(book_id);

            await repository.UpdateBook(entity, model);

            BookGet bg = new BookGet(entity);
            return TypedResults.Ok(bg);
        }




    }
}
