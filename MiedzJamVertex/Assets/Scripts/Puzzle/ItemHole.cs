using RoboMed.Puzzle;
using UnityEngine;
using RoboMed.ItemCollecting;
using RoboMed.Interactibles;

namespace RoboMed.Puzzle
{
    /// <summary>
    /// Miejsce do wstawiania przedmiotów, które jest elementem zagadki
    /// </summary>
    public class ItemHole : ItemStand, IPuzzleValidator, IObjectCan
    {
        public GameObject StartingItem { get; private set; }

        private Quaternion startingRotation; // początkowa rotacja początkowego przedmiotu

        public bool CanDispose(GameObject item)
        {
            // Wolne miejsce i możliwość zebrania
            return CurrentItem == null
                && item.GetComponent<ICollectible>() != null;
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
                Debug.Log(this + ": Brak zamiennika");
                return false;  
            }

            bool sameRotation = CurrentItem.transform.rotation == startingRotation;
            if (!sameRotation)
                Debug.Log($"{this}: {CurrentItem} nie jest obrócony o " + startingRotation.eulerAngles);

            bool validSubstitute = StartingItem.GetComponent<IExchangePuzzle>().IsValidSubstitute(CurrentItem);
            if (!validSubstitute)
                Debug.Log($"{this}: {CurrentItem} nie jest dobrym zamiennikiem dla {StartingItem}");

            return sameRotation && validSubstitute;
        }

        protected new void Awake()
        {
            base.Awake();

            foreach(Transform child in transform)
            {
                if(child.GetComponent<ICollectible>() != null)
                {
                    StartingItem = child.gameObject;
                    SetItem(StartingItem);
                    break;
                }
            }

            if(StartingItem != null)
            {
                startingRotation = StartingItem.transform.rotation;
            }
        }
    }
}
