using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class SimpleExchangePuzzle : MonoBehaviour, IExchangePuzzle
    {
        [SerializeField] ElementType elementType;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out SimpleExchangePuzzle substElement) || substElement.elementType != elementType)
            {
                Debug.Log(this + ": Co ty mi tu próbujesz wcisnąć? " + substitute + "?!");
                return false;
            }

            // Zgodne typy elementów
            return true;
        }
    }
}
