using RoboMed.Control;
using System;

namespace RoboMed.Interactibles
{
    // An object that can be held
    interface IHoldable
    {
        public event Action onHeld;
        public event Action onReleased;

        /// <summary>
        /// Called when the object started being held
        /// </summary>
        void OnHeld();

        /// <summary>
        /// Called after being released
        /// </summary>
        void OnReleased();

        MouseFollower Hand { get; set; }
    }
}
