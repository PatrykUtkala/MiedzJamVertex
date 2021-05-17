using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Control
{
    /// <summary>
    /// Testowy test
    /// </summary>
    class ChameleonInteractible : MonoBehaviour, IInteractible
    {
        [SerializeField] Material availableMaterial;

        private Material defaultMaterial;

        private MeshRenderer meshRenderer;

        public void EnterAvailability()
        {
            meshRenderer.material = availableMaterial;
        }

        public void Interact()
        {
            Debug.Log("Wow! You're so good at this game");
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
