using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMauiApp
{
    public interface IBookInterface
    {   
        Task<bool> AddOrUpdateBook(Book book);
        Task<bool> DeleteBook(int id);
        Task<List<Book>> GetBooks();
    }
}
