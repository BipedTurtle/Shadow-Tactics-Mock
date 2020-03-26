using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Shuriken : MonoBehaviour
    {
        public event Action ShurikenLanded;
        private void OnTriggerEnter(Collider other)
        {
            var enemyHit = other.GetComponent<Enemy>();
            if (!enemyHit)
                return;

            transform.SetParent(enemyHit.transform);
            this.ShurikenLanded?.Invoke();
        }
    }
}
