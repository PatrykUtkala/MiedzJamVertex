using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Movement
{
    public class SmoothMover : MonoBehaviour
    {
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 5f;

        public Vector3 TargetPosition { get; set; }
        public Vector3 LocalTargetPosition
        {
            set
            {
                TargetPosition = transform.TransformPoint(value);
            }
        }
        public Quaternion TargetRotation { get; set; }
        public Quaternion LocalTargetRotation
        {
            set
            {
                if(transform.parent == null)
                {
                    TargetRotation = value;
                }
                else
                {
                    TargetRotation = transform.parent.rotation * value;
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            TargetPosition = position;
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
            LocalTargetPosition = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
            TargetRotation = rotation;
        }

        public void SetLocalRotation(Quaternion rotation)
        {
            transform.localRotation = rotation;
            LocalTargetRotation = rotation;
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * movementSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.deltaTime * rotationSpeed);
        }

        private void Start()
        {
            TargetPosition = transform.position;
            TargetRotation = transform.rotation;
        }
    }
}
