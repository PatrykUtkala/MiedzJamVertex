using RoboMed.Control.InteractionHandlers;
using RoboMed.Interactibles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Drawing
{
    public class PathMarker : MonoBehaviour, IInteractionHandler
    {
        static float maxDistance = 50f; // maksymalna odległość od kamery

        [Tooltip("Odległość między kolejnymi punktami kontrolnymi")]
        [SerializeField] float resolution = 1f;
        [SerializeField] Transform wandEnd;

        public event Action<Stack<Vector3>> onFinishLine;

        private GameObject pointedObject;
        private Quaternion startingRotation; // obrót sprzed wskazywania
        private float startingDistanceFromPointed;

        private float length; // odległość do czubka markera

        private bool isDrawing = false;
        private bool drewThisFrame = false;
        private Stack<Vector3> currPoints = new Stack<Vector3>();

        public bool CanInteractWith(GameObject interactible) => interactible != null && interactible.GetComponent<DrawablePlane>() != null;

        public void InteractWith(GameObject interactible)
        {
            StartNewLine();
            drewThisFrame = true;
            isDrawing = true;
        }

        public void ContinueInteraction(GameObject interactible)
        {
            if (isDrawing)
            {
                ContinueLine();
                drewThisFrame = true;
            }
        }

        public void OnAvailableInteractibleChanged(GameObject newInteractible)
        {
            if (CanInteractWith(newInteractible))
            {
                pointedObject = newInteractible;

                IHoldable holdable = GetComponent<IHoldable>();
                startingDistanceFromPointed = holdable.Hand.DistanceFromPointed;
                holdable.Hand.DistanceFromPointed = length;
            }
            else
            {
                // Reset
                if(pointedObject != null)
                {
                    GetComponent<IHoldable>().Hand.DistanceFromPointed = startingDistanceFromPointed;
                }

                pointedObject = null;
                transform.rotation = startingRotation;
            }
        }

        private void Awake()
        {
            startingRotation = transform.rotation;

            length = Vector3.Distance(transform.position, wandEnd.position);
        }

        private void OnEnable()
        {
            GetComponent<IHoldable>().onReleased += ResetAvailableInteractible;
        }

        private void OnDisable()
        {
            GetComponent<IHoldable>().onReleased -= ResetAvailableInteractible;
        }

        private void Update()
        {
            if (pointedObject != null)
            {
                PointAt(GetPointedPoint());
            }
        }

        private void LateUpdate()
        {
            if (isDrawing && !drewThisFrame)
            {
                if(currPoints.Count > 0)
                {
                    CopperLine.Main.Add(currPoints);
                    onFinishLine?.Invoke(currPoints);
                }

                isDrawing = false;
            }

            drewThisFrame = false;
        }

        private void PointAt(Vector3 point)
        {
            transform.LookAt(point);
            transform.Rotate(90f, 0, 0);
        }

        private Vector3 GetPointedPoint()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycastSuccess = pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
            return hit.point;
        }

        private void ResetAvailableInteractible()
        {
            OnAvailableInteractibleChanged(null);
        }


        /***************** Tworzenie ścieżki *******************/
        private void StartNewLine()
        {
            // Reset
            currPoints = new Stack<Vector3>();
            // Dodanie pierwszego punktu
            currPoints.Push(GetPointedPoint());

            // TODO: raportowanie do solidyfikatora
        }
        private void ContinueLine()
        {
            Vector3 pointedPoint = GetPointedPoint();

            if (Vector3.Distance(pointedPoint, currPoints.Peek()) >= resolution)
            {
                // Punkt kontrolny (odległy od poprzedniego o rozdzielczość)
                currPoints.Push(pointedPoint);
            }
            // TODO: raportowanie do solidyfikatora
        }

        private void OnDrawGizmos()
        {
            if (currPoints == null)
                return;

            GizmosDrawLine(currPoints);
        }

        private void GizmosDrawLine(Stack<Vector3> line)
        {
            Gizmos.color = Color.yellow;

            int i = 0;
            Vector3 prev = Vector3.zero;

            foreach (Vector3 point in line)
            {
                Gizmos.DrawSphere(point, 0.1f);

                if (i >= 1)
                {
                    Gizmos.DrawLine(prev, point);
                }

                prev = point;
                i++;
            }
        }
    }
}
