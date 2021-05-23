using UnityEngine;

namespace RoboMed.Interactibles
{
    /// <summary>
    /// Obiekt przyjmujący inne obiekty (np. trzymane)
    /// </summary>
    interface IObjectCan
    {
        /// <summary>
        /// Czy może przyjąć obiekt
        /// </summary>
        bool CanDispose(GameObject item);

        /// <summary>
        /// Take it, it's worthless to me anyway.
        /// </summary>
        /// <returns>Czy oferta została przyjęta</returns>
        bool Dispose(GameObject gameObject);
    }
}
