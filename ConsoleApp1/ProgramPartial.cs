using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Resource;
using HomeWork6.Helpers;
using HomeWork6.Migrations;
using HomeWork6.Models;
using HomeWork6.ViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6
{
    public partial class Program
    {
        //Authors
        static async Task ReviewAuthors()
        {
            var allAuthors = await _authors.GetAllAuthorsAsync();
            var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, authors, true);
            if (result != 0)
            {
                var currentAuthor = await _authors.GetAuthorAsync(result);
                await AuthorInfo(currentAuthor);
            }
        }
        static async Task AuthorInfo(Author currentAuthor)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
                {
                 new ItemView { Id = 1, Value = "Browse books"},
                 new ItemView { Id = 2, Value = "Edit author"},
                 new ItemView { Id = 3, Value = "Delete author"},
                },
                IsMenu: true, message: String.Format("{0}\n", currentAuthor), startY: 5, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        await BrowseAuthorBooks(currentAuthor);
                        break;
                    }
                case 2:
                    {
                        await EditAuthor(currentAuthor);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveAuthor(currentAuthor);
                        Console.ReadLine();
                        break;
                    }
            }
            await ReviewAuthors();
        }
        static async Task AddAuthor()
        {
            string authorName = InputHelper.GetString("author 'Name' with 'Surname'");
            await _authors.AddAuthorAsync(new Author
            {
                Name = authorName
            });
            Console.WriteLine("Author successfully added.");
        }
        static async Task EditAuthor(Author currentAuthor)
        {
            Console.WriteLine("Changing: {0}", currentAuthor.Name);
            currentAuthor.Name = InputHelper.GetString("author 'Name' with 'SurName'");
            await _authors.UpdateAuthorAsync(currentAuthor);
            Console.WriteLine("Author successfully changed.");
        }
        static async Task RemoveAuthor(Author currentAuthor)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
                {
                 new ItemView { Id = 1, Value = "Yes"},
                 new ItemView { Id = 0, Value = "No"},
                }, message: String.Format("[Are you sure you want to delete the author {0} ?]\n", currentAuthor.Name), startY: 2);

            if (result == 1)
            {
                await _authors.DeleteAuthorAsync(currentAuthor);
                Console.WriteLine("The author has been successfully deleted.");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }
        static async Task SearchAuthors()
        {
            string authorName = InputHelper.GetString("author name or surname");
            var currentAuthors = await _authors.GetAuthorsByNameAsync(authorName);
            if (currentAuthors.Count() > 0)
            {
                var authors = currentAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
                int result = ItemsHelper.MultipleChoice(true, authors, true);
                if (result != 0)
                {
                    var currentAuthor = await _authors.GetAuthorAsync(result);
                    await AuthorInfo(currentAuthor);
                }
            }
            else
            {
                Console.WriteLine("No authors were found by this attribute.");
            }
        }

        static async Task BrowseAuthorBooks(Author currentAuthor)
        {
            var authorsBooks = await _authors.GetAuthorWithBookAsync(currentAuthor.Id);

            if (authorsBooks != null)
            {
                if (authorsBooks.Books.Count() > 0)
                {
                    await ReviewBooks(authorsBooks.Books);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("no books were found");
                }
            }
            else
            {
                Console.WriteLine("author not found");
            }
        }


        //Categories
        static async Task ReviewCategories()
        {
            var allCategories = await _categories.GetAllCategoriesAsync();
            var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, categories, true);

            if (result != 0)
            {
                var currentCategory = await _categories.GetCategoryAsync(result);
                await CategoryInfo(currentCategory);
            }
        }

        static async Task CategoryInfo(Category currentCategory)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Browse books" },
                new ItemView { Id = 2, Value = "Edit category" },
                new ItemView { Id = 3, Value = "Remove category" }
            },
            IsMenu: true, message: String.Format("{0}\n", currentCategory), startY: 5, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        await BrowseCategoryBooks(currentCategory);
                        break;
                    }
                case 2:
                    {
                        await EditCategory(currentCategory);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveCategory(currentCategory);
                        Console.ReadLine();
                        break;
                    }

            }
            await ReviewCategories();
        }

        static async Task AddCategory()
        {
            string categoryName = InputHelper.GetString("category 'Name'");
            string categoryDescription = InputHelper.GetString("category 'Description'");
            await _categories.AddCategoryAsync(new Category
            {
                Name = categoryName,
                Description = categoryDescription
            });
            Console.WriteLine("Category added");
        }

        static async Task EditCategory(Category currentCategory)
        {
            Console.WriteLine("Change: {0}", currentCategory.Name);
            currentCategory.Name = InputHelper.GetString("category name");
            currentCategory.Description = InputHelper.GetString("category description");
            await _categories.UpdateCategoryAsync(currentCategory);
            Console.WriteLine("Category changed\r\n");
        }

        static async Task RemoveCategory(Category currentCategory)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView{ Id = 1, Value = "Yes"},
                new ItemView{ Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to remove this category: {0}]\n", currentCategory.Name), startY: 2);

            if (result == 1)
            {
                await _categories.DeleteCategoryAsync(currentCategory);
                Console.WriteLine("Category has been deleted");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }
        static async Task SearchCategories()
        {
            string categoryName = InputHelper.GetString("category name");
            var currentCategories = await _categories.GetCategoriesByNameAsync(categoryName);
            if (currentCategories.Count() > 0)
            {
                var categories = currentCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
                int result = ItemsHelper.MultipleChoice(true, categories, true);

                if (result != 0)
                {
                    var currentCategory = await _categories.GetCategoryAsync(result);
                    await CategoryInfo(currentCategory);
                }
                else { Console.WriteLine("No categories were found"); }
            }
        }
        static async Task BrowseCategoryBooks(Category currentCategory)
        {
            var categoryWithBooks = await _categories.GetCategoryWithBooksAsync(currentCategory.Id);
            if (categoryWithBooks != null)
            {
                if (categoryWithBooks.Books.Count() > 0)
                {
                    await ReviewBooks(categoryWithBooks.Books);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("No books were found");
                }
            }
            else
            {
                Console.WriteLine("Category not found");
            }
        }

        //Books
        static async Task ReviewBooks(ICollection<Book> authorBooks = null)
        {
            var allBooks = authorBooks == null ? await _books.GetAllBooksAsync() : authorBooks;
            var books = allBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
            int result = ItemsHelper.MultipleChoice(true, books, true);

            if (result != 0)
            {
                var currentBook = await _books.GetBookWithCategoryAndAuthorsAsync(result);
                await BookInfo(currentBook);
            }
        }
        static async Task BookInfo(Book currentBook)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView{ Id = 1, Value = "1. View authors"},
                new ItemView{ Id = 2, Value = "2. Edit book"},
                new ItemView{ Id = 3, Value = "3. Delete book"},
                new ItemView{ Id = 4, Value = "4. Promotion Info"},
            },
            IsMenu: true, message: string.Format("[{0}]\n", currentBook), startY: 10, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        await BrowseAuthors(currentBook);
                        break;
                    }
                case 2:
                    {
                        await EditBook(currentBook);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveBook(currentBook);
                        Console.ReadLine();
                        break;
                    }
                case 4:
                    {
                        await PromotionInfo(currentBook);
                        Console.ReadLine();
                        break;
                    }
            }
            await ReviewBooks();
        }
        static async Task BrowseAuthors(Book currentBook)
        {
            var authors = currentBook.Authors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();

            if (authors.Count > 0)
            {
                int result = ItemsHelper.MultipleChoice(true, authors, true);

                if (result != 0)
                {
                    var currentAuthor = await _authors.GetAuthorAsync(result);
                    await AuthorInfo(currentAuthor);
                }
            }
            else
            {
                Console.WriteLine("Book does not contain any authors");
                Console.ReadLine();
            }
        }
        private static async Task SearchBooks()
        {
            string bookTitle = InputHelper.GetString("book title");
            var currentBook = await _books.GetBooksByNameAsync(bookTitle);

            if (currentBook.Count() < 0)
            {
                var books = currentBook.Select(e => new ItemView
                {
                    Id = e.Id,
                    Value = e.Title
                }).ToList();

                var result = ItemsHelper.MultipleChoice(true, books, true);
                if (result != 0)
                {
                    await BookInfo(await _books.GetBookAsync(result));
                }
                else { Console.WriteLine("No books where found"); }
            }
        }

        static async Task RemoveBook(Book currentBook)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Yes"},
                new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to remove this book {0} ?]", currentBook.Title), startY: 2);

            if (result == 1)
            {
                await _books.DeleteBookAsync(currentBook);
                Console.WriteLine("Book removed");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }

        static async Task AddBook()
        {
            await _books.AddBookAsync(await CreateOrUpdateBook());
            Console.WriteLine("Book was added");
        }
        static async Task EditBook(Book currentBook)
        {
            await _books.EditBookAsync(await CreateOrUpdateBook(currentBook));
            Console.WriteLine("Book was added");
        }

        static async Task<Book> CreateOrUpdateBook(Book? currentBook = null)
        {
            if (currentBook != null)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ResetColor();
            }

            var allCategories = await _categories.GetAllCategoriesAsync();
            var allAuthors = await _authors.GetAllAuthorsAsync();

            string bookTitle = InputHelper.GetString("Title ");
            string bookDescriprtion = InputHelper.GetString("Descriprtion ");
            DateOnly bookPublished = InputHelper.GetDate("Published ");
            string bookPublisher = InputHelper.GetString("Publisher ");
            decimal bookPrice = InputHelper.GetDecimal("Price ");

            string buffer = String.Format("Enter book 'Title': {0}\nEnter book 'Description': {1}\nEnter book 'Published on': {2}\n" +
                "Enter book 'Publisher': {3}\nEnter book 'Price': {3}\nChoose 'Category':", bookTitle, bookDescriprtion, bookPublished,
                bookPublisher, bookPrice);

            var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int categoryId = ItemsHelper.MultipleChoice(true, categories, message: buffer, startY: 10);

            buffer += String.Format("Category with Id: {0}\nChose author", categoryId);
            var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            List<Author> selecterdAuthors = new List<Author>(1);
            int authorId = -1;

            while (authorId != 0)
            {
                selecterdAuthors.Add(new Author { Id = authorId });
                authors = authors.Where(e => e.Id != authorId).ToList();
            }

            return new Book
            {
                Id = currentBook?.Id ?? 0,
                Title = bookTitle,
                Description = bookDescriprtion,
                PublishedOn = bookPublished.ToDateTime(TimeOnly.Parse("0:0")),
                Publisher = bookPublisher,
                CategoryId = categoryId,
                Authors = selecterdAuthors
            };
        }
        //Promotions
        static async Task PromotionInfo(Book currentBook)
        {
            var bookWithPromotion = await _books.GetBookWithPromotionAsync(currentBook.Id);
            var menuList = new List<ItemView>(1);

            if (bookWithPromotion.Promotion != null)
            {
                menuList.Add(new ItemView { Id = 1, Value = "Add promotion" });
            }
            else
            {
                menuList.Add(new ItemView { Id = 2, Value = "Edit promotion" });
                menuList.Add(new ItemView { Id = 3, Value = "Remove promotion" });
            }

            int result = ItemsHelper.MultipleChoice(true, menuList,
               IsMenu: true, String.Format("{0}\n\nPromotion: {1}", currentBook, bookWithPromotion.Promotion), startY: 10, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        await AddPromotion(currentBook);
                        Console.ReadLine();
                        break;
                    }
                case 2:
                    {
                        await EditPromotion(bookWithPromotion);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemovePromotion(bookWithPromotion.Promotion!);
                        Console.ReadLine();
                        break;
                    }
            }

            await BookInfo(currentBook);
        }

        static async Task CreateOrUpdatePromotion(Book currentBook)
        {
            bool isNew = currentBook.Promotion == null;
            string promotionTitle = InputHelper.GetString("promotion 'Title'");

            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "by percent" },
                new ItemView { Id = 2, Value = "by amount"}
            }, true, message: "choose discount format", startY: 10, optionsPerLine: 1);

            decimal value = InputHelper.GetDecimal(result == 1 ? "by percent" : "by amount");
            Promotion promotion = new Promotion
            {
                Id = currentBook?.Promotion?.Id ?? 0,
                Name = promotionTitle,
                BookId = currentBook.Id,
            };
            if (result == 1)
            {
                promotion.Percent = value;
            }
            else
            {
                promotion.Amount = value;
            }
            if (isNew)
            {
                await _promotions.AddPromotionAsync(promotion);
            }
            else
            {
                await _promotions.DeletePromotionAsync(promotion);
            }
        }

        private static async Task RemovePromotion(Promotion currentPromotion)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value ="Yes" },
                new ItemView { Id = 2, Value ="No" }
            }, message: String.Format("[Are you sure you want to remove this promotion {0}?]\n", currentPromotion), startY: 2);

            if (result == 1)
            {
                await _promotions.DeletePromotionAsync(currentPromotion);
                Console.WriteLine("Promotion removed");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }

        static async Task EditPromotion(Book bookWithPromotion)
        {
            await CreateOrUpdatePromotion(bookWithPromotion);
            Console.WriteLine("Promotion changed");
        }

        static async Task AddPromotion(Book currentBook)
        {
            await CreateOrUpdatePromotion(currentBook);
            Console.WriteLine("Promotion added");
        }

        //Orders
        static async Task ReviewOrders()
        {
            var allOrders = await _orders.GetAllOrdersAsync();
            var orders = allOrders.Select(e => new ItemView
            {
                Id = e.Id,
                Value = String.Format("{0} ({1}, {2})", e.CustomerName, e.City, e.Address)
            }).ToList();

            int result = ItemsHelper.MultipleChoice(true, orders, true, optionsPerLine: 1);

            if (result != 0)
            {
                var currentOrder = await _orders.GetOrderWithOrderLinesAndBooksAsync(result);
                await OrderInfo(currentOrder);
            }
        }

        static async Task OrderInfo(Order currentOrder)
        {
            List<ItemView> items = new List<ItemView>
            {
                new ItemView {Id = 1, Value = "Browse books"},
                new ItemView {Id = 1, Value = "Edit order"},
                new ItemView {Id = 1, Value = "Delete order"}
            };

            if (currentOrder.Shipped == false)
            {
                items.Add(new ItemView { Id = 4, Value = "Close order" });
            }

            decimal totalSum = currentOrder.Lines.Sum(e => e.Book.Price);
            decimal totalSumWithDiscount = totalSum;

            foreach (var book in currentOrder.Lines.Select(e => e.Book))
            {
                if (book.Promotion != null)
                {
                    if (book.Promotion.Percent != null)
                    {
                        totalSumWithDiscount -= book.Price * book.Promotion.Percent.Value / 100;
                    }
                    else
                    {
                        totalSumWithDiscount -= book.Promotion.Amount ?? 0;
                    }
                }
            }

            int result = ItemsHelper.MultipleChoice(true, items,
                IsMenu: true, message: String.Format("{0}\n\nTotal cost: {1}\nWith discount: {2}",
                currentOrder, totalSum, totalSumWithDiscount), startY: 8, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        await BrowseBooks(currentOrder);
                        break;
                    }
                case 2:
                    {
                        await EditOrder(currentOrder);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveOrder(currentOrder);
                        Console.ReadLine();
                        break;
                    }
                case 4:
                    {
                        await CloseOrder(currentOrder);
                        Console.ReadLine();
                        break;
                    }
            }
            await ReviewOrders();
        }
        static async Task SearchOrders()
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView {Id = 1, Value = "Customer 'Adress'"},
                new ItemView {Id = 2, Value = "Customer 'Name'"},
            }, message: String.Format("Select data from where to start search?"), startY: 2, optionsPerLine: 1, IsMenu: true);

            if (result == 1)
            {
                string adress = InputHelper.GetString("Customer 'Adress'");
                var currentOrders = await _orders.GetAllOrdersByAddressAsync(adress);

                if(currentOrders.Count() > 0)
                {
                    await Search(currentOrders);
                }
                else
                {
                    Console.WriteLine("Nothing was found");
                }
            }
            else if(result == 2)
            {
                string name = InputHelper.GetString("Customer 'Name'");
                var currentOrders = await _orders.GetAllOrdersByAddressAsync(name);

                if (currentOrders.Count() > 0)
                {
                    await Search(currentOrders);
                }
                else
                {
                    Console.WriteLine("Nothing was found");
                }
            }
        }

        static async Task CloseOrder(Order currentOrder)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Yes"},
                new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Close this order {0} ?]", currentOrder.CustomerName), startY: 2);

            if (result == 1)
            {
                currentOrder.Shipped = true;
                await _orders.UpdateOrderAsync(currentOrder);
                Console.WriteLine("Order has been closed");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }

        static async Task RemoveOrder(Order currentOrder)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
                new ItemView { Id = 1, Value = "Yes"},
                new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to remove this order {0} ?]", currentOrder.CustomerName), startY: 2);

            if (result == 1)
            {
                await _orders.DeleteOrderAsync(currentOrder);
                Console.WriteLine("Order removed");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }

        static async Task EditOrder(Order currentOrder)
        {
            await _orders.UpdateOrderAsync(await CreateOrUpdateOrder(currentOrder));
            Console.WriteLine("Order changed");
        }

        static async Task AddOrder()
        {
            await _orders.UpdateOrderAsync(await CreateOrUpdateOrder());
            Console.WriteLine("Order added");
        }
        static async Task<Order> CreateOrUpdateOrder(Order? currentOrder = null)
        {
            var allBooks = await _books.GetAllBooksAsync();

            string customerName = InputHelper.GetString("Customer 'Name'");
            string customerCity = InputHelper.GetString("Customer 'City'");
            string customerAdress = InputHelper.GetString("Customer 'Adress'");

            Order order = new Order
            {
                Id = currentOrder?.Id ?? 0,
                CustomerName = customerName,
                City = customerCity,
                Address = customerAdress,
            };

            string buffer = String.Format("Enter customer 'Name': {0}\nEnter customer 'City': {1}\nEnter customer 'Adress': {2}\n" +
                "Selected books:\n", customerName, customerCity, customerAdress);

            var books = allBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
            Dictionary<int, (string name, int count)> bufferBooks = new Dictionary<int, (string name, int count)>();
            int bookId = -1, startY = 5;

            while (bookId != 0)
            {
                bookId = ItemsHelper.MultipleChoice(true, new List<ItemView>(books), true,
                    message: buffer + String.Join("\n", bufferBooks.Select(e => e.Value)), startY: startY++);

                if (bookId != 0)
                {
                    if (bufferBooks.ContainsKey(bookId))
                    {
                        bufferBooks[bookId] = (bufferBooks[bookId].name, bufferBooks[bookId].count + 1);
                    }
                    else
                    {
                        bufferBooks.Add(bookId, (name: allBooks.FirstOrDefault(e => e.Id == bookId)!.Title, count: 1));
                    }
                }
            }
            order.Lines = bufferBooks.Select(e => new OrderLine
            {
                BookId = e.Key,
                Quantity = e.Value.count
            }).ToList();

            return order;
        }

        static async Task BrowseBooks(Order currentOrder)
        {
            var books = currentOrder.Lines.Select(e => new ItemView
            {
                Id = e.Book.Id,
                Value = e.Book.Title + $"(Count: {e.Quantity})"
            }).ToList();

            if (books.Count() > 0)
            {
                int result = ItemsHelper.MultipleChoice(true, books, true, optionsPerLine: 1);

                if (result != 0)
                {
                    Book currentBook = await _books.GetBookWithCategoryAndAuthorsAsync(result);
                    await BookInfo(currentBook);
                }
                else
                {
                    Console.WriteLine("No books were added to order");
                    Console.ReadLine();
                }
            }
        }
    }
}
