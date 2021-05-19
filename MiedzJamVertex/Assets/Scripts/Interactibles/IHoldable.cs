using RoboMed.Control;
using System;
using UnityEngine;

namespace RoboMed.Interactibles
{
    // An object that can be held
    interface IHoldable
    {
        event Action onHeld;
        event Action onReleased;

        /// <summary>
        /// W jakiej orientacji powinien być obiekt, gdy jest trzymany w ręku
        /// </summary>
        Quaternion HoldingRotation { get; }

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
