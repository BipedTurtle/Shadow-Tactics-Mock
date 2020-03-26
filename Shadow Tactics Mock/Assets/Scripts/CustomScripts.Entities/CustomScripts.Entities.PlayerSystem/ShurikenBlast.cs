using System;
using System.Collections;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class ShurikenBlast : IPlayerSkill
    {
        public int Damage => 100;

        private Ninja ninja;
        private Shuriken shuriken;
        public ShurikenBlast(Player player)
        {
            this.ninja = (player as Ninja) ?? throw new ArgumentException("Only a Ninja can use this skill. It's not for other types, such as Yuki or Samurai");
            this.shuriken = this.ninja.Inventory.Get<Shuriken>();

            this.ninja.Shuriken.ShurikenLanded += this.OnShurikenLanded_DealDamage;
        }

        private Enemy target;
        private float range = 4f;
        public IPlayerSkill Implement(Enemy target)
        {
            var hasShuriken = this.shuriken != null;
            Debug.Log(hasShuriken);
            if (hasShuriken) {
                this.target = target;
                this.ninja.StartCoroutine(Logic());
            }

            return new NoSkill(this.ninja);


            IEnumerator Logic()
            {
                yield return null;

                var isWithinRange = Chase();
                if (isWithinRange) {
                    this.shuriken.gameObject.SetActive(true);
                    var startPos = this.ninja.transform.position;
                    var targetPos = this.target.transform.position;
                    this.ninja.StartCoroutine(ThrowShuriken(startPos, targetPos));
                    this.ninja.Controller.Agent.ResetPath();
                }
                else {
                    Chase();
                    this.ninja.StartCoroutine(Logic());
                }
            }

            bool Chase()
            {
                var sqrDistance = (target.transform.position - this.ninja.Position).sqrMagnitude;
                var isWithinRange = sqrDistance < Mathf.Pow(this.range, 2);

                if (!isWithinRange)
                    this.ninja.Controller.Agent.SetDestination(target.transform.position);

                return isWithinRange;
            }
            
            IEnumerator ThrowShuriken(Vector3 startPos, Vector3 targetPos, float progress=0)
            {
                var speed = 3f;
                float t = progress + speed * Time.fixedDeltaTime;
                this.shuriken.transform.position = Vector3.Lerp(startPos, targetPos, t);

                yield return null;
                if (t < 1)
                    this.ninja.StartCoroutine(ThrowShuriken(startPos, targetPos, t));
            }
        }

        private void OnShurikenLanded_DealDamage()
        {
            this.target?.TakeDamage(this);
        }
    }
}
