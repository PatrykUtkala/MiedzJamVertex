namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Przedmiot, którego pozycja może być kontrolowana przez zbierające komponenty (np. CollectingArea)
    /// </summary>
    interface ICollectible
    {
        ItemStand StartingPosition { get; set; }

        bool CanCollect { get; }

        void ResetTransform(); // może usunąć

        void OnCollected();
    }
}
