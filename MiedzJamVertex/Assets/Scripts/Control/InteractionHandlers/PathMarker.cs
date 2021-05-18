using RoboMed.Interactibles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Control.InteractionHandlers
{
    public class PathMarker : MonoBehaviour, IInteractionHandler, IHoldable
    {
        [SerializeField] float maxDistance = 50f;
        [SerializeField] Transform wandEnd;

        private GameObject pointedObject;
        private Quaternion startingRotation; // obrót sprzed wskazywania
        private float startingDistanceFromPointed;

        private float length; // odległość do czubka markera

        public InteractionFrequency InteractionFrequency => InteractionFrequency.Continuous;

        public MouseFollower Hand { get; set; }

        public bool CanInteractWith(GameObject interactible) => interactible != null && interactible.GetComponent<DrawablePlane>() != null;

        public void InteractWith(GameObject interactible)
        {
            // Malowanie ścieżki
        }

        public void OnAvailableInteractibleChanged(GameObject newInteractible)
        {
            if (CanInteractWith(newInteractible))
            {
                pointedObject = newInteractible;

                startingDistanceFromPointed = Hand.DistanceFromPointed;
                Hand.DistanceFromPointed = length;
            }
            else
            {
                // Reset
                if(pointedObject != null)
                {
                    Hand.DistanceFromPointed = startingDistanceFromPointed;
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

        private void Update()
        {
            if (pointedObject != null)
            {
                PointAt(pointedObject);
            }
        }

        private void PointAt(GameObject target)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool raycastSuccess = target.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
            if (!raycastSuccess)
            {
                transform.rotation = startingRotation;
                return;
            }

            transform.LookAt(hit.point);
            transform.Rotate(90f, 0, 0);
        }

        private void GetPointedPoint(GameObject target)
        {

        }

        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;

            OnAvailableInteractibleChanged(null);
        }

        private void OnDrawGizmos()
        {
            if(pointedObject != null)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                bool raycastSuccess = pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
                if (!raycastSuccess)
                    return;

                Gizmos.color = new Color(255, 128, 0);
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }
    }
}
