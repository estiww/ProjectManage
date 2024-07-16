namespace Stage0
{
    partial  class Program
    {
        static void Main(string[] args)
        {
            Welcome9645();
            Welcome8589();
            Console.ReadKey();

        }
        static partial void Welcome9645();

        private static void Welcome8589()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}