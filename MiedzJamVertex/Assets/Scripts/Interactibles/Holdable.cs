using UnityEngine;

namespace RoboMed.Interactibles
{
    public class Holdable : MonoBehaviour, IHeldObject
    {
        public void OnHeld()
        {
            GetComponent<IInteractible>().CanInteract = false;
        }

        public void OnReleased()
        {
            GetComponent<IInteractible>().CanInteract = true;
        }
    }
}
