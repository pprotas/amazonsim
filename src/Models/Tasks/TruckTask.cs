namespace Models
{
    public interface TruckTask
    {
        void StartTask(Truck t);
        bool TaskComplete(Truck t);
    }
}