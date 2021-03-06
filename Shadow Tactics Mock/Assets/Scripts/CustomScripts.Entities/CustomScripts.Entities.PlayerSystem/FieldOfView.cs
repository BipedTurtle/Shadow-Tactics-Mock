﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomScripts.Entities.EnemySystem;
using CustomScripts.Managers;

namespace CustomScripts.Entities.PlayerSystem
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float _viewRadius;
        public float ViewRadius { get => this._viewRadius; }
        [SerializeField] private float _viewAngle;
        public float ViewAngle { get => this._viewAngle; }

        private FieldOfViewVisual visual;
        public MeshFilter meshFilter { get; private set; }
        private void Start()
        {
            this.visual = new FieldOfViewVisual(this);
            this.meshFilter = GetComponent<MeshFilter>();

            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            UpdateManager.Instance.GlobalLateUpdate += this.VisualizeFOV;
        }


        private void VisualizeFOV() => this.visual.BuildMesh();

        public void DeVisualizeFOV()
        {
            UpdateManager.Instance.GlobalLateUpdate -= this.VisualizeFOV;
            this.meshFilter.mesh = null;
        } 

        public Vector3 GetDirectionFromAngle(float angleInDeg, bool isGlobalAngle)
        {
            if (!isGlobalAngle)
                angleInDeg += transform.rotation.eulerAngles.y;

            var angleInRad = angleInDeg * Mathf.Deg2Rad;
            var x = Mathf.Sin(angleInRad);
            var z = Mathf.Cos(angleInRad);

            return new Vector3(x, 0, z);
        }

        public bool IsWithinView(Transform target)
        {
            Vector3 thisToTarget = target.position - transform.position;
            var sqrDistance = thisToTarget.sqrMagnitude;
            var isOutsideViewRadius = sqrDistance > Mathf.Pow(this.ViewRadius, 2);
            if (isOutsideViewRadius)
                return false;

            var angleBetween = Vector3.Angle(thisToTarget, transform.forward);
            var halfAngle = this.ViewAngle / 2;
            var isWithinView = angleBetween < halfAngle;
            return isWithinView;
        }
    }
}