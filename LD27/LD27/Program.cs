using System;

namespace LD27
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LD27Game game = new LD27Game())
            {
                game.Run();
            }
        }
    }
#endif
}

