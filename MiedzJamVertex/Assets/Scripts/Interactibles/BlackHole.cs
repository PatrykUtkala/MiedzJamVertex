using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Interactibles
{
    public class BlackHole : MonoBehaviour, IObjectCan
    {
        public bool CanDispose(GameObject item) => true;

        public bool Dispose(GameObject gameObject)
        {
            // Niech się mu przyjrzę
            Destroy(gameObject);
            return true;  // Ofiara zaakceptowana
        }
    }
}
