using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Managers;
using CustomScripts.Entities.EnemySystem;

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
            GameManager.Instance.ImplementAttack += this.ImplementSkill;
        }

        protected IPlayerSkill skill;
        protected abstract void ChooseSkill();
        
        protected void ImplementSkill(Enemy target)
        {
            this.skill.Implement(target);
        }
    }
}
