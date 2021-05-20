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
        [SerializeField] float distanceFromCamera = 5f;
        [SerializeField] float distanceFromPointed = 2f;
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 5f;

        public float DistanceFromPointed { get; set; }

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        private MouseController mouseController;

        private Quaternion straightRotation;
        private Quaternion interactingRotation;

        private void Awake()
        {
            targetPosition = controlledObject.position;
            targetRotation = controlledObject.rotation;
            DistanceFromPointed = distanceFromPointed;

            mouseController = GetComponent<MouseController>();
        }

        // Update is called once per frame
        void Update()
        {
            targetPosition = GetMousePoint();

            // Obracanie adaptacyjne
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetRotation = Quaternion.LookRotation(mouseRay.direction);
        }

        private void LateUpdate()
        {
            controlledObject.position = Vector3.Lerp(controlledObject.position, targetPosition, Time.deltaTime * movementSpeed);
            controlledObject.rotation = Quaternion.Lerp(controlledObject.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        private Vector3 GetMousePoint()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject pointedObject = mouseController.GetPointedInteractible();
            bool pointsInteractible = pointedObject != null && pointedObject.TryGetComponent(out IInteractible interactible) && interactible.CanInteract;
            if (!pointsInteractible)
                return mouseRay.GetPoint(distanceFromCamera);  // Latanie ze sta³¹ odleg³oœci¹ od kamery

            // Uzyskanie wskazywanego punktu
            pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);

            float distanceToObject = Vector3.Distance(hit.point, Camera.main.transform.position);
            return mouseRay.GetPoint(distanceToObject - DistanceFromPointed);
        }
    }
}
