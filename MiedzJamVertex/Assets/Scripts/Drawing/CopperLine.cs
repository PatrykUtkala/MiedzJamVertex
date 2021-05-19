using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Drawing
{
    [RequireComponent(typeof(MeshFilter))]
    public class CopperLine : MonoBehaviour
    {
        public static CopperLine Main { get; private set; }

        [SerializeField] float thickness = 0.2f;
        [SerializeField] float yLift = 0.1f;

        private MeshFilter meshFilter;

        private Vector3[] perpendiculars;

        public void Add(Stack<Vector3> line)
        {
            Mesh newMesh = LineToMesh(line);
            meshFilter.mesh = newMesh;

            // TODO: połączenie z istniejącymi liniami
        }

        private Mesh LineToMesh(Stack<Vector3> line)
        {
            if (line.Count == 1)
                return GetPoint(line.Peek());

            Vector3[] lineCopy = line.ToArray();
            Vector3[] vertices = new Vector3[line.Count * 2];

            perpendiculars = new Vector3[line.Count];
            int i = 0;
            foreach (Vector3 point in line)
            {
                Vector3 perpendicular;
                if (i == 0)
                {
                    perpendicular = lineCopy[i + 1] - point;
                    perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                }
                else if (i == line.Count - 1)
                {
                    perpendicular = point - lineCopy[i - 1];
                    perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                }
                else
                {
                    perpendicular = (lineCopy[i - 1] - point).normalized + (lineCopy[i + 1] - point).normalized;
                    if (perpendicular == Vector3.zero)
                    {
                        // Przypadek, gdy punkty leżą w jednej linii prostej
                        perpendicular = point - lineCopy[i - 1];
                        perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * perpendicular;
                    }
                    // Odwracanie prostopadłych linii w jedną stronę
                    Vector3 tangent = -lineCopy[i - 1] + lineCopy[i + 1]; // styczna w punkcie łamanej
                    if (Vector3.Cross(tangent, perpendicular).y < 0)
                    {
                        perpendicular = -perpendicular;
                    }
                }
                perpendicular.Normalize();


                perpendiculars[i] = perpendicular;

                vertices[2 * i] = point + perpendicular * thickness + Vector3.up * yLift;
                vertices[2 * i + 1] = point - perpendicular * thickness + Vector3.up * yLift;

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

        private Mesh GetPoint(Vector3 point)
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
            if(Main == null)
            {
                Main = this;
            }

            meshFilter = GetComponent<MeshFilter>();
        }
    }
}
