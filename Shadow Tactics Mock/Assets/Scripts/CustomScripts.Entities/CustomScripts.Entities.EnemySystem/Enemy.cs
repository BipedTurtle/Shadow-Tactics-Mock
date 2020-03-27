using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CustomScripts.Managers;
using CustomScripts.Entities.PlayerSystem;

namespace CustomScripts.Entities.EnemySystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour
    {
        public static List<Enemy> Enemies { get; } = new List<Enemy>();

        public Vector3 Position { get => transform.position; }
        public Inventory Inventory { get; } = new Inventory();


        [SerializeField] private int health = 100;
        public void TakeDamage(IPlayerSkill skill)
        {
            this.health -= skill.Damage;
            
            if (this.health <= 0)
                Die();
        }

        public void TakeDamage(int dmg)
        {
            this.health -= dmg;

            if (this.health <= 0)
                Die();
        }


        public abstract void Die();


        protected NavMeshAgent agent;
        protected FieldOfView fov;
        protected virtual void Start()
        {
            Enemies.Add(this);

            this.agent = GetComponent<NavMeshAgent>();
            this.fov = GetComponentInChildren<FieldOfView>() ?? throw new MissingComponentException("Enemy object" +
                "requires field of view");
        }

        private void OnDestroy()
        {
            Enemies.Remove(this);
        }


        private Player target;
        protected void AttackPlayerInView()
        {
            foreach (var player in Player.players)
                this.target = this.fov.IsWithinView(player.transform) ? player : this.target;

            if (target != null)
                Attack(this.target.transform);


            void Attack(Transform targetTransform) 
            {
                this.agent.SetDestination(targetTransform.position);
            }
        }

        protected void StopAttackingIfOutSideView()
        {
            if (this.target == null)
                return;

            var isOutSideOfViewRadius = (this.target.Position - transform.position).sqrMagnitude > Mathf.Pow(this.fov.ViewRadius, 2);
            var isOutSideOfViewAngle = !this.fov.IsWithinView(this.target.transform);

            if (isOutSideOfViewRadius && isOutSideOfViewAngle)
                this.target = null;
        }

        protected abstract void Patrol();

        public abstract void Freeze();
    }
}
