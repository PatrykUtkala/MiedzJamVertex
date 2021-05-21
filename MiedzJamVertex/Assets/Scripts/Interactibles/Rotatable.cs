using UnityEngine;

namespace RoboMed.Interactibles
{
    [RequireComponent(typeof(IHoldable))]
    public class Rotatable : MonoBehaviour
    {
        public float YRotation { get; private set; }
        private bool isUnlocked = false;

        private void Update()
        {
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
            GetComponent<IHoldable>().onHeld += Unlock;
            GetComponent<IHoldable>().onReleased += Block;
        }

        private void OnDisable()
        {
            GetComponent<IHoldable>().onHeld -= Unlock;
            GetComponent<IHoldable>().onReleased -= Block;
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
