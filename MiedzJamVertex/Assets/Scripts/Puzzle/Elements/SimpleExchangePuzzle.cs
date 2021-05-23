using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class SimpleExchangePuzzle : MonoBehaviour, IExchangePuzzle, IElement
    {
        [SerializeField] ElementType elementType;
        [SerializeField] bool isBroken = false;

        public ElementType Type => elementType;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out SimpleExchangePuzzle substElement) || substElement.elementType != elementType)
            {
                Debug.Log(this + ": Co ty mi tu próbujesz wcisnąć? " + substitute + "?!");
                return false;
            }

            // Zgodne typy elementów
            return !substElement.isBroken;
        }
    }
}
