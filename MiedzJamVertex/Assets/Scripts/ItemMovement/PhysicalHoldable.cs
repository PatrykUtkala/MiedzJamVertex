using RoboMed.Control;
using RoboMed.Interactibles;
using System;
using UnityEngine;
using RoboMed.ItemCollecting;

namespace RoboMed.ItemMovement
{
    /// <summary>
    /// Przedmiot trzymany, który podczas spadania zachowuje się w sposób fizyczny
    /// </summary>
    [RequireComponent(typeof(IInteractible))]
    [RequireComponent(typeof(Rotatable))]
    public class PhysicalHoldable : MonoBehaviour, IHoldable, ICollectible
    {
        private Vector3 startingPosition;
        private Quaternion startingRotation;

        public Quaternion HoldingRotation 
            => Quaternion.Euler(startingRotation.eulerAngles.x, GetComponent<Rotatable>().YRotation, startingRotation.eulerAngles.z);

        public MouseFollower Hand { get; set; }

        public bool CanCollect => GetComponent<Rigidbody>().useGravity;

        public ItemStand StartingPosition { get; set; }

        public event Action onHeld;
        public event Action onReleased;

        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;

            SetPhysical(false);

            onHeld?.Invoke();
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;

            SetPhysical(true);

            onReleased?.Invoke();
        }

        public void ResetTransform()
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;

            SetPhysical(false);
        }

        public void OnCollected()
        {
            SetPhysical(false);
        }

        private void SetPhysical(bool physical)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = physical;
            rb.isKinematic = !physical;
        }

        private void Awake()
        {
            startingPosition = transform.position;
            startingRotation = transform.rotation;
        }

        private void Start()
        {
            SetPhysical(false);
        }
    }
}
