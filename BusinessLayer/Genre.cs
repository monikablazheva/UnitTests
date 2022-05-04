using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer
{
    public class Genre
    {
        [Key]
        public int ID { get; private set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        private Genre()
        {

        }

        public Genre(string name)
        {
            Name = name;
        }
    }
}