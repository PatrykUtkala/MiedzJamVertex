namespace RoboMed.Interactibles
{
    // An object that can be held
    interface IHeldObject
    {
        /// <summary>
        /// Called when the object started being held
        /// </summary>
        void OnHeld();

        /// <summary>
        /// Called after being released
        /// </summary>
        void OnReleased();
    }
}
