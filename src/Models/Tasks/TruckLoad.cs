using System.Linq;

namespace Models
{
    public class TruckLoad : TruckTask
    {
        public void StartTask(Truck t)
        {
            t.SwitchLoadable();
        }

        public bool TaskComplete(Truck t)
        {
            if(t.racks.Any() && t.loadable){
                t.SwitchLoadable();
            }
            return t.racks.Count == 3;
        }
    }
}