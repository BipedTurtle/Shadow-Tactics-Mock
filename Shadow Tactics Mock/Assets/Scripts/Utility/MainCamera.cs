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

        public Vector3 GetPointFromCursor(LayerMask mask)
        {
            var ray = this.main.ScreenPointToRay(Input.mousePosition);
            var distance = this.main.transform.position.y * 2;
            var isHit =
                Physics.Raycast(
                    ray: ray,
                    maxDistance: distance,
                    layerMask: mask,
                    hitInfo: out RaycastHit hit);

            return isHit ? hit.point : Vector3.zero;
        }
    }
}
