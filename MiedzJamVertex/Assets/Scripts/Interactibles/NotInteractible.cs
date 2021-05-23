using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Interactibles
{
    /// <summary>
    /// Brak dodatkowej informacji zwrotnej
    /// </summary>
    class NotInteractible : MonoBehaviour, IInteractible
    {
        public bool CanInteract { get; set; } = true;

        public void EnterAvailability()
        {

        }

        public void Interact()
        {

        }

        public void QuitAvailability()
        {

        }
    }
}
