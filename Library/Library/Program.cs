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
		readonly bool isBorrowed;

		public Book(string _title)
		{
			title = _title;
			isBorrowed = false;
		}

		public string GetTitle()
		{
			return title;
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
		private Book GetBookByTitle(string title)
		{
			for(int i = 0; i < books.Count; i += 1)
				if(books[i].GetTitle() == title)
					return books[i];

			return null;
		}

		public void AddBookToLibrary()
		{
			Console.WriteLine("Please enter the title of the book you would like to add.");
			books.Add(new Book(Console.ReadLine()));
			Console.WriteLine("Your book has been added to the library.");
		}
	}
}
