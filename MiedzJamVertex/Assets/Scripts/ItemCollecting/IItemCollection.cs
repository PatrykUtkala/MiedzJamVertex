using UnityEngine;

namespace RoboMed.ItemCollecting
{
    interface IItemCollection
    {
        int FreeSpace { get; }

        /// <summary>
        /// Umieszcza przedmiot w kolekcji
        /// </summary>
        /// <param name="item">Przedmiot do umieszczenia</param>
        /// <returns>Czy udało się umieścić</returns>
        bool AddItem(GameObject item);
    }
}
