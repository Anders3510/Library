using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] options = { "Display a list of all books","Display a list of loaners", "Add a new book to the library", "Register a new loaner", "Loan a book" };
			bool isRunning = true;
			Library lib = new Library();

			while (isRunning)
			{
				for (int i = 0; i < options.Length; i += 1)
				{
					Console.WriteLine(i + 1 + ") " + options[i]);
				}

				string input = Console.ReadLine();
				if (input == "")
					continue;

				switch (input[0])
				{
					case '1':
						lib.DisplayAllBooks();
						break;

					case '2': 
						lib.DisplayAllLoaners();
						break;

					case '3':
						lib.AddBookToLibrary();
						break;

					case '4':
						lib.NewLoaner();
						break;

					case '5':
						lib.LoanBook();
						break;
				}

				lib.WriteToFiles();
			}
		}
	}

	class Library
	{
		readonly List<Book> books = new List<Book>();
		readonly List<Loaner> loaners = new List<Loaner>();
		readonly DateTime maxLoanTime = new DateTime(0);
		const string LoanerPath = @"../../../loaners.bin";
		const string BooksPath = @"../../../books.bin";

		public Library()
		{
			if(File.Exists(LoanerPath))
			{
				try
				{
					using (Stream st = File.Open(LoanerPath, FileMode.Open))
					{
						BinaryFormatter bin = new BinaryFormatter();
						loaners = (List<Loaner>)bin.Deserialize(st);
					}
				}
				catch (IOException)
				{
				}
			}

			if(File.Exists(BooksPath))
			{
				try
				{
					using (Stream st = File.Open(BooksPath, FileMode.Open))
					{
						BinaryFormatter bin = new BinaryFormatter();
						books = (List<Book>)bin.Deserialize(st);
					}
				}
				catch (IOException)
				{
				}
			}
		}

		[Serializable]
		class Loaner
		{
			public readonly List<Book> loanedBooks = new List<Book>();
			readonly string loanerName;

			public Loaner(string _loanerName)
			{
				loanerName = _loanerName;
			}

			public string GetName()
			{
				return loanerName;
			}
		}

		[Serializable]
		class Book
		{
			public readonly string title;
			public readonly string author;
			public readonly DateTime timeLoaned;
			public bool isLoaned;

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

		public void NewLoaner()
		{
			Console.WriteLine("Please enter a name.");
			loaners.Add(new Loaner(Console.ReadLine()));
			Console.WriteLine("A new loaner has been registered sucessfully.");
		}

		/// <summary>
		/// Iterates through the "books" List and writes a table of the contents of
		/// the objects to the console.
		/// </summary>
		public void DisplayAllBooks()
		{
			string loanedStat;
			int textPad = 20;
			Console.WriteLine("  " + "Title".PadRight(textPad) + "Author".PadRight(textPad) + "Loaned out");

			for (int i = 0; i < books.Count; i += 1)
			{
				if (books[i].IsLoanedOut())
					loanedStat = "Yes";
				else
					loanedStat = "No";

				Console.WriteLine(i + 1 + " " + books[i].GetTitle().PadRight(textPad) + books[i].GetAuthor().PadRight(textPad) + loanedStat);
			}
		}

		public void DisplayAllLoaners()
		{
			for(int i = 0; i < loaners.Count; i += 1)
			{
				Console.WriteLine(loaners[i].GetName());
				for(int j = 0; j < loaners[i].loanedBooks.Count; j += 1)
				{
					Console.WriteLine("  " + loaners[i].loanedBooks[j].GetTitle() + " by " + loaners[i].loanedBooks[j].GetAuthor());
				}
			}
		}

		/// <summary>
		/// Allows a user to loan a book from the library,
		/// by providing the name of a registered Loaner.
		/// </summary>
		public void LoanBook()
		{
			string tempName;
			Console.WriteLine("Enter the name of the loaner to register the loan to.");
			tempName = Console.ReadLine();
			if (GetLoanerByName(tempName) != null)
			{
				DisplayAllBooks();
				Console.WriteLine("Please select which book you would like to loan.");
				string input = Console.ReadLine();
				int result;

				while (!int.TryParse(input, out result))
				{
					Console.WriteLine("Please write a number.");
					input = Console.ReadLine();
				}

				if (result > books.Count)
					Console.WriteLine("Something went wrong, please try again.");
				else
				{
					if (!books[result - 1].IsLoanedOut())
					{
						GetLoanerByName(tempName).loanedBooks.Add(books[result - 1]);
						books[result - 1].isLoaned = true;
						Console.WriteLine("The book has been loaned sucessfully.");
					}
					else
						Console.WriteLine("That book is already loaned out.");
				}
			}
			else Console.WriteLine("No loaner has that name");
		}

		/// <summary>
		/// A helper function that retrieves a reference to a Loaner object
		/// based on the "name" variable of the object.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private Loaner GetLoanerByName(string name)
		{
			for (int i = 0; i < loaners.Count; i += 1)
				if (loaners[i].GetName() == name)
					return loaners[i];

			return null;
		}

		/// <summary>
		/// Matches the input title and author to all books
		/// in the "books" list until it finds a match
		/// </summary>
		/// <param name="title">The title to be used for searching.</param>
		/// <param name="author">The author to be used for searching.</param>
		/// <returns>Returns an object of type Book, null if no match is found.</returns>
		private Book GetBookByTitle(string title, string author)
		{
			for (int i = 0; i < books.Count; i += 1)
				if (books[i].GetTitle() == title && books[i].GetAuthor() == author)
					return books[i];

			return null;
		}

		/// <summary>
		/// Matches the input title to all books
		/// in the "books" list until it finds a match
		/// </summary>
		/// <param name="title">The title to be used for searching</param>
		/// <returns>Returns an object of type Book, null if no match is found.</returns>
		private Book GetBookByTitle(string title)
		{
			for (int i = 0; i < books.Count; i += 1)
				if (books[i].GetTitle() == title)
					return books[i];

			return null;
		}

		/// <summary>
		/// Adds a new book to the library based on user input.
		/// </summary>
		public void AddBookToLibrary()
		{
			Console.WriteLine("Please enter the title of the book you would like to add.");
			string tempTitle = Console.ReadLine().Trim();
			while (tempTitle == "")
			{
				Console.WriteLine("Please write a title.");
				tempTitle = Console.ReadLine().Trim();
			}

			Console.WriteLine("Who authored the book?");
			string tempAuthor = Console.ReadLine().Trim();
			while (tempTitle == "")
			{
				Console.WriteLine("Please write a title.");
				tempTitle = Console.ReadLine().Trim();
			}


			if (GetBookByTitle(tempTitle, tempAuthor) == null)
			{
				books.Add(new Book(tempTitle, tempAuthor));
				Console.WriteLine("Book has been added to the library.");
			}
			else
				Console.WriteLine("A book with that title and author is already in the library.");
		}

		public void WriteToFiles()
		{
			try
			{
				using (Stream st = File.Open(LoanerPath, FileMode.Create))
				{
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(st, loaners);
				}
			}
			catch (IOException)
			{
			}

			try
			{
				using (Stream st = File.Open(BooksPath, FileMode.Create))
				{
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(st, books);
				}
			}
			catch (IOException)
			{
			}
		}
	}
}