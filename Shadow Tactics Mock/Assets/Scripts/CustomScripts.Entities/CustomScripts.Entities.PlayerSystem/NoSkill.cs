using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomScripts.Entities.EnemySystem;

namespace CustomScripts.Entities.PlayerSystem
{
    public class NoSkill : IPlayerSkill
    {
        private Player player;
        public NoSkill(Player player)
        {
            this.player = player;
        }

        public int Damage => 0;
        public IPlayerSkill Implement(Enemy target, ActionType actionType)
        {
            this.player.Controller.Agent.ResetPath();
            this.player.StopAllCoroutines();
            this.player.Controller.UnLock();
            return this;
        }


        public void Cancel()
        {

        }
    }
}
