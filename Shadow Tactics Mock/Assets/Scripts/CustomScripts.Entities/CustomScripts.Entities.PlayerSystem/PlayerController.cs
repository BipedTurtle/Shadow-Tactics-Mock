using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(FieldOfView))]
    public class PlayerController : MonoBehaviour
    {
        void Start()
        {
            this.mainCamera = Camera.main;

            UpdateManager.Instance.GlobalUpdate += this.LookAtMousePointer;
        }

        private Camera mainCamera;
        private void LookAtMousePointer()
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y);
            mousePos = this.mainCamera.ScreenToWorldPoint(mousePos);
            var lookAtPos = mousePos + Vector3.up * transform.position.y;
            transform.LookAt(lookAtPos);
        }
    }
}
