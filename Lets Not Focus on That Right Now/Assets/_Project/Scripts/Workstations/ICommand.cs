using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }
    public GameObject CommandOutput { get; set; }
    
    public Task Activate(Workstation.Inventory workstationInventory);
}