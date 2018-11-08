using System;
using System.Collections.Generic;

namespace Library
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] options = {"Display a list of all books","Add a new book to the library","Loan a book"};
			bool isRunning = true;
			Library lib = new Library();

			while(isRunning)
			{
				for(int i = 0; i < options.Length; i += 1)
				{
					Console.WriteLine(i+1 + ") " + options[i]);
				}

				switch (Console.ReadLine()[0])
				{
					case '1':
						lib.DisplayAllBooks();
						break;

					case '2':
						lib.AddBookToLibrary();
						break;

					case '3':
						
						break;
				}
			}
		}
	}

	class Loaner
	{
		readonly List<Book> loanedBooks = new List<Book>();
		readonly string loanerName;
	}

	class Book
	{
		readonly string title;
		readonly string author;
		readonly DateTime timeLoaned;
		readonly bool isLoaned;

		public Book(string _title, string _author)
		{
			title = _title;
			author = _author;
		}

		public string GetTitle()
		{
			return title;
		}

		public string GetAuthor()
		{
			return author;
		}

		public DateTime GetTimeLoaned()
		{
			return timeLoaned;
		}

		public bool IsLoanedOut()
		{
			return isLoaned;
		}
	}	

	class Library
	{
		readonly List<Book> books = new List<Book>();
		readonly DateTime maxLoanTime = new DateTime(0);

		public void DisplayAllBooks()
		{
			string loanedStat;
			for(int i = 0; i < books.Count; i += 1)
			{
				if(books[i].IsLoanedOut())
					loanedStat = "Yes";
				else
					loanedStat = "No";

				Console.WriteLine($"Title: {books[i].GetTitle()}\nAuthor: {books[i].GetAuthor()}\nLoaned out: {loanedStat}");
			}
		}

		public Book LoanBook()
		{
			return new Book("", "");
		}

		/// <summary>
		/// Matches the input title and author to all books
		/// in the "books" list until it finds a match
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
