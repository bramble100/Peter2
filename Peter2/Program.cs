using System;

namespace XMLParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                (new Controller()).Execute(args);
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error: {ex.InnerException.Message}\n");
                }

                Console.WriteLine(Controller.HelpString);
            }
        }
    }
}
