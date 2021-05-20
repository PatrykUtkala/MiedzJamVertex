using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.Drawing
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CopperLine : MonoBehaviour
    {
        public static CopperLine Drawable { get; private set; }

        [SerializeField] bool isDrawable;
        [SerializeField] bool instantiateFromChildren = false;
        [Header("Parametry linii")]
        [SerializeField] float thickness = 0.2f;
        [SerializeField] float yLift = 0.1f;

        protected MeshFilter meshFilter;

        private Vector3[] perpendiculars; // relikt testowania

        /// <summary>
        /// Tworzy mesha ścieżki na podstawie dzieci będących wierzchołkami grafu
        /// </summary>
        public void InstantianteFromChildren()
        {
            List<GraphVertex> keyVertices = new List<GraphVertex>();

            foreach(Transform child in transform)
            {
                if(child.TryGetComponent(out GraphVertex vertex)) {
                    if (vertex.IsFork || vertex.isStart)
                    {
                        keyVertices.Add(vertex);
                    }
                }
            }

            bool first = true;
            foreach(var vertex in keyVertices)
            {
                foreach(var line in vertex.GetAllLines())
                {
                    Stack<Vector3> ln = new Stack<Vector3>(line.Select(gv => gv.transform.position));
                    if (first)
                    {
                        AddOverwrite(ln);
                    }
                    else
                    {
                        Add(ln);
                    }

                    first = false;
                }
            }
        }

        /// <summary>
        /// Zastępuje poprzednie linie nową
        /// </summary>
        public void AddOverwrite(Stack<Vector3> line)
        {
            meshFilter.mesh = LineToMesh(line);
        }

        /// <summary>
        /// Łączy nową linię z istniejącymi
        /// </summary>
        public void Add(Stack<Vector3> line)
        {
            Mesh newMesh = LineToMesh(line);

            CombineInstance[] combine = new CombineInstance[1];
            combine[0].mesh = newMesh;

            meshFilter.mesh.CombineMeshes(combine);
        }

        protected Mesh LineToMesh(Stack<Vector3> line)
        {
            if (line.Count == 1)
                return GetPointMesh(line.Peek());

            Vector3[] localLine = line.ToArray();
            // Zamiana na lokalne punkty
            for(int j = 0; j < localLine.Length; j++)
            {
                localLine[j] = transform.InverseTransformPoint(localLine[j]);
            }

            Vector3[] vertices = new Vector3[line.Count * 2];

            perpendiculars = new Vector3[line.Count];
            int i = 0;
            foreach (Vector3 localPoint in localLine)
            {
                Vector3 perpendicular;
                if (i == 0)
                {
                    perpendicular = localLine[i + 1] - localPoint;
                    perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                }
                else if (i == line.Count - 1)
                {
                    perpendicular = localPoint - localLine[i - 1];
                    perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                }
                else
                {
                    perpendicular = (localLine[i - 1] - localPoint).normalized + (localLine[i + 1] - localPoint).normalized;
                    if (perpendicular == Vector3.zero)
                    {
                        // Przypadek, gdy punkty leżą w jednej linii prostej
                        perpendicular = localPoint - localLine[i - 1];
                        perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                    }
                    // Odwracanie prostopadłych linii w jedną stronę
                    Vector3 tangent = -localLine[i - 1] + localLine[i + 1]; // styczna w punkcie łamanej
                    if (Vector3.Cross(tangent, perpendicular).y < 0)
                    {
                        perpendicular = -perpendicular;
                    }
                }
                perpendicular.Normalize();


                perpendiculars[i] = perpendicular;

                vertices[2 * i] = localPoint + perpendicular * thickness + Vector3.up * yLift;
                vertices[2 * i + 1] = localPoint - perpendicular * thickness + Vector3.up * yLift;

                i++;
            }

            int[] triangles = new int[(line.Count - 1) * 6 * 2];
            for(int j = 0; j < line.Count - 1; j++)
            {
                triangles[6 * j] = 2*j;
                triangles[6 * j + 1] = 2 * (j + 1);
                triangles[6 * j + 2] = 2 * j + 1;

                triangles[6 * j + 3] = 2 * (j + 1);
                triangles[6 * j + 4] = 2 * (j + 1) + 1;
                triangles[6 * j + 5] = 2 * j + 1;
            }
            // Druga strona płaszczyzny
            int offset = (line.Count - 1) * 6;
            for (int j = 0; j < line.Count - 1; j++)
            {
                triangles[offset + 6 * j] = 2 * (j + 1);
                triangles[offset + 6 * j + 1] = 2 * j;
                triangles[offset + 6 * j + 2] = 2 * j + 1;

                triangles[offset + 6 * j + 3] = 2 * (j + 1) + 1;
                triangles[offset + 6 * j + 4] = 2 * (j + 1);
                triangles[offset + 6 * j + 5] = 2 * j + 1;
            }


            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }

        private Mesh GetPointMesh(Vector3 point)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            vertices[0] = point + Vector3.left * thickness / 2;
            vertices[1] = point + Vector3.forward * thickness / 2;
            vertices[2] = point + Vector3.right * thickness / 2;
            vertices[3] = point + Vector3.back * thickness / 2;

            // Podniesienie
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y += yLift;
            }


            int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }

        private void Awake()
        {
            if(Drawable == null && isDrawable)
            {
                Drawable = this;
            }

            meshFilter = GetComponent<MeshFilter>();

            if (instantiateFromChildren)
            {
                InstantianteFromChildren();
            }
        }
    }
}
