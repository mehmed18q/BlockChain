namespace BlockChainConsole
{
	internal static class Statics
	{
		public static int ShowMenu()
		{
			Console.WriteLine($"Login User: {Program.Name}");
			Console.WriteLine("=========================");
			Console.WriteLine("1. Connect to a Server");
			Console.WriteLine("2. Add a Transaction");
			Console.WriteLine("3. Display Blockchain");
			Console.WriteLine("4. Change User Name");
			Console.WriteLine("5. Exit");
			Console.WriteLine("=========================");

			Console.Write("Please Select an Action: ");
			string? action = Console.ReadLine();
			_ = int.TryParse(action ?? "0", out int selected);

			return selected;
		}
	}
}
