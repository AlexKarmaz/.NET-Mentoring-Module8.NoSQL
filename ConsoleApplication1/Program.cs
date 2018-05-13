using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using ConsoleApplication1.DAO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            BookDAO bookDAO = new BookDAO();

            //Task1
            //bookDAO.AddBooks();
            //bookDAO.FindAll();

            //Task2
            //bookDAO.FindBooksWithCountMoreThanOne();

            //Task3
            //bookDAO.FindBooksWithMaxMinCount();

            //Task4
            //bookDAO.FindAllAuthors();

            //Task5
            //bookDAO.GetBooksWithoutAuthors();

            //Task6
            bookDAO.IncreaseCountOfEachBookByOne();
            bookDAO.FindAll();
            Console.ReadKey();
        }
    }
}
