using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboMed.ItemCollecting
{
    /// <summary>
    /// Przedmiot, którego pozycja jest resetowana
    /// </summary>
    interface ICollectible
    {
        bool CanCollect { get; }

        /// <summary>
        /// Przywraca do domyślnego położenia i rotacji
        /// </summary>
        void ResetTransform();

        void OnCollected();
    }
}
