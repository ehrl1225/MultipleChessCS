using Server;
using Chess;

namespace Main
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                Console.WriteLine("Too many argument");
                return;
            } else if (args.Length == 0)
            {
                Console.WriteLine("Need more argument");
                return;
            }
            string arg = args[0];
            int port;
            bool result = int.TryParse(arg, out port);
            if (!result)
            {
                Console.WriteLine("argument is not int");
            }
            Server.Server server = new(port);
            server.Run();
            
        }
    }
    
}