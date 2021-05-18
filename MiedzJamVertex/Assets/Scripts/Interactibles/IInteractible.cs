using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboMed.Interactibles
{
    /// <summary>
    /// An object that can be interacted with with mouse
    /// </summary>
    interface IInteractible
    {
        bool CanInteract { get; set; }

        /// <summary>
        /// Inform the player that an interaction is available. Called when the mouse is held over the object.
        /// </summary>
        void EnterAvailability();

        /// <summary>
        /// To, co tygryski lubią najbardziej
        /// </summary>
        void Interact();

        /// <summary>
        /// Called when the mouse stops hovering over the object
        /// </summary>
        void QuitAvailability();
    }
}
