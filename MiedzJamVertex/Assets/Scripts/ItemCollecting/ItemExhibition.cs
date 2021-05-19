using System.Collections;
using System.Collections.Generic;
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
        private Transform[] stands;
        private int occupiedStands = 0;

        public int FreeSpace => stands.Length - occupiedStands;

        public bool AddItem(GameObject item)
        {
            if (occupiedStands == stands.Length)
                return false;  // brak miejsca

            item.transform.position = stands[occupiedStands].position;
            item.transform.rotation = stands[occupiedStands].rotation;

            occupiedStands++;
            return true;
        }

        private void Awake()
        {
            // Inicjalizacja pozycji dzie�mi
            stands = new Transform[transform.childCount];
            int i = 0;
            foreach(Transform child in transform)
            {
                stands[i++] = child;
            }
        }
    }
}