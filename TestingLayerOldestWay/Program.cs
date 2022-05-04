using System;
using System.Linq;
using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace TestingLayerOldestWay
{
    class Program
    {
        static LibraryDbContext dbContext;

        static void Main(string[] args)
        {
            try
            {
                SetUpDatabase();

                bool result = TestCreateGenre();

                if (result)
                {
                    Console.WriteLine("Creating of genres is working!");
                }
                else
                {
                    Console.WriteLine("Creating of genres is NOT working!");
                }

                // Candidate for delegates and one method that will use the delegate!


                SetUpDatabase();
                
                bool result1 = TestReadGenre();

                if (result1)
                {
                    Console.WriteLine("Reading of genres is working!");
                }
                else
                {
                    Console.WriteLine("Reading of genres is NOT working!");
                }


                SetUpDatabase();

                bool result2 = TestUpdateGenre();

                if (result2)
                {
                    Console.WriteLine("Updating of genres is working!");
                }
                else
                {
                    Console.WriteLine("Updating of genres is NOT working!");
                }


                SetUpDatabase();
                
                bool result3 = TestDeleteGenre();

                if (result3)
                {
                    Console.WriteLine("Deleting of genres is working!");
                }
                else
                {
                    Console.WriteLine("Deleting of genres is NOT working!");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void SetUpDatabase()
        {
            
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            
            dbContext = new LibraryDbContext(builder.Options);
        }

        static bool TestCreateGenre()
        {
            try
            {
                GenreContext genreContext = new GenreContext(dbContext);

                int genresBefore = genreContext.ReadAll().Count();

                genreContext.Create(new Genre("novel"));

                int genresAfter = genreContext.ReadAll().Count();

                Console.WriteLine("Before: {0} # After: {1}", genresBefore, genresAfter);

                if (genresBefore != genresAfter)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        static bool TestReadGenre()
        {
            try
            {
                GenreContext genreContext = new GenreContext(dbContext);

                genreContext.Create(new Genre("Epica"));

                Genre genre = genreContext.Read(1);

                if (genre != null)
                {
                    Console.WriteLine("Genre Info:{0}ID: {1} # Name {2}", 
                        Environment.NewLine, genre.ID, genre.Name);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static bool TestUpdateGenre()
        {
            GenreContext genreContext = new GenreContext(dbContext);

            Genre genre = new Genre("novel");

            genreContext.Create(genre);

            genre.Name = "poem";

            genreContext.Update(genre);

            Genre genreFromDB = genreContext.Read(1);

            if (genreFromDB.Name != "novel")
            {
                Console.WriteLine("Updated genre name: {0}", genreFromDB.Name);
                return true;
            }

            return false;
        }

        static bool TestDeleteGenre()
        {
            GenreContext genreContext = new GenreContext(dbContext);

            genreContext.Create(new Genre("Delete genre"));

            int genresBeforeDeletion = genreContext.ReadAll().Count();

            genreContext.Delete(1);

            int genresAfterDeletion = genreContext.ReadAll().Count();

            if (genresBeforeDeletion != genresAfterDeletion)
            {
                return true;
            }

            return false;
        }

    }
}
