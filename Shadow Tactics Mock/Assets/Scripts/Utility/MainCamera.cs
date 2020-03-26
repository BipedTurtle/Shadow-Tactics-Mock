using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Utility
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private Camera main;
        private void Start()
        {
            this.main = Camera.main;
        }

        public Vector3 FromScreenToWorld(Vector3 pos) => this.main.ScreenToWorldPoint(pos);

        public Ray FromScreenPointToRay(Vector3 pos) => this.main.ScreenPointToRay(pos);
    }
}
