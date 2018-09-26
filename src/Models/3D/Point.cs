using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Point
    {
        private decimal _x;
        private decimal _y;
        private decimal _z;
        private List<Point> _nodes = new List<Point>();
        private decimal _cost;
        private Point _path;

        public List<Point> nodes { get { return _nodes; } }
        public decimal cost { get { return _cost; } }
        public Point path { get { return _path; } }
        public decimal x { get { return _x; } }
        public decimal y { get { return _y; } }
        public decimal z { get { return _z; } }

        public Point(decimal x, decimal y, decimal z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public void AddNode(Point node)
        {
            if (!nodes.Contains(node))
            {
                _nodes.Add(node);
                node.AddNode(this);
            }
        }

        public void AddNode(List<Point> nodeList)
        {
            foreach (Point node in nodeList)
            {
                this.AddNode(node);
            }
        }

        public void SetCost(decimal cost){
            this._cost = cost;
        }
        public void SetPath(Point path){
            this._path = path;
        }
    }
}