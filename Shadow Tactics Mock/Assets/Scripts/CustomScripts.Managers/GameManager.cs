using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;

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

        private bool isAttackReady;
        private void ToggleAttack()
        {
            if (Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.Q))
                this.isAttackReady = true;
            else if (Input.GetMouseButtonDown(0))
                this.isAttackReady = false;
        }

        private void SignalAttack()
        {
            if (!isAttackReady)
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
                this.OnAttackImplemented(enemyHit);
            }
        }

        public event Action<Enemy> ImplementAttack;
        private void OnAttackImplemented(Enemy enemy)
        {
            this.ImplementAttack?.Invoke(enemy);
        }
    }
}