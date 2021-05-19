using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Interactibles
{
    public class ParticleInteractible : MonoBehaviour, IInteractible
    {
        [SerializeField] ParticleSystem particles;

        public bool CanInteract { get; set; } = true;

        public void EnterAvailability()
        {
            particles.Play();
        }

        public void Interact()
        {

        }

        public void QuitAvailability()
        {

        }
    }
}
