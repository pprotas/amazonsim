using System.Linq;

namespace Models
{
    public class TruckDump : TruckTask
    {
        Point point;
        public TruckDump(Point point)
        {
            this.point = point;
        }
        public void StartTask(Truck t)
        {
            if (point.rack == null)
            {
                t.RemoveRack(point);
            }
        }

        public bool TaskComplete(Truck t)
        {
            return !t.racks.Any();
        }
    }
}