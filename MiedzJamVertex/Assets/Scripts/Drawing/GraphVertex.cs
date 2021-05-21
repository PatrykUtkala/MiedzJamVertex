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
        [SerializeField] List<GraphVertex> connections = new List<GraphVertex>();

        public bool isStart = false;

        /// <summary>
        /// Czy wierzchołek posiada więcej niż jedną krawędź wychodzącą
        /// </summary>
        public bool IsFork => connections != null && connections.Count > 1;

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
                bool isStraight = currentVertex.connections != null && currentVertex.connections.Count == 1;
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
        public List<Stack<GraphVertex>> GetAllLines()
        {
            List<Stack<GraphVertex>> lines = new List<Stack<GraphVertex>>();

            foreach(GraphVertex direction in connections)
            {
                if (direction == null)
                    continue;

                Stack<GraphVertex> line = new Stack<GraphVertex>();
                line.Push(this); // ten wierzchołek jako początkowy
                direction.LoadLongestLine(line);
                lines.Add(line);
            }

            return lines;
        }

        /// <summary>
        /// Tworzy krawędź do poprzedniego elementu w hierarchii
        /// </summary>
        public void LinkToPrevious()
        {
            int selfIndex = transform.GetSiblingIndex();
            if (selfIndex == 0)
                return;  // Pierwszy wierzchołek - brak poprzedniego

            if (transform.parent.GetChild(selfIndex - 1).TryGetComponent(out GraphVertex prevVertex))
            {
                // Łączenie
                if(connections.Count > 0 && connections[0] == null)
                {
                    connections[0] = prevVertex;
                }
                else
                {
                    connections.Add(prevVertex);
                }
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
    }
}
