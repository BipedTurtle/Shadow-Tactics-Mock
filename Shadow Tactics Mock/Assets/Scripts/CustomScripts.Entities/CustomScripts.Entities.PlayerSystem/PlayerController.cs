using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        void Start()
        {
            this.mainCamera = Camera.main;
            this.Agent = GetComponent<NavMeshAgent>();

            UpdateManager.Instance.GlobalUpdate += this.LookAtMousePointer;
            UpdateManager.Instance.GlobalUpdate += this.MoveToClickPoint;
        }

        public void Lock()
        {
            UpdateManager.Instance.GlobalUpdate -= this.LookAtMousePointer;
            UpdateManager.Instance.GlobalUpdate -= this.MoveToClickPoint;
        }

        public void UnLock()
        {
            UpdateManager.Instance.GlobalUpdate += this.LookAtMousePointer;
            UpdateManager.Instance.GlobalUpdate += this.MoveToClickPoint;
        }

        private Camera mainCamera;
        private void LookAtMousePointer()
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y);
            mousePos = this.mainCamera.ScreenToWorldPoint(mousePos);
            var lookAtPos = mousePos + Vector3.up * transform.position.y;
            transform.LookAt(lookAtPos);
        }
        
        public NavMeshAgent Agent { get; private set; }
        private void MoveToClickPoint()
        {
            Ray fromCameraRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
            bool groundHit =
                Physics.Raycast(
                    ray: fromCameraRay,
                    maxDistance: 30f,
                    hitInfo: out RaycastHit hit
                    );
            bool mouseClicked = Input.GetMouseButtonDown(1);

            if (groundHit && mouseClicked) {
                var destination = hit.point;
                this.Agent.SetDestination(destination);
            }
        }
    }
}
