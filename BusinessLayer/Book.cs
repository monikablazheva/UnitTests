using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessLayer
{
    public class Book
    {
        [Key]
        public string ISBN { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Range(5, 3000, ErrorMessage = "Pages must be between 5 and 3000!")]
        public int Pages { get; set; }

        [ForeignKey("Author")]
        public int AuthorID { get; set; }

        [Required]
        public Author Author { get; set; }

        [Required]
        public IEnumerable<Genre> Genres { get; set; }

        private Book()
        {

        }

        public Book(string isbn, string title, int pages, Author author, IEnumerable<Genre> genres)
        {
            ISBN = isbn;
            Title = title;
            Pages = pages;
            Author = author;
            Genres = genres;
        }

    }
}
