using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class Resistor : MonoBehaviour, IExchangePuzzle, IElement
    {
        [SerializeField] float resistance;
        [SerializeField] float tolerance;
        [SerializeField] bool isBroken = false;

        public ElementType Type => ElementType.Resistor;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out Resistor substResistor))
            {
                Debug.Log(this + ": Co ty mi tu pr?bujesz wcisn??? " + substitute + "?!");
                return false;
            }

            bool sameResistance = Mathf.Abs(substResistor.resistance - resistance) <= Mathf.Epsilon;
            if (!sameResistance)
                Debug.Log($"R??na rezystancja: {this} i {substResistor}");

            bool sameTolerance = Mathf.Abs(substResistor.tolerance - tolerance) <= Mathf.Epsilon;
            if (!sameTolerance)
                Debug.Log($"R??na tolerancja: {this} i {substResistor}");

            return sameResistance && sameTolerance && !substResistor.isBroken;
        }
    }
}