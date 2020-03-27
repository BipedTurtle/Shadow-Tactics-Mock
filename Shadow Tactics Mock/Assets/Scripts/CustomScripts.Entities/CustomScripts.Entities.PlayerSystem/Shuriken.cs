using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Managers;

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
            this.ShurikenLanded?.Invoke();
        }


        private Ninja belongsTo;
        public void OnStored()
        {
            this.gameObject.SetActive(true);
            transform.SetParent(this.belongsTo.transform);
            transform.position = this.belongsTo.Position;
        }


        public void Consume(Vector3 shootingCoord, Vector3 targetCoord)
        {
            this.belongsTo.Inventory.Remove(this);
            gameObject.SetActive(true);

            var speed = 30f;
            this.ShurikenLanded += StopFlying;
            UpdateManager.Instance.GlobalFixedUpdate += FlyToTarget;

            void FlyToTarget()
            {
                Vector3 trajectory = (targetCoord - shootingCoord).normalized;
                var movement = trajectory * speed * Time.fixedDeltaTime;
                transform.position += movement;
            }

            void StopFlying()
            {
                UpdateManager.Instance.GlobalFixedUpdate -= FlyToTarget;
            }
        }
    }
}
