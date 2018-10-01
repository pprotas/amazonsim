using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class RobotPickUp : RobotTask
    {
        public void StartTask(Robot r)
        {
            if (r.currentPoint.rack != null)
            {
                r.AssignRack(r.currentPoint.rack);
            }
        }

        public bool TaskComplete(Robot r)
        {
            return r.rack != null;
        }
    }
}