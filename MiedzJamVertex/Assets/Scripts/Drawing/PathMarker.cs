using RoboMed.Control.InteractionHandlers;
using RoboMed.Interactibles;
using RoboMed.Puzzle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace RoboMed.Drawing
{
    [RequireComponent(typeof(IHoldable))]
    public class PathMarker : MonoBehaviour, IInteractionHandler
    {
        public static PathMarker Main { get; private set; }
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
            if (!TryGetPointedPoint(out Vector3 pointedPoint))
                return;  // nie jest wskazywany punkt na colliderze pochodzącym z pointedObject

            StartNewLine(pointedPoint);
            drewThisFrame = true;
            isDrawing = true;
        }

        public void ContinueInteraction(GameObject interactible)
        {
            if (isDrawing && TryGetPointedPoint(out Vector3 pointedPoint))
            {
                ContinueLine(pointedPoint);
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
            }
        }

        private void Awake()
        {
            if(Main == null)
            {
                Main = this;
            }

            startingRotation = transform.rotation;

            length = Vector3.Distance(transform.position, wandEnd.position);

            onFinishLine += LineValidator.OnFinishLine;
        }

        private void OnDestroy()
        {
            if(Main == this)
            {
                Main = null;
            }
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
            if (pointedObject != null && TryGetPointedPoint(out Vector3 pointedPoint))
            {
                PointAt(pointedPoint);
            }
            else
            {
                // Nie jest wskazywany poprawny punkt
                transform.rotation = startingRotation;
            }
        }

        private void LateUpdate()
        {
            if (isDrawing && !drewThisFrame)
            {
                if(currPoints.Count > 0)
                {
                    Solidify();
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

            pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
            return hit.point;
        }

        private bool TryGetPointedPoint(out Vector3 point)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycastSuccess = pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
            point = hit.point;

            return raycastSuccess;
        }

        private void Solidify()
        {
            if (currPoints.Count == 0)
                return;

            if(CopperLine.Drawable == null)
            {
                Debug.LogError("Brak Drawable Circuit na scenie");
            }
            else
            {
                CopperLine.Drawable.AddOverwrite(currPoints);
            }
        }

        private void ResetAvailableInteractible()
        {
            OnAvailableInteractibleChanged(null);
        }


        /***************** Tworzenie ścieżki *******************/
        private void StartNewLine(Vector3 point)
        {
            // Reset
            currPoints = new Stack<Vector3>();
            // Dodanie pierwszego punktu
            currPoints.Push(point);

            // Raport
            Solidify();
        }
        private void ContinueLine(Vector3 point)
        {
            if (Vector3.Distance(point, currPoints.Peek()) >= resolution)
            {
                // Punkt kontrolny (odległy od poprzedniego o rozdzielczość)
                currPoints.Push(point);

                Solidify();
            }
        }

        private void OnDrawGizmos()
        {
            if (currPoints == null)
                return;

            Gizmos.color = Color.yellow;
            GizmosDrawLine(currPoints);
        }

        public static void GizmosDrawLine(Stack<Vector3> line)
        {
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
