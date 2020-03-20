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


        private int viewSteps = 8;
        private Vector3[] GetVertices()
        {
            // num of vertices -1 plus the origin point, hence +1
            var anglePerStep = this.fov.ViewAngle / this.viewSteps;
            var playerPos = fov.transform.position;
            var currentAngle = -anglePerStep * (this.viewSteps / 2);

            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(playerPos);

            ViewCastInfo oldViewcast = new ViewCastInfo();
            for (int i = 0; i < this.viewSteps - 1; i++) {
                var currentViewCast = this.ViewCast(currentAngle);

                if (i > 0) {
                    if (oldViewcast.Hit != currentViewCast.Hit) {
                        var edges = this.FindEdges(minCast: oldViewcast, maxCast: currentViewCast);
                        if (edges.minEdge != Vector3.zero)
                            vertices.Add(edges.minEdge);
                        if (edges.maxEdge != Vector3.zero)
                            vertices.Add(edges.maxEdge);
                    }
                }

                var pointHit =
                    currentViewCast.Hit ?
                        currentViewCast.Point :
                        playerPos + currentViewCast.ViewDir;
                vertices.Add(pointHit);

                oldViewcast = currentViewCast;
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

        private int edgeSolverIterations = 10;
        private (Vector3 minEdge, Vector3 maxEdge) FindEdges(ViewCastInfo minCast, ViewCastInfo maxCast)
        {
            float minAngle = minCast.Angle;
            float maxAngle = maxCast.Angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeSolverIterations; i++) {
                var bisectionAngle = (minAngle + maxAngle) / 2;
                var bisectingCast = this.ViewCast(bisectionAngle);

                if (minCast.Hit == bisectingCast.Hit) {
                    minAngle = bisectingCast.Angle;
                    minPoint = bisectingCast.Point;
                }
                else {
                    maxAngle = bisectingCast.Angle;
                    maxPoint = bisectingCast.Point;
                }
            }

            return (minPoint, maxPoint);
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

            var point = castHit ? hit.point : playerPos + dir * this.fov.ViewRadius;
            var distance = hit.distance == 0 ? this.fov.ViewRadius : hit.distance;
            return new ViewCastInfo(castHit, point, dir, distance, angle);
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