using RoboMed.Drawing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboMed.Puzzle
{
    public class LineValidator : MonoBehaviour, IPuzzleValidator
    {
        private static List<LineValidator> lineValidators = new List<LineValidator>();

        [Header("Miejsca do połączenia")]
        [Tooltip("Miejsca, gdzie narysowana linia może się stykać z początkową częścią obwodu")]
        [SerializeField] Collider startArea;
        [Tooltip("Miejsca, gdzie narysowana linia może się stykać z końcową częścią obwodu")]
        [SerializeField] Collider endArea;

        private bool isValid = false; // zcachowany wynik walidacji

        public bool Validate()
        {
            return isValid;
        }

        private bool IsNewDrawnValid()
        {
            List<Vector3> connectionPoints = CopperLine.Permanent.GetConnectionPointsWith(CopperLine.Drawable);

            bool linksStart = false;
            bool linksEnd = false;
            foreach (Vector3 conPoint in connectionPoints)
            {
                bool currentLinksStart = false;
                bool currentLinksEnd = false;

                if (startArea.bounds.Contains(conPoint))
                {
                    linksStart = true;
                    currentLinksStart = true;
                }

                if (endArea.bounds.Contains(conPoint))
                {
                    linksEnd = true;
                    currentLinksEnd = true;
                }

                if (!currentLinksStart && !currentLinksEnd)
                {
                    Debug.Log("Połączono linię w niewłaściwym miejscu");
                    return false;
                }
            }

            if (!linksStart || !linksEnd)
                Debug.Log("Nie połączono docelowych linii");
            return linksStart && linksEnd;
        }

        public static void OnFinishLine(Stack<Vector3> line)
        {
            bool isValid = false;

            foreach(LineValidator disconnectedLine in lineValidators)
            {
                if (disconnectedLine.IsNewDrawnValid())
                {
                    disconnectedLine.isValid = true;
                    isValid = true;
                }
            }

            if (isValid)
            {
                // Umieszczenie poprawnej ścieżki na stałe
                CopperLine.Permanent.Add(line);
                CopperLine.Drawable.Clear();
            }
            else
            {
                // Wymazanie niepoprawnej ścieżki
                CopperLine.Drawable.Clear();
            }
        }

        private void OnEnable()
        {
            lineValidators.Add(this);
        }

        private void OnDisable()
        {
            lineValidators.Remove(this);
        }
    }
}
