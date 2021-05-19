using UnityEngine;

namespace RoboMed.ItemCollecting
{
    interface IItemCollection
    {
        int FreeSpace { get; }
        bool AddItem(GameObject item);
    }
}
