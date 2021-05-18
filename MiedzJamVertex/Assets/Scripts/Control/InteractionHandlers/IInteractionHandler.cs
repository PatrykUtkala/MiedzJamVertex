using UnityEngine;

namespace RoboMed.Control.InteractionHandlers
{
    public enum InteractionFrequency { OneTime, Continuous };

    interface IInteractionHandler
    {
        /// <summary>
        /// Stwierdza, czy interakcja następuje tylko po kliknięciu, czy też przy trzymaniu wciśniętego przycisku.
        /// </summary>
        InteractionFrequency InteractionFrequency { get; }

        bool CanInteractWith(GameObject interactible);
        void InteractWith(GameObject interactible);

        void OnAvailableInteractibleChanged(GameObject newInteractible);
    }
}
