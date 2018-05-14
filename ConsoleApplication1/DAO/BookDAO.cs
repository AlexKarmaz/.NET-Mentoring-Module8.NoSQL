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
            if(books.Count == 0)
            {
                Console.WriteLine("Book collection is empty");
            }
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

        //Task1
        //Добавьте следующие книги(название, автор, количество экземпляров, жанр, год издания):
        //Hobbit, Tolkien, 5, fantasy, 2014
        //Lord of the rings, Tolkien, 3, fantasy, 2015
        //Kolobok, 10, kids, 2000
        //Repka, 11, kids, 2000
        //Dyadya Stiopa, Mihalkov, 1, kids, 2001
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

        //Task2
        //Найдите книги с количеством экземпляров больше единицы.
        //a.Покажите в результате только название книги.
        //b.Отсортируйте книги по названию.
        //c.Ограничьте количество возвращаемых книг тремя.
        //d.Подсчитайте количество таких книг.
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

        //Task3
        //Найдите книгу с макимальным/минимальным количеством (count).
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

        //Task4
        //Найдите список авторов (каждый автор должен быть в списке один раз).
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

        //Task5
        //Выберите книги без авторов.
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

        //Task6
        //Увеличьте количество экземпляров каждой книги на единицу.
        public void IncreaseCountOfEachBookByOne()
        {
            bookCollection.UpdateMany(Builders<Book>.Filter.Empty, Builders<Book>.Update.Inc(b => b.Count, 1));
        }

        //Task7
        //Добавьте дополнительный жанр “favority” всем книгам с жанром “fantasy” 
        //(последующие запуски запроса не должны дублировать жанр “favority”).
        public void AddGenreFavorityToFantasyBook()
        {
            bookCollection.UpdateMany(Builders<Book>.Filter.Eq<string>("Genre", "fantasy") & !Builders<Book>.Filter.Eq<string>("Genre", "favority"), Builders<Book>.Update.Set("Genre", "fantasy, favority"));
        }

        //Task8
        //Удалите книги с количеством экземпляров меньше трех.
        public void DeleteBooksWithCountLessThanThree()
        {
            var result = bookCollection.DeleteMany<Book>(b => b.Count < 3);
            Console.WriteLine(result.DeletedCount + "books have been deleted");
        }

        //Task9
        //Удалите все книги.
        public void DeleteAllBooks()
        {
            bookCollection.Database.DropCollection("books");
            Console.WriteLine("All books have been deleted");
        }
    }
}
