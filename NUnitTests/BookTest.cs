using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUnitTests
{
    public class Tests
    {
        private LibraryDbContext dbContext;
        private BookContext bookContext;
        DbContextOptionsBuilder builder;

        [SetUp]
        public void Setup()
        {
            builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            dbContext = new LibraryDbContext(builder.Options);
            bookContext = new BookContext(dbContext);
        }

        [Test]
        public void TestBookCreate()
        {
            int booksCountBefore = bookContext.ReadAll().Count();
            bookContext.Create(new Book("B000849393", "Qvorov  selection", 200, new Author("Someone", 43, "Bulgaria"), new List<Genre> { new Genre("lirika") }));
            int booksCountAfter = bookContext.ReadAll().Count();
            Assert.IsTrue(booksCountBefore != booksCountAfter);
        }
    }
}