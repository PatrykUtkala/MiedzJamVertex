using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Interactibles
{
    public class SpeakingObject : MonoBehaviour, IInteractible
    {
        public bool CanInteract { get; set; } = true;

        public void EnterAvailability()
        {

        }

        public void Interact()
        {
            Debug.Log("Wiedziałeś, że miedź ma właściwości bakteriobójcze? Mnie to jednak by się przydało coś na wirusy...");
        }

        public void QuitAvailability()
        {

        }
    }
}
