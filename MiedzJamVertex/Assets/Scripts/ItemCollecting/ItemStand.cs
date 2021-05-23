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
        protected Quaternion startingRotation;
        private Vector3 designatedPosition;

        public GameObject StartingItem { get; private set; }
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

        public void DestroyItem()
        {
            if (CurrentItem == null)
                return;

            Destroy(CurrentItem);
            CurrentItem = null;
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

            // Obrót
            if (Mathf.Abs(Quaternion.Angle(item.transform.rotation, startingRotation)) >= 45f
                && Mathf.Abs(Quaternion.Angle(item.transform.rotation, startingRotation * Quaternion.Euler(0, 180f, 0))) >= 45f)
                item.transform.Rotate(0, 90f, 0);
        }

        protected void Awake()
        {
            // Przedmioty będą ustawiane w tej samej pozycji, co ten obiekt
            designatedPosition = transform.position;
        }

        protected void Start()
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out ICollectible startingCollectible))
                {
                    StartingItem = child.gameObject;
                    startingCollectible.StartingPosition = this;
                    startingRotation = StartingItem.transform.rotation;

                    SetItem(StartingItem);
                    break;
                }
            }
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
