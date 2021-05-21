using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class Resistor : MonoBehaviour, IExchangePuzzle
    {
        [SerializeField] float resistance;
        [SerializeField] float tolerance;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out Resistor substResistor))
            {
                Debug.Log(this + ": Co ty mi tu próbujesz wcisn¹æ? " + substitute + "?!");
                return false;
            }

            bool sameResistance = Mathf.Abs(substResistor.resistance - resistance) <= Mathf.Epsilon;
            if (!sameResistance)
                Debug.Log($"Ró¿na rezystancja: {this} i {substResistor}");

            bool sameTolerance = Mathf.Abs(substResistor.tolerance - tolerance) <= Mathf.Epsilon;
            if (!sameTolerance)
                Debug.Log($"Ró¿na tolerancja: {this} i {substResistor}");

            return sameResistance && sameTolerance;
        }
    }
}