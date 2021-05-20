using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.Drawing
{
    [ExecuteInEditMode]
    public class GraphVertex : MonoBehaviour
    {
        [Tooltip("Skierowane połączenia z tego punktu")]
        [SerializeField] GraphVertex[] connections;

        public bool isStart = false;

        /// <summary>
        /// Czy wierzchołek posiada więcej niż jedną krawędź wychodzącą
        /// </summary>
        public bool IsFork => connections != null && connections.Length > 1;

        private bool initialized = false;

        /// <summary>
        /// Zwraca najdłuższą ścieżkę bez rozwidleń z tego punktu
        /// </summary>
        /// <returns>Stos z najdalszym elementem na górze</returns>
        public void LoadLongestLine(Stack<GraphVertex> line)
        {
            GraphVertex currentVertex = this;
            while(true)
            {
                line.Push(currentVertex);
                // Kończymy, gdy napotkamy punkt końcowy lub rozwidlenie
                bool isStraight = currentVertex.connections != null && currentVertex.connections.Length == 1;
                if (!isStraight)
                    break;

                // Przejście dalej po linii bez rozwidleń
                currentVertex = currentVertex.connections[0];
                if (currentVertex == null)
                    break;
            }
        }

        /// <summary>
        /// Zwraca ścieżkę do następnego rozwidlenia w każdą stronę
        /// </summary>
        /// <returns></returns>
        public Stack<GraphVertex>[] GetAllLines()
        {
            Stack<GraphVertex>[] lines = new Stack<GraphVertex>[connections.Length];

            for(int i = 0; i < connections.Length; i++)
            {
                Stack<GraphVertex> line = new Stack<GraphVertex>();
                line.Push(this); // ten wierzchołek jako początkowy

                GraphVertex direction = connections[i];
                if (direction != null)
                {
                    direction.LoadLongestLine(line);
                }

                lines[i] = line;
            }

            return lines;
        }

        private void Awake()
        {
            if (Application.isPlaying)
                return;

            if (initialized)
                return;  // oh no, not again
            initialized = true;

            connections = new GraphVertex[1];

            // Domyślne połączenie z wcześniejszym wierzchołkiem
            int selfIndex = transform.GetSiblingIndex();
            if (selfIndex == 0)
                return;  // Pierwszy wierzchołek - brak poprzedniego

            if (transform.parent.GetChild(selfIndex - 1).TryGetComponent(out GraphVertex vertex))
            {
                // Łączenie
                connections[0] = vertex;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isStart ? Color.red : Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);

            Gizmos.color = Color.yellow;

            if (connections == null)
                return;

            foreach(GraphVertex target in connections)
            {
                if (target == null)
                    continue;

                Gizmos.DrawLine(transform.position, target.transform.position);
                // Strzałka
                Vector3 middle = (transform.position + target.transform.position) / 2;
                Vector3 arrowLeft = middle - Quaternion.Euler(0, 30f, 0) * (target.transform.position - transform.position).normalized * 0.2f;
                Gizmos.DrawLine(middle, arrowLeft);
                Vector3 arrowRight = middle - Quaternion.Euler(0, -30f, 0) * (target.transform.position - transform.position).normalized * 0.2f;
                Gizmos.DrawLine(middle, arrowRight);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            foreach(Stack<GraphVertex> line in GetAllLines())
            {
                Stack<Vector3> vertexPositions = new Stack<Vector3>();
                foreach (var vertex in line)
                {
                    if (vertex == null)
                        continue;

                    vertexPositions.Push(vertex.transform.position);
                }
                PathMarker.GizmosDrawLine(vertexPositions);
            }
        }
    }
}
