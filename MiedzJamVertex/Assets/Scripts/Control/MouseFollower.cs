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

        public float DistanceFromPointed { get; set; }

        private Vector3 targetPosition;

        private MouseController mouseController;

        private void Awake()
        {
            targetPosition = transform.position;
            DistanceFromPointed = distanceFromPointed;

            mouseController = GetComponent<MouseController>();
        }

        // Update is called once per frame
        void Update()
        {
            targetPosition = GetMousePoint();
        }

        private void LateUpdate()
        {
            controlledObject.position = Vector3.Lerp(controlledObject.position, targetPosition, Time.deltaTime * movementSpeed);
        }

        private Vector3 GetMousePoint()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            GameObject pointedObject = mouseController.GetPointedInteractible();
            bool pointsInteractible = pointedObject != null && pointedObject.TryGetComponent(out IInteractible interactible) && interactible.CanInteract;
            if (!pointsInteractible)
                return mouseRay.GetPoint(distanceFromCamera);  // Latanie ze sta�� odleg�o�ci� od kamery

            // Uzyskanie wskazywanego punktu
            pointedObject.GetComponent<Collider>().Raycast(mouseRay, out RaycastHit hit, maxDistance);

            float distanceToObject = Vector3.Distance(hit.point, Camera.main.transform.position);
            return mouseRay.GetPoint(distanceToObject - DistanceFromPointed);
        }
    }
}