using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class RobotPickUp : RobotTask
    {
        public void StartTask(Robot r)
        {
            if (r.currentPoint.rack != null && r.rack == null)
            {
                r.AssignRack(r.currentPoint.rack);
                r.currentPoint.rack.AssignPoint(null);
                r.currentPoint.AddRack(null);
            }
        }

        public bool TaskComplete(Robot r)
        {
            if (r.rack != null)
            {
                return r.rack != null;
            }
            return false;
        }
    }
}