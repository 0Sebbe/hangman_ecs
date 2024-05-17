using System;
using System.Collections.Generic;

//----Entity----
public class Entity
{
    public List<Component> Components { get; } = new List<Component>();
}

//----Components----
public abstract class Component { }

public class HangmanWordComponent : Component
{
    public string Word { get; set; }
}

public class GuessedLetterComponent : Component
{
    public char Letter { get; set; }
}

public class HangmanGameStateComponent : Component
{
    public int RemainingAttempts { get; set; }
    public bool IsGameOver { get; set; }
}


//----System----
public class HangmanSystem
{
    private List<Entity> entities = new List<Entity>();

    public Entity CreateEntity(params Component[] components)
    {
        var entity = new Entity();
        foreach (var component in components)
        {
            entity.Components.Add(component);
        }
        entities.Add(entity);
        return entity;
    }

    public IEnumerable<Entity> GetEntitiesWith<T>() where T : Component
    {
        foreach (var entity in entities)
        {
            foreach (var component in entity.Components)
            {
                if (component is T)
                {
                    yield return entity;
                    break;
                }
            }
        }
    }
}


//----Game----
public class Game
{
    private List<string> words = new List<string> { "coffee", "store", "keyboard", "laptop", "programming", "hamburger", "school", "language", "controller", "popularity" };
    private string currentWord;
    private char[] wordState;
    private HangmanSystem hangmanSys = new HangmanSystem();

    public void Initialize()
    {
        currentWord = words[new Random().Next(0, words.Count)];
        wordState = new char[currentWord.Length];
        for (int i = 0; i < currentWord.Length; i++)
        {
            wordState[i] = '_';
        }

        var hangmanEntity = hangmanSys.CreateEntity(new HangmanWordComponent { Word = currentWord },
                                                    new HangmanGameStateComponent { RemainingAttempts = 6, IsGameOver = false });
        Console.WriteLine("Welcome to Hangman!");
        Console.WriteLine("Guess the word: " + new string(wordState));
    }

    public void Start()
    {
        while (true)
        {
            bool isGameOver = true;
            foreach (var entity in hangmanSys.GetEntitiesWith<HangmanGameStateComponent>())
            {
                var hangmanState = (HangmanGameStateComponent)entity.Components.Find(c => c is HangmanGameStateComponent);
                if (!hangmanState.IsGameOver)
                {
                    isGameOver = false;
                    break;
                }
            }

            if (isGameOver)
            {
                Console.WriteLine("Game over! The word was: " + currentWord);
                return;
            }

            Console.WriteLine("Enter a letter: ");
            string input = Console.ReadLine().ToLower();

            if (input.Length == 1 && char.IsLetter(input[0]))
            {
                char letter = input[0];

                hangmanSys.CreateEntity(new GuessedLetterComponent { Letter = letter });

                bool correctGuess = false;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    if (currentWord[i] == letter)
                    {
                        wordState[i] = letter;
                        correctGuess = true;
                    }
                }

                if (!correctGuess)
                {
                    foreach (var entity in hangmanSys.GetEntitiesWith<HangmanGameStateComponent>())
                    {
                        var hangmanState = (HangmanGameStateComponent)entity.Components.Find(c => c is HangmanGameStateComponent);
                        if (!hangmanState.IsGameOver)
                        {
                            hangmanState.RemainingAttempts--;
                            if(hangmanState.RemainingAttempts == 0)
                            {
                                Console.WriteLine("Game over. You lost.");
                                return;
                            }
                            Console.WriteLine("Incorrect guess. Remaining attempts: " + hangmanState.RemainingAttempts);
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Correct guess!");
                }

                Console.WriteLine("Current state: " + new string(wordState));

                if (new string(wordState) == currentWord)
                {
                    foreach (var entity in hangmanSys.GetEntitiesWith<HangmanGameStateComponent>())
                    {
                        var hangmanState = (HangmanGameStateComponent)entity.Components.Find(c => c is HangmanGameStateComponent);
                        if (!hangmanState.IsGameOver)
                        {
                            hangmanState.IsGameOver = true;
                            break;
                        }
                    }
                    Console.WriteLine("Congratulations! You guessed the word!");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a single letter.");
            }
        }
    }
}

//----Runtime----
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Initialize();
        game.Start();
    }
}