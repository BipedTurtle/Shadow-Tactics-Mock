using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Shuriken : MonoBehaviour, IStorable
    {
        private void Start()
        {
            this.belongsTo = GetComponentInParent<Ninja>();
        }


        public event Action ShurikenLanded;
        private void OnTriggerEnter(Collider other)
        {
            var enemyHit = other.GetComponent<Enemy>();
            if (!enemyHit)
                return;

            transform.SetParent(enemyHit.transform);
            enemyHit.Inventory.Add(this);
            gameObject.SetActive(false);
            this.ShurikenLanded?.Invoke();
        }


        private Ninja belongsTo;
        public void OnStored()
        {
            this.gameObject.SetActive(true);
        }
    }
}
