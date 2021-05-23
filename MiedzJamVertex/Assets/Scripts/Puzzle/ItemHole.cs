using RoboMed.Puzzle;
using UnityEngine;
using RoboMed.ItemCollecting;
using RoboMed.Interactibles;
using RoboMed.Puzzle.Elements;

namespace RoboMed.Puzzle
{
    /// <summary>
    /// Miejsce do wstawiania przedmiotów, które jest elementem zagadki
    /// </summary>
    public class ItemHole : ItemStand, IPuzzleValidator, IObjectCan
    {
        [SerializeField] ElementType allowedElements;

        public bool CanDispose(GameObject item)
        {
            if (CurrentItem != null)
                return false; // brak miejsca

            if (item.GetComponent<ICollectible>() == null)
                return false; // brak możliwości zebrania

            if (!item.TryGetComponent(out IElement elementType) || elementType.Type != allowedElements)
                return false;  // niezgodny element

            if (Mathf.Abs(Quaternion.Angle(item.transform.rotation, startingRotation)) >= 45f
                && Mathf.Abs(Quaternion.Angle(item.transform.rotation, startingRotation * Quaternion.Euler(0, 180f, 0))) >= 45f)
                return false; // niepoprawne ustawienie

            return true;
        }

        public bool Dispose(GameObject item)
        {
            if(CurrentItem == null)
            {
                // Wstawienie przedmiotu na wolne miejsce
                SetItem(item);
                return true;
            }

            return false; // brak miejsca
        }

        public bool Validate()
        {
            if (StartingItem == null)
            {
                // Jeśli nic nie było, to nic być nie powinno
                if(CurrentItem != null)
                {
                    Debug.Log(this + ": Wstawiono na miejsce, w którym nic nie było");
                    return false;
                }

                return true;
            }

            if (CurrentItem == null)
            {
                // Brak przedmiotu, a było tam coś
                Debug.Log(this + ": Brak zamiennika dla " + StartingItem);
                return false;
            }

            if(StartingItem.GetComponent<IExchangePuzzle>() == null)
            {
                Debug.LogError(StartingItem + " nie posiada komponentu IExchangePuzzle");
                return true;
            }
            return StartingItem.GetComponent<IExchangePuzzle>().IsValidSubstitute(CurrentItem);
        }
    }
}
