using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomScripts.Entities.EnemySystem;

namespace CustomScripts.Entities.PlayerSystem
{
    public interface IPlayerSkill
    {
        int Damage { get; }
        IPlayerSkill Implement(Enemy target, ActionType actionType);
    }
}
