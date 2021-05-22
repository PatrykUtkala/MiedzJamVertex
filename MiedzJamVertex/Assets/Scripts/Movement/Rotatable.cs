using UnityEngine;
using RoboMed.Interactibles;

namespace RoboMed.Movement
{
    public class Rotatable : MonoBehaviour
    {
        [SerializeField] bool canRotate = true;
        public float YRotation { get; private set; }

        private bool isUnlocked = false;

        private void Update()
        {
            if (!canRotate)
                return;

            // Obracanie tylko gdy trzymane w rêce
            if (isUnlocked)
            {
                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    // Zgodnie z ruchem wskazówek zegara
                    float angle = 90f;
                    transform.Rotate(new Vector3(0, angle, 0), Space.Self);
                    YRotation += angle;
                }
                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    // Przeciwnie do ruchu wskazówek zegara
                    float angle = -90f;
                    transform.Rotate(new Vector3(0, angle, 0), Space.Self);
                    YRotation += angle;
                }
            }
        }

        private void OnEnable()
        {
            if(TryGetComponent(out IHoldable holdable))
            {
                holdable.onHeld += Unlock;
                holdable.onReleased += Block;
            }
        }

        private void OnDisable()
        {
            if (TryGetComponent(out IHoldable holdable))
            {
                holdable.onHeld -= Unlock;
                holdable.onReleased -= Block;
            }
        }

        private void Unlock()
        {
            isUnlocked = true;
        }

        private void Block()
        {
            isUnlocked = false;
        }

        private void Awake()
        {
            YRotation = transform.rotation.eulerAngles.y;
        }
    }
}
