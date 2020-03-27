using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomScripts.Entities.EnemySystem;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class SetTrap : IPlayerSkill
    {
        public int Damage => throw new NotImplementedException();

        private Player player;
        public SetTrap(Player player)
        {
            this.player = player;
        }

        public IPlayerSkill Implement(Enemy target, ActionType actionType)
        {
            bool typeMismatch = (actionType != ActionType.Install);
            if (typeMismatch)
                return new NoSkill(this.player);

            Debug.Log("set trap");

            return new NoSkill(this.player);
        }
    }
}
