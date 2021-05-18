using UnityEngine;
using RoboMed.Interactibles;

namespace RoboMed.Control.InteractionHandlers
{
    public class ItemHolder : MonoBehaviour, IInteractionHandler
    {
        [Tooltip("Miejsce, w którym jest trzymany obiekt")]
        [SerializeField] Transform holdPoint;

        public GameObject HeldObject { get; private set; } = null;

        public InteractionFrequency InteractionFrequency
        {
            get
            {
                if(HeldObject != null)
                {
                    if (HeldObject.TryGetComponent(out IInteractionHandler handler))
                        return handler.InteractionFrequency;

                    return InteractionFrequency.OneTime;
                }

                return InteractionFrequency.OneTime;
            }
        }

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
                if(interactible.TryGetComponent(out IObjectCan objectCan))
                {
                    if (objectCan.Dispose(HeldObject))
                    {  // objectCan odebrał trzymany obiekt
                        HeldObject = null;
                    }
                }
                else if(HeldObject.TryGetComponent(out IInteractionHandler handler) && handler.CanInteractWith(interactible))
                {
                    handler.InteractWith(interactible);
                }
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
            HeldObject.GetComponent<IHoldable>().Hand = GetComponent<MouseFollower>();

            HeldObject.GetComponent<IHoldable>().OnHeld();
        }

        private void ReleaseObject()
        {
            if (HeldObject == null)
                return;

            HeldObject.transform.parent = startingParent;
            HeldObject.transform.position = startingPosition;

            HeldObject.GetComponent<IHoldable>().OnReleased();
            HeldObject = null;
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
                return interactible.GetComponent<IHoldable>() != null;

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
