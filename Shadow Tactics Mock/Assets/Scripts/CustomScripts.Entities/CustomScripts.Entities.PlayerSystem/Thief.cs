using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    public class Thief : Player
    {
        [SerializeField] private Trap _trap;
        public Trap Trap { get => _trap; }
        protected override void Start()
        {
            base.Start();
            base.Inventory.Add(this._trap);

            UpdateManager.Instance.GlobalUpdate += this.ChooseSkill;
        }

        protected override void ChooseSkill()
        {
            if (Input.GetKeyDown(KeyCode.A))
                base.skill = new BasicAttack(this);
            else if (Input.GetKeyDown(KeyCode.Q))
                base.skill = new SetTrap(this);
            else if (Input.GetMouseButtonDown(1)) {
                base.skill = new NoSkill(this);
                base.ImplementSkill(null, ActionType.None);
            }
        }
    }
}
