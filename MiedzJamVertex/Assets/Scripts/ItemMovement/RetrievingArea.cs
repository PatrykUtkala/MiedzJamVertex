using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.ItemMovement
{
    /// <summary>
    /// Obszar, po wejściu do którego przedmioty IRetrievable są cofane
    /// </summary>
    public class RetrievingArea : MonoBehaviour
    {
        enum CollisionType { Collision, Trigger};

        [SerializeField] CollisionType detection;

        protected void TryRetrieve(GameObject go)
        {
            IRetrievable retrievable = go.GetComponent<IRetrievable>();
            if (retrievable == null)
                return;

            if (retrievable.ShouldRetrieve)
            {
                retrievable.ResetTransform();
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
    }
}
