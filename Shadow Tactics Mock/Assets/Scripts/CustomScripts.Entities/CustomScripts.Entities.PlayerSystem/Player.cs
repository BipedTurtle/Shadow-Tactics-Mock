using System.Collections.Generic;
using UnityEngine;
using CustomScripts.Managers;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Utility;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]
    public abstract class Player : MonoBehaviour
    {
        public static List<Player> players { get; } = new List<Player>();
        public Vector3 Position => transform.position;
        public Animator Animator { get; private set; }
        public PlayerController Controller { get; private set; }

        protected virtual void Start()
        {
            players.Add(this);
            this.Animator = GetComponent<Animator>();
            this.Controller = GetComponent<PlayerController>();
            this.skill = new NoSkill(this);

            UpdateManager.Instance.GlobalUpdate += this.ChooseSkill;
            UpdateManager.Instance.GlobalUpdate += this.UponRightMouseButtonClicked;
            GameManager.Instance.ImplementAttack += this.ImplementSkill;
        }

        protected IPlayerSkill skill;
        protected abstract void ChooseSkill();

        protected void ImplementSkill(Enemy target)
        {
            this.skill.Implement(target);
        }

        private void UponRightMouseButtonClicked()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            this.ProbeEnemy();
        }


        public Inventory Inventory { get; } = new Inventory();
        private Enemy probingTarget;
        private bool HasProbingTarget => this.probingTarget != null;
        protected void ProbeEnemy()
        {
            var ray = MainCamera.Instance.FromScreenPointToRay(Input.mousePosition);
            var distance = MainCamera.Instance.transform.position.y * 2;
            var enemyHit =
                Physics.Raycast(
                    ray: ray,
                    maxDistance: distance,
                    layerMask: 1 << 8, //enemy
                    hitInfo: out RaycastHit hit);

            if (!enemyHit)
                return;

            this.probingTarget = hit.transform.GetComponent<Enemy>();
            this.Controller.Agent.SetDestination(hit.point);

            UpdateManager.Instance.GlobalUpdate += Probe;

            void Probe()
            {
                var sqrDistance = (hit.transform.position - transform.position).sqrMagnitude;
                var probeTrheshold = 1f;
                var canProbe = sqrDistance < Mathf.Pow(probeTrheshold, 2);
                if (canProbe) {
                    var enemy = hit.transform.GetComponent<Enemy>();
                    this.Inventory.Add(enemy.Inventory);
                    UpdateManager.Instance.GlobalUpdate -= Probe;
                }
            }
        }

    }
}
