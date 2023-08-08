using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MyMauiApp
{
    public class BookDataManager : IBookInterface
    {
        //Define Json File Path
        private static string JsonFilePath => Path.Combine(FileSystem.AppDataDirectory, "book.json");

        // This method Read the content of the JSON file asynchronously
        public async Task<List<Book>> GetBooks()
        {
            if (!File.Exists(JsonFilePath))
                return new List<Book>();

            string json = await File.ReadAllTextAsync(JsonFilePath);
            return JsonConvert.DeserializeObject<List<Book>>(json);
        }


        public async Task<bool> AddOrUpdateBook(Book book)
        {
            List<Book> books = await GetBooks();

            // Check if the book exists based on its Id (assuming Id is unique)
            Book existingBook = books.FirstOrDefault(b => b.Id == book.Id);

            if (existingBook != null)
            {
                // Update existing Data
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.DateofPublishing = book.DateofPublishing.Date;
            }
            else
            {
                // Insert new book
                books.Add(book);
            }

            await SaveBooks(books);

            return true;
        }

        private static async Task SaveBooks(List<Book> books)
        {
            string json = JsonConvert.SerializeObject(books, Formatting.Indented);
            await File.WriteAllTextAsync(JsonFilePath, json);
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            List<Book> books = await GetBooks();
            Book bookToDelete = books.FirstOrDefault(b => b.Id == bookId);

            if (bookToDelete != null)
            {
                books.Remove(bookToDelete);
                await SaveBooks(books);
                return true; // Return true to indicate successful deletion
            }

            return false; // Return false if the book with the given id was not found
        }
    }
}
