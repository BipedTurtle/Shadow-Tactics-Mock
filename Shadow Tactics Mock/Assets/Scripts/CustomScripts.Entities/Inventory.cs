using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;

namespace CustomScripts.Entities
{
    public class Inventory
    {
        private HashSet<IStorable> storables = new HashSet<IStorable>();

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

        public T Get<T>()
        {
            var toReturn =
                this.storables
                    .OfType<T>()
                    .FirstOrDefault();

            this.storables.Remove(toReturn as IStorable);
            return toReturn;
        }

        public void ProbeInventory(Enemy enemy)
        {
            this.Add(enemy.Inventory);
        }
    }
}
