using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Graph
    {
        private List<Point> _points = new List<Point>();
        public List<Point> points { get { return _points; } }
        public Graph(List<Point> points)
        {
            _points = points;
        }
    }
}