using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.CreatorUtilities
{
    public class GridInstantiator : MonoBehaviour
    {
        [Tooltip("Obiekt do stworzenia")]
        [SerializeField] GameObject targetPrefab;
        [Tooltip("Rozmiar prostok¹ta miêdzy wêz³ami siatki")]
        [SerializeField] Vector2 tileSize = new Vector2(1, 1);
        [SerializeField] Vector2Int repetitions = new Vector2Int(2, 2);

        public Vector3 TotalSize
        {
            get
            {
                return new Vector3(
                    tileSize.x * (repetitions.x - 1),
                    0,
                    tileSize.y * (repetitions.y - 1)
                    );
            }
        }

        public Vector3 UpperLeftCorner => transform.position - TotalSize / 2;

        public void InstantiateGrid()
        {
            if (targetPrefab == null)
                return;

            for (int x = 0; x < repetitions.x; x++)
            {
                for (int y = 0; y < repetitions.y; y++)
                {
                    Vector3 nodePosition = GridNodePosition(x, y);
                    GameObject instance = Instantiate(targetPrefab, nodePosition, Quaternion.identity, transform);
                    instance.name = $"{targetPrefab.name} ({x}, {y})";
                }
            }
        }

        private Vector3 GridNodePosition(int x, int y)
        {
            return UpperLeftCorner
                + tileSize.x * x * Vector3.right
                + tileSize.y * y * Vector3.forward;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            for (int x = 0; x < repetitions.x; x++)
            {
                for (int y = 0; y < repetitions.y; y++)
                {
                    Vector3 nodePosition = GridNodePosition(x, y);
                    Gizmos.DrawSphere(nodePosition, 0.1f);
                }
            }
        }
    }

}