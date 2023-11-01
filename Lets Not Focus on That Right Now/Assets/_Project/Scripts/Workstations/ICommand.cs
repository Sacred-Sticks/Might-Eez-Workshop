using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }
    
    public Task Activate(Workstation.Inventory workstationInventory);
}