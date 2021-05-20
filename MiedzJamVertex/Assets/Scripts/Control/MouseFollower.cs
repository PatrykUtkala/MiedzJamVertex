using RoboMed.Control.InteractionHandlers;
using RoboMed.Interactibles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Control
{
    [RequireComponent(typeof(MouseController))]
    public class MouseFollower : MonoBehaviour
    {
        static float maxDistance = 100f;

        [SerializeField] Transform controlledObject;
        [SerializeField] GameObject handBody;
        [SerializeField] float distanceFromCamera = 5f;
        [SerializeField] float distanceFromPointed = 2f;
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 5f;
        [Tooltip("Pozycja, wzglêdem której obraca siê obiekt")]
        [SerializeField] Transform rotationReference;

        public float DistanceFromPointed { get; set; }

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        private MouseController mouseController;

        private Quaternion startingRotation;

        private void Awake()
        {
            targetPosition = controlledObject.position;
            targetRotation = controlledObject.rotation;
            DistanceFromPointed = distanceFromPointed;

            startingRotation = controlledObject.rotation;

            mouseController = GetComponent<MouseController>();
        }

        private void OnEnable()
        {
            GetComponent<ItemHolder>().onHeld += HideHand;
            GetComponent<ItemHolder>().onReleased += ShowHand;
        }

        private void OnDisable()
        {
            GetComponent<ItemHolder>().onHeld -= HideHand;
            GetComponent<ItemHolder>().onReleased -= ShowHand;
        }

        // Update is called once per frame
        void Update()
        {
            targetPosition = GetMousePoint();

            // Obracanie adaptacyjne
            targetRotation = GetTargetRotation();
        }

        private void LateUpdate()
        {
            controlledObject.position = Vector3.Lerp(controlledObject.position, targetPosition, Time.deltaTime * movementSpeed);
            controlledObject.rotation = Quaternion.Lerp(controlledObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private Vector3 GetMousePoint()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (mouseController.TryGetPointedPoint(out Vector3 point, maxDistance))
            {
                float distanceToObject = Vector3.Distance(point, Camera.main.transform.position);
                return mouseRay.GetPoint(distanceToObject - DistanceFromPointed);

            }

            // Latanie ze sta³¹ odleg³oœci¹ od kamery
            return mouseRay.GetPoint(distanceFromCamera); 
        }

        private Quaternion GetTargetRotation()
        {
            if(mouseController.CurrentInteractible != null)
            {
                Vector3 lookDirection = controlledObject.position - rotationReference.position;
                return Quaternion.LookRotation(lookDirection);
            }
            else
            {
                return startingRotation;
            }
        }

        private void ShowHand()
        {
            handBody.GetComponent<MeshRenderer>().enabled = true;
        }

        private void HideHand()
        {
            handBody.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
