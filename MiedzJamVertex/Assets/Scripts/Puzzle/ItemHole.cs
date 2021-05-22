﻿using RoboMed.Puzzle;
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

        public GameObject StartingItem { get; private set; }

        public bool CanDispose(GameObject item)
        {
            if (CurrentItem != null)
                return false; // brak miejsca

            if (item.GetComponent<ICollectible>() == null)
                return false; // brak możliwości zebrania

            if (!item.TryGetComponent(out IElement elementType) || elementType.Type != allowedElements)
                return false;  // niezgodny element

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

        protected new void Awake()
        {
            base.Awake();

            foreach(Transform child in transform)
            {
                if(child.TryGetComponent(out ICollectible startingCollectible))
                {
                    StartingItem = child.gameObject;
                    startingCollectible.StartingPosition = this;

                    SetItem(StartingItem);
                    break;
                }
            }
        }
    }
}
