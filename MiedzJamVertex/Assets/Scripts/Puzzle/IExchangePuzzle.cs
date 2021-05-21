using UnityEngine;

namespace RoboMed.Puzzle
{
    /// <summary>
    /// Elementy, które powinny być zamienione na inne
    /// </summary>
    interface IExchangePuzzle
    {
        /// <summary>
        /// Sprawdza, czy ten element może być zamieniony przez dany
        /// </summary>
        /// <param name="substitute">Zamiennik tego elementu</param>
        bool IsValidSubstitute(GameObject substitute);
    }
}
