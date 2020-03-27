using System;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Utility;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    public class SetTrap : IPlayerSkill
    {
        public int Damage => throw new NotImplementedException();

        private Thief player;
        public SetTrap(Thief player)
        {
            this.player = player;

            GameManager.Instance.CancelSkill += this.Cancel;
        }

        public IPlayerSkill Implement(Enemy target, ActionType actionType)
        {
            bool typeMismatch = (actionType != ActionType.Install);
            if (typeMismatch)
                return new NoSkill(this.player);

            var mask = 1 << 9; // Ground layer
            this.installSpot = MainCamera.Instance.GetPointFromCursor(mask);
            var hasTrap = this.player.Inventory.Contains<Trap>();
            if (hasTrap)
                UpdateManager.Instance.GlobalUpdate += this.InstallTrap;

            return new NoSkill(this.player);
        }

        private Vector3 installSpot = Vector3.zero;
        public void InstallTrap()
        {
            var trap = this.player.Inventory.Get<Trap>();
            if (installSpot == Vector3.zero || trap == null) {
                UpdateManager.Instance.GlobalUpdate -= this.InstallTrap;
                return;
            }

            MoveToInstallSpot();

            var distance = Vector3.Distance(this.installSpot.Flatten(), this.player.Position.Flatten());
            var threshold = .1f;
            var canInstall = distance < threshold;
            if (canInstall) {
                trap.InstallAt(this.installSpot);
                this.player.Inventory.Remove(this.player.Trap);
                UpdateManager.Instance.GlobalUpdate -= this.InstallTrap;
            }


            void MoveToInstallSpot()
            {
                var agent = this.player.Controller.Agent;
                agent.SetDestination(this.installSpot);
            }
        }

        public void Cancel()
        {
            UpdateManager.Instance.GlobalUpdate -= this.InstallTrap;
            var agent = this.player.Controller.Agent;
            agent.ResetPath();
        }
    }
}
