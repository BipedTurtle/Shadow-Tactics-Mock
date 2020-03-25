using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        public static List<Player> players { get; } = new List<Player>();
        public Vector3 Position => transform.position;
        private Animator animator;

        private void Start()
        {
            players.Add(this);
            this.animator = GetComponent<Animator>();

            GameManager.Instance.ImplementAttack += this.BasicAttack;
        }

        private void BasicAttack()
        {
            this.animator.SetTrigger("Basic Attack");
        }
    }
}
