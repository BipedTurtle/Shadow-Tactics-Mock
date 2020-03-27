using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Trap : MonoBehaviour, IStorable
    {
        [SerializeField] private float activationRadius = 1f;

        private void Start()
        {
            UpdateManager.Instance.GlobalUpdate += this.Detect;
        }

        private void Detect()
        {
            if (!canBeUsed)
                return;

            var enemies =
                Enemy.Enemies.
                    Select(e => (distance: Vector3.Distance(e.Position, transform.position), enemy: e)).
                    Where(e => e.distance < this.activationRadius).
                    Select(e => e.enemy);

            foreach (var enemy in enemies)
                this.Attack(enemy);
        }

        private int damage = 100;
        private bool canBeUsed = true;
        private void Attack(Enemy enemy)
        {
            enemy.TakeDamage(this.damage);
            this.canBeUsed = false;
        }

        public void InstallAt(Vector3 installSpot)
        {
            gameObject.SetActive(true);
            var verticalOffset = Vector3.up * transform.localScale.y;
            transform.position = installSpot + verticalOffset;
            transform.SetParent(null);
        }

        public void OnStored()
        {
            this.canBeUsed = true;
        }
    }
}
