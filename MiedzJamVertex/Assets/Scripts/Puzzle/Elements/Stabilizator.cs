using UnityEngine;

namespace RoboMed.Puzzle.Elements
{
    public class Stabilizator : MonoBehaviour, IExchangePuzzle
    {
        enum StabilizatorType { Voltage, Current }

        [SerializeField] StabilizatorType stabilizatorType;
        [SerializeField] float voltage;
        [SerializeField] bool isBroken = false;

        private Quaternion startingRotation;

        public bool IsValidSubstitute(GameObject substitute)
        {
            if (!substitute.TryGetComponent(out Stabilizator substStabilizator))
            {
                Debug.Log(this + ": Co ty mi tu próbujesz wcisnąć? " + substitute + "?!");
                return false;
            }

            bool sameRotation = Mathf.Abs(Quaternion.Angle(substitute.transform.rotation, startingRotation)) < Mathf.Epsilon;
            if (!sameRotation)
                Debug.Log($"{this}: {substitute} nie jest obrócony o " + startingRotation.eulerAngles);

            bool sameType = substStabilizator.stabilizatorType == stabilizatorType;
            if (!sameType)
                Debug.Log($"{this}: {substitute} ma niezgodny typ");

            bool sameVoltage = Mathf.Abs(substStabilizator.voltage - voltage) <= Mathf.Epsilon;
            if (!sameVoltage)
                Debug.Log($"Różne napięcie: {this} i {substitute}");

            return sameRotation && sameType && sameVoltage && !substStabilizator.isBroken;
        }

        private void Start()
        {
            startingRotation = transform.rotation;
        }
    }
}
