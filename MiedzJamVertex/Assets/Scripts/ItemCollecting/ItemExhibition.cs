using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Kolekcja przedmiot�w, w kt�rej s� one ustawiane tylko w okre�lonych miejscach
    /// </summary>
    public class ItemExhibition : MonoBehaviour, IItemCollection
    {
        /// <summary>
        /// Miejsca do wystawy przedmiot�w
        /// </summary>
        private ItemStand[] stands;

        public int FreeSpace => stands.Where(s => s.CurrentItem == null).Count();

        public bool AddItem(GameObject item)
        {
            if (FreeSpace == 0)
                return false;  // brak miejsca

            int posIndex = GetFirstEmpty();
            stands[posIndex].SetItem(item);

            return true;
        }

        /// <summary>
        /// Zwraca indeks pierwszego wolnego stanowiska
        /// </summary>
        private int GetFirstEmpty()
        {
            int i = 0;
            for(; i < stands.Length; i++)
            {
                if (stands[i].CurrentItem == null)
                {
                    // Znaleziono puste
                    return i;
                }
            }

            // Nie znaleziono
            return i;
        }

        private void Awake()
        {
            stands = GetComponentsInChildren<ItemStand>();
        }
    }
}