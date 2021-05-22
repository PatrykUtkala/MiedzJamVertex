using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Kolekcja przedmiotów, w której s¹ one ustawiane tylko w okreœlonych miejscach
    /// </summary>
    public class ItemExhibition : MonoBehaviour, IItemCollection
    {
        public static ItemExhibition Trash { get; private set; }

        [SerializeField] bool isTrash = false;

        /// <summary>
        /// Miejsca do wystawy przedmiotów
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
            if (isTrash)
            {
                if(Trash == null)
                {
                    Trash = this;
                }
                else
                {
                    Debug.LogWarning("Wykryto wiêcej ni¿ jeden kosz na œmieci");
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