using System.Collections.Generic;
using System.Linq;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;

namespace CustomScripts.Entities
{
    public class Inventory
    {
        private List<IStorable> storables = new List<IStorable>();

        public void Add(IStorable storable)
        {
            this.storables.Add(storable);
            storable.OnStored();
        }

        public void Add(Inventory other)
        {
            foreach (var item in other.storables) {
                this.storables.Add(item);
                item.OnStored();
            }
        }

        public bool Contains(IStorable storable) => this.storables.Contains(storable);

        public void Remove(IStorable storable) => this.storables.Remove(storable);

        public void Display()
        {
            if (this.storables.Count == 0)
                Debug.Log("Inventory empty");

            foreach (var item in this.storables)
                Debug.Log(item);
        }

        public T Get<T>()
        {
            var toReturn =
                this.storables
                    .OfType<T>()
                    .FirstOrDefault();

            return toReturn;
        }

        public void ProbeInventory(Enemy enemy)
        {
            this.Add(enemy.Inventory);
        }
    }
}
