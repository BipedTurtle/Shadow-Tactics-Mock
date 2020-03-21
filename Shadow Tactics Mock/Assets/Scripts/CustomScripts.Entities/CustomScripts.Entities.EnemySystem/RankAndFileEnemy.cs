using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Managers;

namespace CustomScripts.Entities.EnemySystem
{
    public class RankAndFileEnemy : Enemy
    {
        protected override void Start()
        {
            base.Start();

            this.rallyPoints = this.rallies.Select(r => r.position).ToArray();
            transform.position = this.rallyPoints[0];

            UpdateManager.Instance.GlobalUpdate += this.Patrol;
        }

        [SerializeField] private Transform[] rallies;
        private Vector3[] rallyPoints;
        private int rallyIndex = 1;
        protected override void Patrol()
        {
            var destination = this.rallyPoints[rallyIndex];
            var threshold = .6f;
            var reachedDestination = (destination - transform.position).magnitude < threshold;

            if (reachedDestination) {
                this.rallyIndex = GetNextRally();
                return;
            }

            this.WalkToRallyPoint(destination);


            int GetNextRally()
            {
                var ralliesMaxIndex = this.rallies.Count() - 1;
                if (this.rallyIndex >= ralliesMaxIndex) {
                    this.rallyPoints = rallyPoints.Reverse().ToArray();
                    return 1;
                }

                return this.rallyIndex + 1;
            }
        }

        private void WalkToRallyPoint(Vector3 rallyPoint)
        {
            base.agent.SetDestination(rallyPoint);
        }
    }
}
