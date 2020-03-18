using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Managers
{
    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }


        public event Action GlobalUpdate;
        public event Action GlobalFixedUpdate;
        public event Action GlobalLateUpdate;

        private void Update()
        {
            this.GlobalUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            this.GlobalFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            this.GlobalLateUpdate?.Invoke();
        }
    }
}
