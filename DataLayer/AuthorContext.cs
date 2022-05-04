using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class AuthorContext : IDB<Author, int>, IDBFiltration<Author>
    {
        private LibraryDbContext _context;

        public AuthorContext(LibraryDbContext context)
        {
            this._context = context;
        }

        public void Create(Author item)
        {
            try
            {
                _context.Authors.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Author Read(int key, bool noTracking = false, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Author> query = _context.Authors;

                if (noTracking)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                if (useNavigationProperties)
                {
                    query = query.Include(a => a.Books).Include(a => a.Genres);
                }

                Author authorFromDB = query.SingleOrDefault(a => a.ID == key);

                if (authorFromDB == null)
                {
                    throw new ArgumentException("There is no author with that key!");
                }

                return authorFromDB;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Author> Read(int skip, int take, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Author> query = _context.Authors.AsNoTrackingWithIdentityResolution();

                if (useNavigationProperties)
                {
                    query = query.Include(a => a.Books).Include(a => a.Genres);
                }

                return query.Skip(skip).Take(take).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Author> ReadAll(bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Author> query = _context.Authors.AsNoTracking();

                if (useNavigationProperties)
                {
                    query = query.Include(a => a.Books).Include(a => a.Genres);
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Author item, bool useNavigationProperties = false)
        {
            try
            {
                Author authorFromDB = Read(item.ID, useNavigationProperties);

                _context.Entry(authorFromDB).CurrentValues.SetValues(item);

                if (useNavigationProperties)
                {
                    List<Book> authorPreviousBooks = authorFromDB.Books.ToList();
                    List<Book> books = new List<Book>(item.Books.Count());

                    foreach (Book book in item.Books)
                    {
                        Book bookFromDB = _context.Books.Include(b => b.Genres).SingleOrDefault(b => b.ISBN == book.ISBN);

                        if (bookFromDB == null)
                        {
                            _context.Entry(book).Collection(b => b.Genres).Load();
                            books.Add(book);
                        }
                        else
                        {
                            //_context.Entry(bookFromDB).CurrentValues.SetValues(book);
                            books.Add(bookFromDB);
                            authorPreviousBooks.Remove(bookFromDB);
                        }
                    }

                    authorFromDB.Books = books;

                    Author unnkownAuthor = Find("Unknown", useNavigationProperties);

                    foreach (Book book in authorPreviousBooks)
                    {
                        book.Author = unnkownAuthor;
                    }

                    // GG, Йоанчо! :)

                    List<Genre> genres = new List<Genre>();

                    foreach (Book book in books)
                    {
                        List<Genre> genresForCurrentBook = book.Genres.ToList();

                        foreach (Genre genre in genresForCurrentBook)
                        {
                            if (!genres.Contains(genre))
                            {
                                genres.Add(genre);
                            }
                        }
                    }

                    authorFromDB.Genres = genres;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int key)
        {
            try
            {
                _context.Authors.Remove(Read(key));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Find author by name
        /// </summary>
        /// <param name="name">Enter name (string)</param>
        /// <returns></returns>
        public Author Find(string name, bool useNavigationProperties = false)
        {
            try
            {
                return _context.Authors.SingleOrDefault(a => a.Name == name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Author> Find(object[] args)
        {
            try
            {
                string country = args[0].ToString();

                return _context.Authors.Where(a => a.Country == country).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Author> GroupBy(object[] args)
        {
            try
            {
                // return IEnumerable<int, Author> (?) for the count and author
                return (IEnumerable<Author>)_context.Authors.GroupBy(a => a.Books.Count(), a => a).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Author> OrderBy(object[] args)
        {
            try
            {
                return _context.Authors.OrderBy(a => a.Name).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
