namespace Models
{
    public interface RobotTask
    {
        void StartTask(Robot r);
        bool TaskComplete(Robot r);
    }
}