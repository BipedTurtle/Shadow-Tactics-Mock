using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomScripts.Managers;
using CustomScripts.Entities.PlayerSystem;

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
            UpdateManager.Instance.GlobalUpdate += base.AttackPlayerInView;
            UpdateManager.Instance.GlobalUpdate += base.StopAttackingIfOutSideView;
        }


        public override void Freeze() {
            this.agent.isStopped = true;
            UpdateManager.Instance.GlobalUpdate -= this.Patrol;
            UpdateManager.Instance.GlobalUpdate -= base.AttackPlayerInView;
            UpdateManager.Instance.GlobalUpdate -= base.StopAttackingIfOutSideView;
        }


        public override void Die()
        {
            this.fov.DeVisualizeFOV();
            this.Freeze();
        }


        [SerializeField] private Transform[] rallies;
        private Vector3[] rallyPoints;
        private int rallyIndex = 1;
        private enum PatrolPattern { BackAndForth, Loop }
        [SerializeField] private PatrolPattern patrolPattern;
        protected override void Patrol()
        {
            var destination = this.rallyPoints[rallyIndex];
            var threshold = .6f;
            var reachedDestination = (destination - transform.position).magnitude < threshold;

            if (reachedDestination) {
                this.rallyIndex = this.GetNextRally();
                return;
            }

            this.WalkToRallyPoint(destination);

            
        }

        private int GetNextRally()
        {
            switch (this.patrolPattern) {
                case PatrolPattern.BackAndForth:
                    return this.GetNextRallyBackAndForth();
                case PatrolPattern.Loop:
                    return this.GetNextRallyLoop();
                default:
                    throw new ArgumentException("The given pattern is not one of the defined patrol patterns"); }
        }

        private int RalliesCount => this.rallies.Count();
        private int GetNextRallyBackAndForth()
        {
            var maxRallyIndex = this.RalliesCount - 1;
            if (this.rallyIndex >= maxRallyIndex) {
                this.rallyPoints = rallyPoints.Reverse().ToArray();
                return 1;
            }

            return this.rallyIndex + 1;
        }

        private int GetNextRallyLoop()
        {
            return (this.rallyIndex + 1) % this.RalliesCount;
        }

        private void WalkToRallyPoint(Vector3 rallyPoint)
        {
            base.agent.SetDestination(rallyPoint);
        }
    }
}
