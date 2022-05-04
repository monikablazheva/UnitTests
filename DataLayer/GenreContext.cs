using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class GenreContext : IDB<Genre, int>, IDBFiltration<Genre>
    {
        LibraryDbContext _context;

        public GenreContext(LibraryDbContext context)
        {
            _context = context;
        }

        public void Create(Genre item)
        {
            try
            {
                _context.Genres.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Genre Read(int key, bool noTracking = false, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Genre> query = _context.Genres;

                if (noTracking)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                if (useNavigationProperties)
                {
                    query = query.Include(g => g.Books).Include(g => g.Authors);
                }

                return query.SingleOrDefault(g => g.ID == key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Genre> Read(int skip, int take, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Genre> query = _context.Genres.AsNoTrackingWithIdentityResolution();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Books).Include(b => b.Authors);
                }

                return query.Skip(skip).Take(take).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Genre> ReadAll(bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Genre> query = _context.Genres.AsNoTracking();

                if (useNavigationProperties)
                {
                    query = query.Include(g => g.Books).Include(g => g.Authors);
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="useNavigationProperties"></param>
        public void Update(Genre item, bool useNavigationProperties = false)
        {
            try
            {
                Genre genreFrom = Read(item.ID);

                _context.Entry(genreFrom).CurrentValues.SetValues(item);
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
                _context.Genres.Remove(Read(key));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get genres by their name!
        /// </summary>
        /// <param name="args">string name</param>
        /// <returns></returns>
        public IEnumerable<Genre> Find(object[] args)
        {
            try
            {
                string name = args.ToString();

                return _context.Genres.Where(g => g.Name == name).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get genres by their amount of books!
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<Genre> GroupBy(object[] args)
        {
            try
            {

                return _context.Genres.Include(g => g.Books).OrderByDescending(g => g.Books.Count()).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">true for ascending and false for descending by name</param>
        /// <returns></returns>
        public IEnumerable<Genre> OrderBy(object[] args)
        {
            try
            {
                bool isAscending = bool.Parse(args[0].ToString());

                if (isAscending)
                {
                    return _context.Genres.OrderBy(g => g.Name).ToList();
                }
                else
                {
                    return _context.Genres.OrderByDescending(g => g.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
