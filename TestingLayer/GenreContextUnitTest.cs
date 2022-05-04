using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace TestingLayer
{
    public class GenreContextUnitTest
    {
        private LibraryDbContext dbContext;
        private GenreContext genreContext;
        DbContextOptionsBuilder builder;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // TODO: Add code here that is run before
            //  all tests in the assembly are run
            
        }

        [SetUp]
        public void Setup()
        {
            builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            dbContext = new LibraryDbContext(builder.Options);
            genreContext = new GenreContext(dbContext);
        }

        [Test]
        public void TestCreateGenre()
        {
            int genresBefore = genreContext.ReadAll().Count();

            genreContext.Create(new Genre("novel"));

            int genresAfter = genreContext.ReadAll().Count();

            Assert.IsTrue(genresBefore != genresAfter);
        }

        [Test]
        public void TestReadGenre()
        {
            genreContext.Create(new Genre("novel"));

            Genre genre = genreContext.Read(1);

            Assert.That(genre != null, "There is no record with id 1!");
        }

        [Test]
        public void TestUpdateGenre()
        {
            genreContext.Create(new Genre("novel"));

            Genre genre = genreContext.Read(1);

            genre.Name = "poem";

            genreContext.Update(genre);

            Genre genre1 = genreContext.Read(1);

            Assert.IsTrue(genre1.Name == "poem", "Genre Update() does not change name!");
        }

        [Test]
        public void TestDeleteGenre()
        {
            genreContext.Create(new Genre("Delete genre"));

            int genresBeforeDeletion = genreContext.ReadAll().Count();

            genreContext.Delete(1);

            int genresAfterDeletion = genreContext.ReadAll().Count();

            Assert.AreNotEqual(genresBeforeDeletion, genresAfterDeletion);
        }

    }
}