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

        public void BuildMesh()
        {
            Mesh viewMesh = new Mesh();
            viewMesh.vertices = this.GetVertices();
            viewMesh.triangles = this.GetTriangles(viewMesh.vertices);
            viewMesh.RecalculateNormals();

            this.fov.meshFilter.mesh = viewMesh;
        }


        private int viewSteps = 30;
        private Vector3[] GetVertices()
        {
            // num of vertices -1 plus the origin point, hence +1
            var anglePerStep = this.fov.ViewAngle / this.viewSteps;
            var playerPos = fov.transform.position;
            var currentAngle = -anglePerStep * (this.viewSteps / 2);

            Vector3[] vertices = new Vector3[(this.viewSteps - 1) + 1];
            vertices[0] = fov.transform.InverseTransformPoint(playerPos);

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

                vertices[i + 1] = fov.transform.InverseTransformPoint(pointHit);
                currentAngle += anglePerStep;
            }

            return vertices;
        }

        private int[] GetTriangles(Vector3[] vertices)
        {
            var vertexCount = vertices.Count();
            var numTriangles = vertexCount - 2;
            int[] triangles = new int[numTriangles * 3];
            
            for (int i = 0; i < numTriangles; i++) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            return triangles;
        }
    }
}