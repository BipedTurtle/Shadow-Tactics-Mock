using System;
using UnityEngine;
using UnityEditor;
using CustomScripts.Entities.EnemySystem;

namespace CustomScripts.Entities.PlayerSystem
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            var fov = (FieldOfView)target;

            this.DrawPossibleFieldOfView(fov);
            this.DrawActualFieldOfView(fov);
            this.IndicateEnemiesWithinView(fov);
        }


        private void DrawPossibleFieldOfView(FieldOfView fov)
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, fov.transform.forward, 360, fov.ViewRadius);
        }

        private void DrawActualFieldOfView(FieldOfView fov)
        {
            var halfAngle = fov.ViewAngle / 2;
            var viewLeftBound = fov.GetDirectionFromAngle(-halfAngle, isGlobalAngle: false);
            var viewRightBound = fov.GetDirectionFromAngle(halfAngle, isGlobalAngle: false);

            Handles.color = Color.white;
            var playerPos = fov.transform.position;
            Handles.DrawLine(playerPos, playerPos + viewLeftBound * fov.ViewRadius);
            Handles.DrawLine(playerPos, playerPos + viewRightBound * fov.ViewRadius);
        }

        private void IndicateEnemiesWithinView(FieldOfView fov)
        {
            var player = fov.transform;

            foreach (var enemy in Enemy.Enemies) {
                if (!enemy.IsWithinPlayerView)
                    continue;

                Handles.color = Color.blue;
                Handles.DrawLine(enemy.Position, player.position);
            }
        }
    }
}
