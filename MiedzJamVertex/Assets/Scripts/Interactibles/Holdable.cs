using RoboMed.Control;
using System;
using UnityEngine;

namespace RoboMed.Interactibles
{
    public class Holdable : MonoBehaviour, IHoldable
    {
        public event Action onHeld;
        public event Action onReleased;

        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;
            onHeld?.Invoke();
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;
            onReleased?.Invoke();
        }

        public MouseFollower Hand { get; set; }
    }
}
