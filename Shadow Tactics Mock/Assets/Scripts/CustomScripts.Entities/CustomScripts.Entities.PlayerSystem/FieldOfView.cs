using System.Collections;
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

            UpdateManager.Instance.GlobalUpdate += this.CheckEnemies;
            UpdateManager.Instance.GlobalLateUpdate += this.VisualizeFOV;
        }


        private void VisualizeFOV() => this.visual.BuildMesh();


        public Vector3 GetVectorFromAngle(float angleInDeg, bool isGlobalAngle)
        {
            if (!isGlobalAngle)
                angleInDeg += transform.rotation.eulerAngles.y;

            var angleInRad = angleInDeg * Mathf.Deg2Rad;
            var x = Mathf.Sin(angleInRad);
            var z = Mathf.Cos(angleInRad);

            return new Vector3(x, 0, z);
        }


        private void CheckEnemies()
        {
            foreach (var enemy in Enemy.Enemies) {
                Vector3 playerToEnemy = enemy.Position - transform.position;

                var squaredDistance = playerToEnemy.sqrMagnitude;
                var isWithinPossibleView = squaredDistance <= Mathf.Pow(this.ViewRadius, 2);
                if (!isWithinPossibleView) {
                    enemy.Mark(isWithinView: false);
                    continue;
                }

                var halfAngle = this.ViewAngle / 2;
                var angleBetween = Vector3.Angle(transform.forward, playerToEnemy);
                var isWithinView = angleBetween <= halfAngle;
                enemy.Mark(isWithinView);
            }
        }
    }
}