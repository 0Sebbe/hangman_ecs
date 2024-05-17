namespace Hangman.Components
{
    public class HangmanGameStateComponent : Component
    {
        public int RemainingAttempts { get; set; }
        public bool IsGameOver { get; set; }
    }
}