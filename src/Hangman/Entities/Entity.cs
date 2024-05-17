using Hangman.Components;
using System.Collections.Generic;

namespace Hangman.Entities
{
    public class Entity
    {
        public List<Component> Components { get; } = new List<Component>();
    }
}