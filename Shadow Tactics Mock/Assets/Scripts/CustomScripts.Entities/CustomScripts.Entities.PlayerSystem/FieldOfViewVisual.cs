using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomScripts.Entities.PlayerSystem
{
    public class FieldOfViewVisual
    {
        private FieldOfView fov;
        public FieldOfViewVisual(FieldOfView fov)
        {
            this.fov = fov;
        }


        private int viewSteps = 30;
        private Vector3[] vertices;
        private void CastRays()
        {
            // num of vertices -1 plus the origin point, hence +1
            var anglePerStep = this.fov.ViewAngle / this.viewSteps;
            var playerPos = fov.transform.position;
            var currentAngle = -anglePerStep * (this.viewSteps / 2);

            this.vertices = new Vector3[(this.viewSteps - 1) + 1];
            this.vertices[0] = playerPos;

            for (int i = 0; i < this.viewSteps - 1; i++) {
                var dir = fov.GetVectorFromAngle(currentAngle, isGlobalAngle: false);
                var didHitSomething = Physics.Raycast(
                        origin: playerPos,
                        direction: dir,
                        maxDistance: fov.ViewRadius,
                        hitInfo: out RaycastHit hit
                        );

                var pointHit =
                    didHitSomething ?
                        hit.point :
                        playerPos + dir * fov.ViewRadius;

                this.vertices[i + 1] = pointHit;
                currentAngle += anglePerStep;
            }
        }

        public Vector3[] GetVertices()
        {
            this.CastRays();
            return this.vertices;
        }
    }
}
