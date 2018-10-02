namespace Models
{
    public class RobotDropOff : RobotTask
    {
        Truck truck;
        Point point;

        public RobotDropOff(Truck t)
        {
            truck = t;
            point = null;
        }

        public RobotDropOff(Point p)
        {
            truck = null;
            point = p;
        }
        public void StartTask(Robot r)
        {
            if (truck != null && truck.loadable)
            {
                r.RemoveRack(truck);
            }
            else if (point != null)
            {
                r.RemoveRack(point);
            }
        }
        public bool TaskComplete(Robot r)
        {
            return r.rack == null;
        }
    }
}