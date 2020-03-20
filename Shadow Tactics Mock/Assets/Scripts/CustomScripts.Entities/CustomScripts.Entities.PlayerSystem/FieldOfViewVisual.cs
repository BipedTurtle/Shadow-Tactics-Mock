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
            vertices[0] = playerPos;
            
            for (int i = 0; i < this.viewSteps - 1; i++) {
                var viewCast = this.ViewCast(currentAngle);

                var pointHit =
                    viewCast.Hit ?
                        viewCast.Point :
                        playerPos + viewCast.ViewDir;

                vertices[i + 1] = pointHit;
                currentAngle += anglePerStep;
            }

            return
                vertices.
                    Select(v => this.fov.transform.InverseTransformPoint(v)).
                    ToArray();
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

        private ViewCastInfo ViewCast(float angle)
        {
            var dir = this.fov.GetDirectionFromAngle(angle, isGlobalAngle: false);
            var playerPos = this.fov.transform.position;

            var castHit =
                Physics.Raycast(
                    origin: playerPos,
                    direction: dir,
                    maxDistance: this.fov.ViewRadius,
                    hitInfo: out RaycastHit hit) ;

            var distance = hit.distance == 0 ? this.fov.ViewRadius : hit.distance;
            return new ViewCastInfo(castHit, hit.point, dir, distance, angle);
        }
    }

    struct ViewCastInfo
    {
        public bool Hit { get; }
        public Vector3 Point { get; }
        public Vector3 Direction { get; }
        public float Distance { get; }
        public Vector3 ViewDir { get => Direction * Distance; }
        public float Angle { get; }


        public ViewCastInfo(bool hit, Vector3 point, Vector3 direction,float distance, float angle)
        {
            this.Hit = hit;
            this.Point = point;
            this.Direction = direction;
            this.Distance = distance;
            this.Angle = angle;
        }
    }
}