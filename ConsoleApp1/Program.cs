using HomeWork6.Data;
using HomeWork6.Helpers;
using HomeWork6.Interfaces;
using HomeWork6.Models;
using HomeWork6.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace HomeWork6;

partial class Program
{

    enum ShopMenu
    {
        Books, Authors, Categories, Orders, SearchAuthors, SearchBooks, SearchCategories, SearchOrders, AddBook, AddAuthor, AddCategory, AddOrder, Exit
    }


    private static IAuthor _authors;
    private static IBook _books;
    private static ICategory _categories;
    private static IOrder _orders;
    private static IPromotion _promotions;
    public static ApplicationContext DbContext() => new ApplicationContextFactory().CreateDbContext();
    static async Task Main()
    {
        Initialize();
        await Menu();
    }

    private static async Task Menu()
    {
        int input = new int();
        do
        {
            input = ConsoleHelper.MultipleChoice(true, new ShopMenu());
            switch ((ShopMenu)input)
            {
                case ShopMenu.Books:
                    await ReviewBooks();
                    break;
                case ShopMenu.Authors:
                    await ReviewAuthors();
                    break;
                case ShopMenu.Categories:
                    await ReviewCategories();
                    break;
                case ShopMenu.Orders:
                    await ReviewOrders();
                    break;
                case ShopMenu.SearchAuthors:
                    await SearchAuthors();
                    break;
                case ShopMenu.SearchBooks:
                    await SearchBooks();
                    break;
                case ShopMenu.SearchCategories:
                    await SearchCategories();
                    break;
                case ShopMenu.SearchOrders:
                    break;
                case ShopMenu.AddBook:
                    await AddBook();
                    break;
                case ShopMenu.AddAuthor:
                    await AddAuthor();
                    break;
                case ShopMenu.AddCategory:
                    await AddCategory();
                    break;
                case ShopMenu.AddOrder:
                    await AddOrder();
                    break;
                case ShopMenu.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        } while (true);
    }

    static void Initialize()
    {
        new DbInit().Init(DbContext());
        _books = new BookRepository();
        _authors = new AuthorRepository();
        _categories = new CategoryRepository();
        _orders = new OrderRepository();
        _promotions = new PromotionRepository();
    }
}
