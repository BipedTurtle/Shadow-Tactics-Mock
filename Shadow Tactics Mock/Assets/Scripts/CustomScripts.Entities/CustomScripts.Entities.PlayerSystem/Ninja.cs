using CustomScripts.Entities.EnemySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Ninja : Player
    {
        [SerializeField] private Shuriken _shuriken;
        public Shuriken Shuriken => this._shuriken;
        protected override void Start()
        {
            base.Start();

            base.Inventory.Add(this.Shuriken);
        }

        protected override void ChooseSkill()
        {
            if (Input.GetKeyDown(KeyCode.A))
                base.skill = new BasicAttack(this);
            else if (Input.GetKeyDown(KeyCode.Q))
                base.skill = new ShurikenBlast(this);
            else if (Input.GetMouseButtonDown(1)) {
                base.skill = new NoSkill(this);
                base.ImplementSkill(null);
            }
        }
    }
}
