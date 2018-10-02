namespace Models
{
    public class TruckTeleport : TruckTask
    {
        private Point point;
        public TruckTeleport(Point point)
        {
            this.point = point;
        }
        public void StartTask(Truck t)
        {
            t.Move(point.x, point.y, point.z);
        }

        public bool TaskComplete(Truck t)
        {
            return t.x == point.x && t.y == point.y && t.z == point.z;
        }
    }
}