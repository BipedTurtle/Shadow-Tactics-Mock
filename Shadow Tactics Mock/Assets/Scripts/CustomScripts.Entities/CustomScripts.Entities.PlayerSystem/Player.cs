using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        public static List<Player> players { get; } = new List<Player>();
        public Vector3 Position => transform.position;

        private void Start()
        {
            players.Add(this);
        }
    }
}
