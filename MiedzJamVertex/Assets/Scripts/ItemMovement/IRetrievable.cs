using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboMed.ItemMovement
{
    /// <summary>
    /// Przedmiot, którego pozycja jest resetowana
    /// </summary>
    interface IRetrievable
    {
        bool ShouldRetrieve { get; }

        void ResetTransform();
    }
}
