using exercise.webapi.DTOs;
using exercise.webapi.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.Models
{
    public class Book
    {
        private static int _lastId = 0;

        public int Id { get; set; }
        public string Title { get; set; }
        
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        
        /*
        public Book()
        {
            Id = ++_lastId;
        }
        */
        
    }
}
