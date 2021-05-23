using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class Condensator : MonoBehaviour, IExchangePuzzle, IElement
    {
        [SerializeField] float capacity;
        [SerializeField] float voltage;
        [SerializeField] bool isBroken = false;

        private Quaternion startingRotation;

        public ElementType Type => ElementType.Condensator;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out Condensator substCondensator))
            {
                Debug.Log(this + ": Co ty mi tu próbujesz wcisnąć? " + substitute + "?!");
                return false;
            }

            bool sameRotation = Mathf.Abs(Quaternion.Angle(substitute.transform.rotation, startingRotation)) < Mathf.Epsilon;
            if (!sameRotation)
                Debug.Log($"{this}: {substitute} nie jest obrócony o " + startingRotation.eulerAngles);

            bool greaterEqualCapacity = substCondensator.capacity >= capacity - Mathf.Epsilon;
            if (!greaterEqualCapacity)
                Debug.Log($"{this}: {substCondensator} ma mniejszą pojemność");

            bool greaterEqualVoltage = substCondensator.voltage >= voltage - Mathf.Epsilon;
            if(!greaterEqualVoltage)
                Debug.Log($"{this}: {substCondensator} ma mniejsze napięcie");

            return sameRotation && greaterEqualCapacity && greaterEqualVoltage && !substCondensator.isBroken;
        }

        private void Start()
        {
            startingRotation = transform.rotation;
        }
    }
}
