using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class RobotMove : RobotTask
    {
        Graph graph;
        Point point;
        public RobotMove(Graph graph, Point point)
        {
            this.graph = graph;
            this.point = point;
        }

        public void StartTask(Robot r)
        {
            r.MoveOverPath(this.graph, this.point);
        }

        public bool TaskComplete(Robot r)
        {
                return this.point == r.currentPoint;
        }
    }
}