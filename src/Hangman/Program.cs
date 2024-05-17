using Hangman.Game;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Initialize();
        game.Start();
    }
}