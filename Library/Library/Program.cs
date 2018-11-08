using System;
using System.Collections.Generic;

namespace Library
{
	class Program
	{
		static void Main(string[] args)
		{
			Library lib = new Library();
		}
	}

	class Book
	{
		readonly string title;
		readonly string author;
		readonly bool isBorrowed;

		public Book(string _title, string _author)
		{
			title = _title;
			author = _author;
			isBorrowed = false;
		}

		public string GetTitle()
		{
			return title;
		}

		public string GetAuthor()
		{
			return author;
		}

		public bool IsBorrowed()
		{
			return isBorrowed;
		}
	}	

	class Library
	{
		readonly List<Book> books = new List<Book>();

		/// <summary>
		/// Retrieves a book based on whether the parameter "title" matches the book's
		/// "title" variable.
		/// </summary>
		/// <param name="title">The title to be used for searching</param>
		/// <returns>Returns an object of type Book, null if no match is found.</returns>
		private Book GetBookByTitleAndAuthor(string title, string author)
		{
			for(int i = 0; i < books.Count; i += 1)
				if(books[i].GetTitle() == title && books[i].GetAuthor() == author)
					return books[i];

			return null;
		}

		public void AddBookToLibrary()
		{
			Console.WriteLine("Please enter the title of the book you would like to add.");
			string tempTitle = Console.ReadLine();
			Console.WriteLine("Who authored the book?");
			string tempAuthor = Console.ReadLine();
			
			if(GetBookByTitleAndAuthor(tempTitle, tempAuthor) == null)
			{
				books.Add(new Book(tempTitle, tempAuthor));
				Console.WriteLine("Book has been added to the library.");
			}
			else
				Console.WriteLine("A book with that title and author is already in the library.");

		}
	}
}
