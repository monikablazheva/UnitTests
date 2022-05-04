using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class BookContext : IDB<Book, string>, IDBFiltration<Book>
    {
        private LibraryDbContext _context;

        public BookContext(LibraryDbContext context)
        {
            _context = context;
        }

        public void Create(Book item)
        {
            try
            {
                Author authorFromDB = _context.Authors.Find(item.AuthorID);

                if (authorFromDB != null)
                {
                    item.Author = authorFromDB;
                }

                List<Genre> genres = new List<Genre>();

                foreach (var genre in item.Genres)
                {
                    Genre genreFromDB = _context.Genres.Find(genre.ID);

                    if (genreFromDB != null)
                    {
                        genres.Add(genreFromDB);
                    }
                    else
                    {
                        genres.Add(genre);
                    }
                }

                item.Genres = genres;

                _context.Books.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Book Read(string key, bool noTracking = false, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Book> query = _context.Books;

                if (noTracking)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.SingleOrDefault(b => b.ISBN == key);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Book> ReadAll(bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Book> query = _context.Books.AsNoTracking();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Book> Read(int skip, int take, bool useNavigationProperties)
        {
            try
            {
                IQueryable<Book> query = _context.Books.AsNoTrackingWithIdentityResolution();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.Skip(skip).Take(take).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Book item, bool useNavigationProperties = false)
        {
            try
            {
                Book bookFromDB = Read(item.ISBN, useNavigationProperties);

                if (useNavigationProperties)
                {
                    bookFromDB.Author = item.Author;

                    List<Genre> genres = new List<Genre>();

                    foreach (Genre genre in item.Genres)
                    {
                        Genre genreFromDB = _context.Genres.Find(genre.ID);

                        if (genreFromDB != null)
                        {
                            genres.Add(genreFromDB);
                        }
                        else
                        {
                            genres.Add(genre);
                        }
                    }

                    bookFromDB.Genres = genres;
                }

                _context.Entry(bookFromDB).CurrentValues.SetValues(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string key)
        {
            try
            {
                _context.Books.Remove(Read(key));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Book Find(string title)
        {
            try
            {
                return _context.Books.FirstOrDefault(b => b.Title == title);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get books by their genre!
        /// </summary>
        /// <param name="args">string => name of genre</param>
        /// <returns></returns>
        public IEnumerable<Book> Find(object[] args)
        {
            try
            {
                string genre = args[0].ToString();

                return _context.Books.Include(b => b.Genres).Where(b => b.Genres.Contains(_context.Genres.Single(g => g.Name == genre))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Not implemented method!
        /// </summary>
        public IEnumerable<Book> GroupBy(object[] args)
        {
            try
            {
                throw new NotImplementedException("There is no sense in returning the genres count for each book!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Order books belonging to specific author
        /// </summary>
        /// <param name="args">author name (string)</param>
        /// <returns></returns>
        public IEnumerable<Book> OrderBy(object[] args)
        {
            try
            {
                string authorName = args[0].ToString();

                return _context.Books.Include(b => b.Author).Where(b => b.Author.Name == authorName).OrderBy(b => b.Title).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
