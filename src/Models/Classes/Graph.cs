using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Graph {
        List<Point> points = new List<Point>();

        public Graph(List<Point> points){
            this.points = points;
        }
    }
}