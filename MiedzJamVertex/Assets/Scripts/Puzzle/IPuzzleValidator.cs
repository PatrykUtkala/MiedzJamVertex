using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboMed.Puzzle
{
    interface IPuzzleValidator
    {
        /// <summary>
        /// Sprawdza, czy poprawnie rozwiązano tę część zagadki
        /// </summary>
        bool Validate();
    }
}
