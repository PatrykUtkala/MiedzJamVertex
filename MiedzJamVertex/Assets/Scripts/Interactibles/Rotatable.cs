using UnityEngine;

namespace RoboMed.Interactibles
{
    [RequireComponent(typeof(IHoldable))]
    public class Rotatable : MonoBehaviour
    {
        private bool isUnlocked = false;

        private void Update()
        {
            // Obracanie tylko gdy trzymane w rêce
            if (isUnlocked)
            {
                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    // Zgodnie z ruchem wskazówek zegara
                    transform.Rotate(new Vector3(0, 90f, 0), Space.World);
                }
                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    // Przeciwnie do ruchu wskazówek zegara
                    transform.Rotate(new Vector3(0, -90f, 0), Space.World);
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
    }
}
