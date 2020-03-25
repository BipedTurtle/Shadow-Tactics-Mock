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
        public static NoSkill Instance { get; }
        static NoSkill()
        {
            if (Instance != null)
                return;

            Instance = new NoSkill();
        }

        public IPlayerSkill Implement(Enemy target) => this;

    }
}
