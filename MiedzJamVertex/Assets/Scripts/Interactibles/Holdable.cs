using RoboMed.Control;
using System;
using UnityEngine;

namespace RoboMed.Interactibles
{
    [RequireComponent(typeof(IInteractible))]
    [RequireComponent(typeof(Rigidbody))]
    public class Holdable : MonoBehaviour, IHoldable
    {
        public event Action onHeld;
        public event Action onReleased;

        public MouseFollower Hand { get; set; }

        public Quaternion HoldingRotation { get; private set; }


        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;

            onHeld?.Invoke();
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;

            onReleased?.Invoke();
        }

        private void Awake()
        {
            // Rotacja trzymania jest początkową
            HoldingRotation = transform.rotation;
        }
    }
}
