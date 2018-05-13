using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using ConsoleApplication1.Entities;

namespace ConsoleApplication1.DAO
{
    public class BookDAO
    {
        private MongoClient mongoClient;
        private IMongoCollection<Book> bookCollection;

        public BookDAO()
        {
            mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("books");
            bookCollection = database.GetCollection<Book>("books");
        }

        public void FindAll()
        {
            var books = bookCollection.AsQueryable<Book>().ToList();
            foreach(var book in books)
            {
                Console.WriteLine("id:" + book.id);
                Console.WriteLine("Name:" + book.Name);
                Console.WriteLine("Author:" + book.Author);
                Console.WriteLine("Count:" + book.Count);
                Console.WriteLine("Genre:" + book.Genre);
                Console.WriteLine("Year:" + book.Year);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public void AddBooks()
        {
            List<Book> books = new List<Book> {
                { new Book{
                    Name = "Hobbit",
                    Author = "Tolkien",
                    Count = 5,
                    Genre = "fantasy",
                    Year = 2014
                } },
                { new Book {
                    Name = "Lord of the rings",
                    Author = "Tolkien",
                    Count = 3,
                    Genre = "fantasy",
                    Year = 2015
                } },
                { new Book {
                    Name = "Kolobok",
                    Count = 10,
                    Genre = "kids",
                    Year = 2000
                } },
                { new Book {
                    Name = "Repka",
                    Count = 11,
                    Genre = "kids",
                    Year = 2000
                } },
                { new Book {
                    Name = "Dyadya Stiopa",
                    Author = "Mihalkov",
                    Count = 1,
                    Genre = "kids",
                    Year = 2001
                } }
            };
           
            bookCollection.InsertMany(books);
        }

        public void FindBooksWithCountMoreThanOne()
        {
            var books = bookCollection.AsQueryable<Book>().Where(b => b.Count > 1).OrderBy(b => b.Name).Take(3).Select(b => b.Name).ToList();
            foreach (var book in books)
            {
                Console.WriteLine("Name:" + book);
                Console.WriteLine("---------------------------------------------------------------");
            }
            Console.WriteLine("Count:" + books.Count);
        }

        public void FindBooksWithMaxMinCount()
        {
            var max = bookCollection.AsQueryable<Book>().Max(b => b.Count);
            var min = bookCollection.AsQueryable<Book>().Min(b => b.Count);

            var booksWithMaxCount = bookCollection.AsQueryable<Book>().Where(b => b.Count == max);
            var booksWithMinCount = bookCollection.AsQueryable<Book>().Where(b => b.Count == min);

            Console.WriteLine("Max Count:" + max);
            Console.WriteLine("Books with Max Count:");
            foreach (var book in booksWithMaxCount)
            {
                Console.WriteLine("id:" + book.id);
                Console.WriteLine("Name:" + book.Name);
                Console.WriteLine("Author:" + book.Author);
                Console.WriteLine("Count:" + book.Count);
                Console.WriteLine("Genre:" + book.Genre);
                Console.WriteLine("Year:" + book.Year);
                Console.WriteLine("---------------------------------------------------------------");
            }

            Console.WriteLine("Min Count:" + min);
            Console.WriteLine("Books with Min Count:");
            foreach (var book in booksWithMinCount)
            {
                Console.WriteLine("id:" + book.id);
                Console.WriteLine("Name:" + book.Name);
                Console.WriteLine("Author:" + book.Author);
                Console.WriteLine("Count:" + book.Count);
                Console.WriteLine("Genre:" + book.Genre);
                Console.WriteLine("Year:" + book.Year);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public void FindAllAuthors()
        {
            var authors = bookCollection.AsQueryable<Book>().Where(b => !string.IsNullOrEmpty(b.Author)).Select(b => b.Author).Distinct();

            Console.WriteLine("List of all authors:");
            foreach (var author in authors)
            {
                Console.WriteLine("Name:" + author);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public void GetBooksWithoutAuthors()
        {
            var books = bookCollection.AsQueryable<Book>().Where(b => string.IsNullOrEmpty(b.Author));

            Console.WriteLine("List of books without author:");
            foreach (var book in books)
            {
                Console.WriteLine("id:" + book.id);
                Console.WriteLine("Name:" + book.Name);
                Console.WriteLine("Author:" + book.Author);
                Console.WriteLine("Count:" + book.Count);
                Console.WriteLine("Genre:" + book.Genre);
                Console.WriteLine("Year:" + book.Year);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public void IncreaseCountOfEachBookByOne()
        {
            bookCollection.UpdateMany<Book>(Builders<Book>.Filter.BitsAllSet, Builders<Book>.Update.CurrentDate(b => b.Count + 1));
        }
    }
}
