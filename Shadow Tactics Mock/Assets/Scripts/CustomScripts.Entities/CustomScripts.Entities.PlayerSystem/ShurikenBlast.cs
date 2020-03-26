using System;
using System.Collections;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class ShurikenBlast : IPlayerSkill
    {
        public int Damage => 100;

        private Player player;
        public ShurikenBlast(Player player)
        {
            this.player = player;
            this.player.Shuriken.ShurikenLanded += this.OnShurikenLanded_DealDamage;
        }

        private Enemy target;
        private float range = 4f;
        public IPlayerSkill Implement(Enemy target)
        {
            this.target = target;
            this.player.StartCoroutine(Logic());
            return new NoSkill(this.player);


            IEnumerator Logic()
            {
                yield return null;

                var isWithinRange = Chase();
                if (isWithinRange) {
                    var startPos = this.player.Shuriken.transform.position;
                    var targetPos = target.transform.position;
                    this.player.StartCoroutine(ThrowShuriken(startPos, targetPos));
                }
                else
                {
                    Chase();
                    this.player.StartCoroutine(Logic());
                }
            }

            bool Chase()
            {
                var sqrDistance = (target.transform.position - this.player.Position).sqrMagnitude;
                var isWithinRange = sqrDistance < Mathf.Pow(this.range, 2);

                if (!isWithinRange)
                    this.player.Controller.Agent.SetDestination(target.transform.position);

                return isWithinRange;
            }
            
            IEnumerator ThrowShuriken(Vector3 startPos, Vector3 targetPos, float progress=0)
            {
                Transform shuriken = this.player.Shuriken.transform;

                var speed = 3f;
                float t = progress + speed * Time.fixedDeltaTime;
                shuriken.position = Vector3.Lerp(startPos, targetPos, t);

                yield return null;
                if (t < 1)
                    this.player.StartCoroutine(ThrowShuriken(startPos, targetPos, t));
            }
        }

        private void OnShurikenLanded_DealDamage()
        {
            this.target?.TakeDamage(this);
        }
    }
}
