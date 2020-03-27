using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomScripts.Entities.EnemySystem;
using UnityEngine.AI;
using UnityEngine;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    public class BasicAttack : IPlayerSkill
    {
        private Player player;
        private NavMeshAgent agent;
        public int Damage { get; } = 100; 

        public BasicAttack(Player player)
        {
            this.player = player;
            this.agent = player.Controller.Agent;

            GameManager.Instance.CancelSkill += this.Cancel;   
        }

        private float attackRange = 1.2f;
        public IPlayerSkill Implement(Enemy target, ActionType actionType)
        {
            var typeMismatch = (actionType != ActionType.Attack);
            if (typeMismatch)
                return new NoSkill(this.player);

            this.player.Controller.Lock();
            this.player.StartCoroutine(Logic());

            return new NoSkill(this.player);


            IEnumerator Logic()
            {
                var isWithinRange = Chase();
                if (isWithinRange)
                    Attack();

                yield return null;
                if (!isWithinRange)
                    this.player.StartCoroutine(Logic());
            }

            bool Chase()
            {
                var targetPos = target.transform.position;
                var distance = (targetPos - player.Position).sqrMagnitude;
                var targetWithinRange = distance < Mathf.Pow(this.attackRange, 2);
                if (!targetWithinRange)
                    this.agent.SetDestination(targetPos);

                return targetWithinRange;
            }

            void Attack()
            {
                this.agent.isStopped = true;
                this.player.transform.LookAt(target.transform.position);
                this.player.Animator.SetTrigger("Basic Attack");
                target.TakeDamage(skill: this);

                player.StopAllCoroutines();
                this.player.Controller.UnLock(unlockAfter:1f);
                this.agent.isStopped = false;
            }
        }

        public void Cancel()
        {
            this.player.StopAllCoroutines();
            this.player.Controller.Agent.ResetPath();
            this.player.Controller.UnLock();
        }
    }
}
