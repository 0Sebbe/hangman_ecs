using Hangman.Components;
using Hangman.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Hangman.Systems
{
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
}