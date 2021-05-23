using System;
using UnityEngine;
using RoboMed.Interactibles;
using RoboMed.Control.InteractionHandlers;

namespace RoboMed.Control
{
    public class MouseController : MonoBehaviour
    {
        public GameObject CurrentInteractible { get; private set; } = null;

        private IInteractionHandler[] handlers;


        /// <summary>
        /// Returns the closest GameObject with available interactible component pointed by mouse
        /// </summary>
        public GameObject GetPointedInteractible()
        {
            RaycastHit[] raycast = RaycastMouse();
            if (raycast.Length == 0)
                return null;

            foreach(var hit in raycast)
            {
                Collider collider = hit.collider;
                if(collider.TryGetComponent(out IInteractible interactible) && interactible.CanInteract && CanInteractWith(collider.gameObject))
                {
                    return collider.gameObject;
                }
            }

            return null;
        }

        public bool TryGetPointedPoint(out Vector3 point, float maxDistance)
        {
            point = Vector3.zero;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject pointedObject = GetPointedInteractible();
            bool pointsInteractible = pointedObject != null && pointedObject.TryGetComponent(out IInteractible interactible) && interactible.CanInteract;
            if (!pointsInteractible)
                return false;

            // Uzyskanie wskazywanego punktu na obiekcie
            pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);
            point = hit.point;
            return true;
        }

        private void Awake()
        {
            handlers = GetComponents<IInteractionHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateInteractible();
            if (CurrentInteractible == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                InteractOneTime();
            }
            if (Input.GetMouseButton(0))
            {  // Przycisk trzymany
                InteractContinuous();
            }
        }

        private void InteractOneTime()
        {
            bool handled = false;

            foreach (var handler in handlers)
            {
                handler.InteractWith(CurrentInteractible);
                handled = true;
            }

            if (handled)
            {
                CurrentInteractible.GetComponent<IInteractible>().Interact();
            }
        }

        private void InteractContinuous()
        {
            bool handled = false;

            foreach (var handler in handlers)
            {
                handler.ContinueInteraction(CurrentInteractible);
                handled = true;
            }

            if (handled)
            {
                CurrentInteractible.GetComponent<IInteractible>().Interact();
            }
        }

        private void UpdateInteractible()
        {
            GameObject pointedInteractible = GetPointedInteractible();

            if (CurrentInteractible != pointedInteractible)
            {
                // Inform the previous object
                if (CurrentInteractible != null) 
                    CurrentInteractible.GetComponent<IInteractible>().QuitAvailability();

                // Inform the current object
                if(pointedInteractible != null)
                    pointedInteractible.GetComponent<IInteractible>().EnterAvailability();

                // Inform all the interaction handlers
                foreach (var handler in handlers)
                {
                    handler.OnAvailableInteractibleChanged(pointedInteractible);
                }
            }

            CurrentInteractible = pointedInteractible;
        }

        private bool CanInteractWith(GameObject interactible)
        {
            if(TryGetComponent(out ItemHolder itemHolder) && itemHolder.HeldObject != null)
            {
                // Jeœli trzymamy coœ w rêku, interakcje pochodz¹ tylko z trzymanego przedmiotu
                return itemHolder.CanInteractWith(interactible);
            }

            foreach(var handler in handlers)
            {
                if (handler.CanInteractWith(interactible))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Performs raycast using direction pointed by mouse
        /// </summary>
        /// <returns>Array of hits sorted by ascending distance</returns>
        private RaycastHit[] RaycastMouse()
        {
            Ray ray = GetMouseRay();
            RaycastHit[] hits = Physics.RaycastAll(ray);

            Array.Sort(hits, (RaycastHit left, RaycastHit right) => left.distance.CompareTo(right.distance));

            return hits;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}