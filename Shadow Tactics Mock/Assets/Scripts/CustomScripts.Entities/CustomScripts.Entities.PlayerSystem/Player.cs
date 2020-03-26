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
    public class Player : MonoBehaviour
    {
        public static List<Player> players { get; } = new List<Player>();
        public Vector3 Position => transform.position;
        public Animator Animator { get; private set; }
        public PlayerController Controller { get; private set; }
        [SerializeField] private Shuriken _shuriken;
        public Shuriken Shuriken { get => this._shuriken; }

        private void Start()
        {
            players.Add(this);
            this.Animator = GetComponent<Animator>();
            this.Controller = GetComponent<PlayerController>();
            this.skill = new NoSkill(this);

            UpdateManager.Instance.GlobalUpdate += this.ChooseSkill;
            GameManager.Instance.ImplementAttack += this.ImplementSkill;
        }

        private IPlayerSkill skill;
        private void ChooseSkill()
        {
            if (Input.GetKeyDown(KeyCode.A))
                this.skill = new BasicAttack(this);
            else if (Input.GetKeyDown(KeyCode.Q))
                this.skill = new ShurikenBlast(this);
            else if (Input.GetMouseButtonDown(1)) {
                this.skill = new NoSkill(this);
                this.ImplementSkill(null);
            }
        }

        private void ImplementSkill(Enemy target)
        {
            this.skill.Implement(target);
        }
    }
}
