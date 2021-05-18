using UnityEngine;

namespace RoboMed.Control.InteractionHandlers
{

    interface IInteractionHandler
    {
        bool CanInteractWith(GameObject interactible);
        /// <summary>
        /// Wywoływane po naciśnięciu przycisku
        /// </summary>
        void InteractWith(GameObject interactible);
        /// <summary>
        /// Wywoływane, gdy przycisk jest wciśnięty
        /// </summary>
        void ContinueInteraction(GameObject interactible);

        void OnAvailableInteractibleChanged(GameObject newInteractible);
    }
}
