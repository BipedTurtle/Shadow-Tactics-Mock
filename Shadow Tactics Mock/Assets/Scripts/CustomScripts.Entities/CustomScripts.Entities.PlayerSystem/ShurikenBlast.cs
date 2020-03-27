using System;
using System.Collections;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;
using CustomScripts.Managers;

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
            GameManager.Instance.CancelSkill += this.Cancel;
        }

        private Enemy target;
        private float range = 4f;
        public IPlayerSkill Implement(Enemy target, ActionType actionType)
        {
            var typeMismatch = (actionType != ActionType.Attack);
            if (typeMismatch)
                return new NoSkill(this.ninja);

            var hasShuriken = this.shuriken != null;
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
                    this.target.Freeze();

                    var lookVector = this.target.Position - this.ninja.Position;
                    var angleBetween = Vector3.Angle(this.ninja.transform.forward, lookVector);
                    var threshold = .5f;
                    var shouldTurnMore = angleBetween > threshold;
                    if (shouldTurnMore)
                        this.ninja.StartCoroutine(Turn());
                    else {
                        var startPos = this.ninja.transform.position;
                        var targetPos = this.target.transform.position;
                        this.shuriken.Consume(startPos, targetPos);

                        this.ninja.Controller.Agent.ResetPath();
                    }
                }
                else {
                    Chase();
                    this.ninja.StartCoroutine(Logic());
                }
            }

            IEnumerator Turn(float progress = 0)
            {
                this.ninja.Controller.Agent.ResetPath();

                var angularSpeed = 30f;
                var t = progress + angularSpeed * Time.fixedDeltaTime;
                var currentRotation = this.ninja.transform.rotation;
                var lookVector = this.target.Position - this.ninja.Position;
                var targetRotation = Quaternion.LookRotation(lookVector);
                this.ninja.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, t);

                yield return null;
                var toTargetVector = this.target.Position - this.ninja.Position;
                var angleBetween = Vector3.Angle(this.ninja.transform.forward, toTargetVector);
                var threshold = 1f;
                if (angleBetween < threshold)
                    this.ninja.StartCoroutine(Logic());
                else
                    this.ninja.StartCoroutine(Turn());
            }

            bool Chase()
            {
                var sqrDistance = (target.transform.position - this.ninja.Position).sqrMagnitude;
                var isWithinRange = sqrDistance < Mathf.Pow(this.range, 2);

                if (!isWithinRange)
                    this.ninja.Controller.Agent.SetDestination(target.transform.position);

                return isWithinRange;
            }
        }

        private void OnShurikenLanded_DealDamage()
        {
            this.target?.TakeDamage(this);
        }

        public void Cancel()
        {

        }
    }
}
