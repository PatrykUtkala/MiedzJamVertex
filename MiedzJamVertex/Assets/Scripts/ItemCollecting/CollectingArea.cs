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
    public class CollectingArea : MonoBehaviour
    {
        enum CollisionType { Collision, Trigger};

        [SerializeField] CollisionType detection;
        [Tooltip("Zbierająca kolekcja z komponentem IItemCollection. Jeśli brak, odstawia na domyślne miejsce.")]
        [SerializeField] GameObject itemCollection;

        protected void TryCollect(GameObject go)
        {
            ICollectible collectible = go.GetComponent<ICollectible>();
            if (collectible == null)
                return;

            if (collectible.CanCollect)
            {
                bool collected = false;
                if(itemCollection != null)
                {
                    collected = itemCollection.GetComponent<IItemCollection>().AddItem(go);
                }

                if (!collected)
                {
                    collectible.ResetTransform();
                }

                collectible.OnCollected();
            }
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
