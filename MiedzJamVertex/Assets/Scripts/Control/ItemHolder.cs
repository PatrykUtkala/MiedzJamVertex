using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RoboMed.Interactibles;

namespace RoboMed.Control
{
    public class ItemHolder : MonoBehaviour, IInteractionHandler
    {
        [Tooltip("Miejsce, w którym jest trzymany obiekt")]
        [SerializeField] Transform holdPoint;

        // Informacje o trzymanym obiekcie
        private GameObject heldObject = null;
        private Transform startingParent;
        private Vector3 startingPosition;

        public void Interact(GameObject interactible)
        {
            if(heldObject == null)
            {
                // Podniesienie obiektu
                if(interactible.GetComponent<IHeldObject>() != null)
                {
                    SetHeldObject(interactible);
                }
            }
            else if(heldObject != interactible)
            {
                // Sprawdzenie, czy można użyć trzymanego obiektu na innym
                if(interactible.TryGetComponent(out IObjectCan objectCan))
                {
                    if (objectCan.Dispose(heldObject))
                    {
                        heldObject = null;
                    }
                }
            }
        }

        private void SetHeldObject(GameObject go)
        {
            heldObject = go;
            // Zapisanie poprzedniego stanu
            startingParent = go.transform.parent;
            startingPosition = go.transform.position;

            // Przeniesienie do ręki
            heldObject.transform.parent = holdPoint;
            heldObject.transform.position = holdPoint.position;

            heldObject.GetComponent<IHeldObject>().OnHeld();
        }

        private void ReleaseObject()
        {
            if (heldObject == null)
                return;

            heldObject.transform.parent = startingParent;
            heldObject.transform.position = startingPosition;

            heldObject.GetComponent<IHeldObject>().OnReleased();
            heldObject = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                ReleaseObject();
            }
        }
    }
}
