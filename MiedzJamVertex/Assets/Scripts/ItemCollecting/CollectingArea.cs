using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Obszar, po wejściu do którego przedmioty są zbierane lub cofane na domyślne pozycje
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CollectingArea : MonoBehaviour
    {
        enum CollisionType { Collision, Trigger};

        [SerializeField] CollisionType detection = CollisionType.Trigger;
        [Tooltip("Zbierająca kolekcja z komponentem IItemCollection. Jeśli brak, odstawia na domyślne miejsce.")]
        [SerializeField] GameObject itemCollection;

        protected bool TryCollect(GameObject go)
        {
            ICollectible collectible = go.GetComponent<ICollectible>();
            if (collectible == null)
                return false;

            if (collectible.CanCollect)
            {
                bool collected = false;
                if(itemCollection != null)
                {
                    collected = itemCollection.GetComponent<IItemCollection>().AddItem(go);
                }

                if (!collected && collectible.StartingPosition != null && collectible.StartingPosition.CurrentItem == null)
                {
                    // Umieszczenie w początkowym miejscu
                    collectible.StartingPosition.SetItem(go);
                    collected = true;
                }

                if (!collected)
                {
                    if(ItemExhibition.Trash != null)
                    {
                        collected = ItemExhibition.Trash.AddItem(go);
                    }
                    else
                    {
                        Debug.LogError("Brak kosza na śmieci");
                    }
                }

                if (!collected)
                {
                    collectible.ResetTransform();
                }

                if (collected)
                {
                    collectible.OnCollected(); // może usunąć stąd
                    return true;
                }
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (detection != CollisionType.Trigger)
                return;

            TryCollect(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (detection != CollisionType.Collision)
                return;

            TryCollect(collision.gameObject);
        }

        private void Awake()
        {
            if(itemCollection != null && itemCollection.GetComponent<IItemCollection>() == null)
            {
                Debug.LogError(this + ": Item Collection nie ma komponentu IItemCollection");
            }
        }
    }
}
