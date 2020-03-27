using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Thief : Player
    {

        protected override void ChooseSkill()
        {
            if (Input.GetKeyDown(KeyCode.A))
                base.skill = new BasicAttack(this);
            else if (Input.GetKeyDown(KeyCode.Q))
                base.skill = new SetTrap(this);
        }
    }
}
