using RoboMed.Control;
using UnityEngine;

namespace RoboMed.Interactibles
{
    public class Holdable : MonoBehaviour, IHoldable
    {
        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;
        }

        public MouseFollower Hand { get; set; }
    }
}
