using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Control
{
    public class MouseController : MonoBehaviour
    {
        private IInteractible currentInteractible = null;

        // Update is called once per frame
        void Update()
        {
            UpdateInteractible();

            if (Input.GetMouseButtonDown(0))
            {
                currentInteractible?.Interact();
            }
        }

        private void UpdateInteractible()
        {
            IInteractible pointedInteractible = GetPointedInteractible();

            if (currentInteractible != pointedInteractible)
            {
                // Inform the previous object
                currentInteractible?.QuitAvailability();
                // Inform the current object
                pointedInteractible?.EnterAvailability();
            }

            currentInteractible = pointedInteractible;
        }

        /// <summary>
        /// Returns the closest interactible pointed by mouse
        /// </summary>
        private IInteractible GetPointedInteractible()
        {
            RaycastHit[] raycast = RaycastMouse();
            if (raycast.Length == 0)
                return null;

            // Find the first interactible
            foreach(RaycastHit hit in raycast)
            {
                if(hit.collider.TryGetComponent(out IInteractible interactible))
                {
                    return interactible;
                }
            }

            return null;
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