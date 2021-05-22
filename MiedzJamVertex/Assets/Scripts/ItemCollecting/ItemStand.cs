using RoboMed.Interactibles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Miejsce na umieszczenie przedmiotu
    /// </summary>
    public class ItemStand : MonoBehaviour
    {
        private Vector3 designatedPosition;

        public GameObject CurrentItem { get; private set; } = null;

        public void SetItem(GameObject item)
        {
            if(item != null)
            {
                SetItemTransform(item);

                if(item.TryGetComponent(out ICollectible collectible))
                {
                    collectible.OnCollected();
                }
            }

            CurrentItem = item;


            if(TryGetComponent(out IInteractible standInteractible))
            {
                standInteractible.CanInteract = CurrentItem == null;  // dostępny, tylko jeśli wolne miejsce
            }
        }

        private void SetItemTransform(GameObject item)
        {
            item.transform.position = designatedPosition;
            item.transform.parent = transform;
            if (item.TryGetComponent(out IHoldable holdable))
            {
                // Użycie zdefiniowanej rotacji do prezentacji przedmiotu
                item.transform.rotation = holdable.PresentingRotation;
            }
            else
            {
                item.transform.rotation = transform.rotation;
            }
        }

        protected void Awake()
        {
            // Przedmioty będą ustawiane w tej samej pozycji, co ten obiekt
            designatedPosition = transform.position;
        }

        protected void Update()
        {
            if (CurrentItem == null)
                return;

            if (CurrentItem.transform.position != designatedPosition)
            {
                // Przedmiot odszedł
                SetItem(null);
            }
        }
    }
}
