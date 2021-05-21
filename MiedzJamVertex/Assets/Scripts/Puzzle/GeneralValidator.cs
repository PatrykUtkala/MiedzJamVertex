using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Puzzle
{
    /// <summary>
    /// Zebranie wszystkich elementów zagadek
    /// </summary>
    public class GeneralValidator : MonoBehaviour, IPuzzleValidator
    {
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
    }
}
