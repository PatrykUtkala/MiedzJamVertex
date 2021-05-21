using UnityEngine;
using RoboMed.Interactibles;
using RoboMed.ItemMovement;
using System;

namespace RoboMed.Control.InteractionHandlers
{
    public class ItemHolder : MonoBehaviour, IInteractionHandler
    {
        [Tooltip("Miejsce, w którym jest trzymany obiekt")]
        [SerializeField] Transform holdPoint;

        public event Action onHeld;
        public event Action onReleased;

        public GameObject HeldObject { get; private set; } = null;

        // Informacje o trzymanym obiekcie
        private Transform startingParent;
        private Vector3 startingPosition;

        public void InteractWith(GameObject interactible)
        {
            if(HeldObject == null)
            {
                // Podniesienie obiektu
                if(interactible.GetComponent<IHoldable>() != null)
                {
                    SetHeldObject(interactible);
                }
            }
            else if(HeldObject != interactible)
            {
                // Sprawdzenie, czy można użyć trzymanego obiektu na innym
                if(interactible.TryGetComponent(out IObjectCan objectCan) && objectCan.CanDispose(HeldObject))
                {
                    // Można umieścić
                    GameObject heldObject = HeldObject;
                    ReleaseObject();
                    objectCan.Dispose(heldObject);
                }
                else if(HeldObject.TryGetComponent(out IInteractionHandler handler) && handler.CanInteractWith(interactible))
                {
                    handler.InteractWith(interactible);
                }
            }
        }

        public void ContinueInteraction(GameObject interactible)
        {
            if (interactible == HeldObject)
                return;

            if(HeldObject != null && HeldObject.TryGetComponent(out IInteractionHandler handler) && handler.CanInteractWith(interactible))
            {
                handler.ContinueInteraction(interactible);
            }
        }

        private void SetHeldObject(GameObject go)
        {
            HeldObject = go;
            // Zapisanie poprzedniego stanu
            startingParent = go.transform.parent;
            startingPosition = go.transform.position;

            // Przeniesienie do ręki
            HeldObject.transform.parent = holdPoint;
            HeldObject.transform.position = holdPoint.position;
            HeldObject.transform.rotation = go.GetComponent<IHoldable>().HoldingRotation; // TODO: smooth-out
            HeldObject.GetComponent<IHoldable>().Hand = GetComponent<MouseFollower>();

            HeldObject.GetComponent<IHoldable>().OnHeld();

            onHeld?.Invoke();
        }

        private void ReleaseObject()
        {
            if (HeldObject == null)
                return;

            HeldObject.transform.parent = startingParent;

            HeldObject.GetComponent<IHoldable>().OnReleased();
            HeldObject = null;

            onReleased?.Invoke();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                ReleaseObject();
            }
        }

        public bool CanInteractWith(GameObject interactible)
        {
            if (HeldObject == null)
                return interactible.GetComponent<IHoldable>() != null; // możliwość podniesienia

            if (interactible.GetComponent<IThrowSpot>() != null)
                return true;
            if(HeldObject.TryGetComponent(out IInteractionHandler handler) && handler.CanInteractWith(interactible))
                return true;  // interakcja pochodząca z trzymanego przedmiotu

            return interactible.GetComponent<IObjectCan>() != null;
        }

        public void OnAvailableInteractibleChanged(GameObject newInteractible)
        {
            if (HeldObject != null && HeldObject.TryGetComponent(out IInteractionHandler handler))
            {
                handler.OnAvailableInteractibleChanged(newInteractible);
            }
        }

    }
}
