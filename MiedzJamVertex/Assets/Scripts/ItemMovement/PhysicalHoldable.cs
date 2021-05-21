using RoboMed.Control;
using RoboMed.Interactibles;
using System;
using UnityEngine;
using RoboMed.ItemCollecting;
using RoboMed.Movement;

namespace RoboMed.ItemMovement
{
    /// <summary>
    /// Przedmiot trzymany, który podczas spadania zachowuje się w sposób fizyczny
    /// </summary>
    [RequireComponent(typeof(IInteractible))]
    [RequireComponent(typeof(Rotatable))]
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicalHoldable : MonoBehaviour, IHoldable, ICollectible
    {
        private Vector3 startingPosition;
        private Quaternion startingRotation;

        public Quaternion PresentingRotation 
            => Quaternion.Euler(startingRotation.eulerAngles.x, GetComponent<Rotatable>().YRotation, startingRotation.eulerAngles.z);

        public MouseFollower Hand { get; set; }
        public bool IsHeld { get; private set; }

        public bool CanCollect => GetComponent<Rigidbody>().useGravity;

        public ItemStand StartingPosition { get; set; }

        public event Action onHeld;
        public event Action onReleased;

        private bool worldRotation = false; // stwierdza, czy powinno się utrzymać rotację względem globalnego układu

        public void OnHeld()
        {
            IsHeld = true;

            GetComponent<IInteractible>().CanInteract = false;

            SetPhysical(false);

            onHeld?.Invoke();
        }

        public void OnReleased()
        {
            IsHeld = false;

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

        public void OnAvailableInteractibleChanged(GameObject newInteractible)
        {
            if(newInteractible == null)
            {
                // Pozycja względem ręki
                transform.localRotation = PresentingRotation;
                worldRotation = false;
            }
            else if(newInteractible.GetComponent<IObjectCan>() != null)
            {
                // Ustawienie w pozycji, w jakiej będzie wstawiony przedmiot
                worldRotation = true;
            }
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

        private void LateUpdate()
        {
            if (worldRotation)
            {
                transform.rotation = PresentingRotation;
            }
        }
    }
}
