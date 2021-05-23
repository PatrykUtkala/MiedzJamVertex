using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboMed.Puzzle
{
    /// <summary>
    /// Zebranie wszystkich elementów zagadek
    /// </summary>
    public class GeneralValidator : MonoBehaviour, IPuzzleValidator
    {
        public static GeneralValidator Current { get; private set; }

        [Tooltip("Obiekty z komponentem IPuzzleValidator")]
        [SerializeField] List<GameObject> puzzleValidators;

        public bool Validate()
        {
            bool solved = true;

            // Prawda we wszystkich elementach
            foreach(var validator in puzzleValidators)
            {
                solved = validator.GetComponent<IPuzzleValidator>().Validate();
                if (!solved)
                    return solved;
            }

            return solved;
        }

        public void AddPuzzleChildren()
        {
            foreach(Transform child in transform)
            {
                if (child.GetComponent<IPuzzleValidator>() != null)
                    puzzleValidators.Add(child.gameObject);
            }
        }

        private void OnEnable()
        {
            if(Current == null)
            {
                Current = this;
            }
        }

        private void OnDisable()
        {
            if(Current == this)
            {
                Current = null;
            }
        }
    }
}
