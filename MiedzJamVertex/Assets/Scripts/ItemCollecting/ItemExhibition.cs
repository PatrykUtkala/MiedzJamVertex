using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Kolekcja przedmiot?w, w kt?rej s? one ustawiane tylko w okre?lonych miejscach
    /// </summary>
    public class ItemExhibition : MonoBehaviour, IItemCollection
    {
        public static ItemExhibition Trash { get; private set; }

        [SerializeField] bool isTrash = false;

        /// <summary>
        /// Miejsca do wystawy przedmiot?w
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

        public void Clear()
        {
            if (stands == null)
                return;

            foreach(ItemStand stand in stands)
            {
                stand.DestroyItem();
            }
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
            if (isTrash)
            {
                if(Trash == null)
                {
                    Trash = this;
                }
                else
                {
                    Debug.LogWarning("Wykryto wi?cej ni? jeden kosz na ?mieci");
                }
            }

            stands = GetComponentsInChildren<ItemStand>();
        }

        private void OnDestroy()
        {
            if(Trash == this)
            {
                Trash = null;
            }
        }
    }
}