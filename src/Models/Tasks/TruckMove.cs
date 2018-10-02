namespace Models{
    public class TruckMove : TruckTask{
        private Point point;
        public TruckMove(Point point){
            this.point = point;
        }
        public void StartTask(Truck t)
        {
            t.Move(point);
        }

        public bool TaskComplete(Truck t)
        {
            if(t.x == point.x && t.y == point.y && t.z == point.z){
                return true;
            }
            return false;
        }
    }
}