using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Interactibles
{
    /// <summary>
    /// Testowy test
    /// </summary>
    class ChameleonInteractible : MonoBehaviour, IInteractible
    {
        [SerializeField] Material availableMaterial;

        private Material defaultMaterial;

        private MeshRenderer meshRenderer;

        public bool CanInteract { get; set; } = true;

        public void EnterAvailability()
        {
            meshRenderer.material = availableMaterial;
        }

        public void Interact()
        {

        }

        public void QuitAvailability()
        {
            meshRenderer.material = defaultMaterial;
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            defaultMaterial = meshRenderer.material;
        }
    }
}
