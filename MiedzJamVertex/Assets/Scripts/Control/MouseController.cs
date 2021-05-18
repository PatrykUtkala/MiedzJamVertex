using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboMed.Interactibles;

namespace RoboMed.Control
{
    public class MouseController : MonoBehaviour
    {
        public GameObject CurrentInteractible { get; private set; } = null;


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
                if(collider.TryGetComponent(out IInteractible interactible) && interactible.CanInteract)
                {
                    return collider.gameObject;
                }
            }

            return null;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateInteractible();

            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentInteractible == null)
                    return;

                CurrentInteractible.GetComponent<IInteractible>().Interact();

                foreach (var handler in GetComponents<IInteractionHandler>())
                {
                    handler.Interact(CurrentInteractible);
                }
            }
        }

        private void UpdateInteractible()
        {
            GameObject pointedObject = GetPointedInteractible();
            // Only interactible objects
            bool interactiblePresent = pointedObject != null && pointedObject.TryGetComponent(out IInteractible interactible) && interactible.CanInteract;
            GameObject pointedInteractible = interactiblePresent ? pointedObject : null;

            if (CurrentInteractible != pointedInteractible)
            {
                // Inform the previous object
                if (CurrentInteractible != null)
                    CurrentInteractible.GetComponent<IInteractible>().QuitAvailability();

                // Inform the current object
                if(pointedInteractible != null)
                    pointedInteractible.GetComponent<IInteractible>().EnterAvailability();
            }

            CurrentInteractible = pointedInteractible;
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