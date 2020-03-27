using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Entities.PlayerSystem;

namespace CustomScripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private Camera mainCamera;
        private void Start()
        {
            this.mainCamera = Camera.main;

            //DO NOT CHANGE THE ORDER
            UpdateManager.Instance.GlobalUpdate += this.SignalAttack;
            UpdateManager.Instance.GlobalUpdate += this.ToggleAttack;
        }

        private bool isActionReady;
        private void ToggleAttack()
        {
            if (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.Q))
                this.isActionReady = true;
            else if (Input.GetMouseButtonDown(0))
                this.isActionReady = false;
        }

        private void SignalAttack()
        {
            if (!isActionReady)
                return;

            var shouldHoldAttack = !Input.GetMouseButtonDown(0);
            if (shouldHoldAttack)
                return;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var distance = mainCamera.transform.position.y * 2;
            var mask = 1 << 8; // Enemy layer
            var isHit =
                Physics.Raycast(
                    ray: ray,
                    maxDistance: distance,
                    hitInfo: out RaycastHit hit,
                    layerMask: mask);

            if (isHit) {
                var enemyHit = hit.transform.GetComponent<Enemy>();
                this.OnActionImplemented(enemyHit, ActionType.Attack);
            }
            else
                this.OnActionImplemented(null, ActionType.Install);
        }

        public event Action<Enemy, ActionType> ImplementAction;
        private void OnActionImplemented(Enemy enemy, ActionType type)
        {
            this.ImplementAction?.Invoke(enemy, type);
        }
    }
}

namespace CustomScripts.Entities.PlayerSystem
{
    public enum ActionType
    {
        None,
        Attack,
        Install
    }
}