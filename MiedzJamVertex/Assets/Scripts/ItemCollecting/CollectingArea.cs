using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Obszar, po wejściu do którego przedmioty IRetrievable są cofane
    /// </summary>
    public class CollectingArea : MonoBehaviour
    {
        enum CollisionType { Collision, Trigger};

        [SerializeField] CollisionType detection;
        [Tooltip("Zbierająca kolekcja z komponentem. Jeśli brak, odstawia na domyślne miejsce.")]
        [SerializeField] GameObject itemCollection;

        protected void TryRetrieve(GameObject go)
        {
            ICollectable retrievable = go.GetComponent<ICollectable>();
            if (retrievable == null)
                return;

            if (retrievable.CanCollect)
            {
                bool collected = false;
                if(itemCollection != null)
                {
                    collected = itemCollection.GetComponent<IItemCollection>().AddItem(go);
                }

                if (!collected)
                {
                    retrievable.ResetTransform();
                }

                retrievable.OnCollected();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (detection != CollisionType.Trigger)
                return;

            TryRetrieve(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (detection != CollisionType.Collision)
                return;

            TryRetrieve(collision.gameObject);
        }

        private void Awake()
        {
            if(itemCollection != null && itemCollection.GetComponent<IItemCollection>() == null)
            {
                Debug.LogError("Item Collection nie ma komponentu IItemCollection");
            }
        }
    }
}
