using static System.Collections.Specialized.BitVector32;

namespace MyMauiApp;

public partial class BookPage : ContentPage
{
    private List<Book> books;
    private Book selectedBook;

    ErrorLogs ErrorLogs = new ErrorLogs();

    public BookPage()
    {
        InitializeComponent();

        InitializeAsync();
    }


    private async void InitializeAsync()
    {
        BookDataManager bookDataManager = new BookDataManager();
        books = await bookDataManager.GetBooks();

        RefreshListView();
    }

    // This method is triggered when the "Insert" button is clicked to add a new book.
    private async void OnInsertClicked(object sender, EventArgs e)
    {
        try
        {
            string title = titleEntry.Text;
            string author = authorEntry.Text;
            DateTime dateofPublishing = publishingdate.Date;

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
            {
                // Generate a new unique Id for the book
                int newId = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;

                // Create a new Book object with the provided details
                Book newBook = new Book { Id = newId, Title = title, Author = author, DateofPublishing = dateofPublishing.Date };

                BookDataManager bookDataManager = new BookDataManager();
                await bookDataManager.AddOrUpdateBook(newBook);
                books.Add(newBook);

                ClearFields();
                RefreshListView();
            }
            else
            {
                await DisplayAlert("Validation Error", "Please fill in all the required fields.", "OK");
            }
        }
        catch (Exception ex)
        {
            ErrorLogs.Add_Error_Log(ex, "BookPage-OnInsertClicked()");
        }
    }

    // This method is triggered when the "Update" button is clicked to update the details of a selected book.
    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        try
        {
            // Check if a book is selected
            if (selectedBook != null)
            {
                string title = titleEntry.Text.Trim();
                string author = authorEntry.Text.Trim();
                DateTime dateofPublishing = publishingdate.Date;


                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
                {
                    // Update the details of the selected book
                    selectedBook.Title = title;
                    selectedBook.Author = author;
                    selectedBook.DateofPublishing = dateofPublishing;
                    // Call the BookDataManager to add or update the book in the data source
                    BookDataManager bookDataManager = new BookDataManager();
                    await bookDataManager.AddOrUpdateBook(selectedBook);

                    // Clear input fields and refresh the list view
                    ClearFields();
                    RefreshListView();
                }
                else
                {
                    await DisplayAlert("Validation Error", "Please fill in all the required fields.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLogs.Add_Error_Log(ex, "BookPage-OnUpdateClicked()");
        }
    }

    // This method is triggered when the "Delete" button is clicked for a specific book item.
    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        try
        {
            // Check if the sender is a Button and the CommandParameter is a Book object
            if (sender is Button deleteButton && deleteButton.CommandParameter is Book bookToDelete)
            {
                bool isConfirmed = await DisplayAlert("Delete Book", $"Are you sure you want to delete '{bookToDelete.Title}'?", "Yes", "No");
                if (isConfirmed)
                {
                    // Call the instance's DeleteBook method to delete the book with the specified Id & Remove the deleted book from the local collection
                    BookDataManager bookDataManager = new BookDataManager();
                    await bookDataManager.DeleteBook(bookToDelete.Id);
                    books.Remove(bookToDelete);

                    ClearFields();
                    RefreshListView();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorLogs.Add_Error_Log(ex, "BookPage-OnDeleteItemClicked()");
        }
    }

    // This method is triggered when a book is selected from the list view.
    private void OnBookSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            selectedBook = e.SelectedItem as Book;
            if (selectedBook != null)
            {
                // Populate the input fields with the details of the selected book
                titleEntry.Text = selectedBook.Title;
                authorEntry.Text = selectedBook.Author;
                publishingdate.Date = selectedBook.DateofPublishing.Date;
            }
        }
        catch (Exception ex)
        {
            ErrorLogs.Add_Error_Log(ex, "BookPage-OnBookSelected()");
        }
    }

    private void RefreshListView()
    {
        try
        {
            bookListView.ItemsSource = null;
            bookListView.ItemsSource = books.OrderBy(b => b.Id);
        }
        catch (Exception ex)
        {
            ErrorLogs.Add_Error_Log(ex, "BookPage-RefreshListView()");
        }
    }

    private void ClearFields()
    {
        titleEntry.Text = string.Empty;
        authorEntry.Text = string.Empty;
        // selectedBook = null;
    }
}