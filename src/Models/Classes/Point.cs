using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Point
    {
        private string _name;
        private string _connection1;
        private string _connection2;
        private double _x;
        private double _y;
        private double _z;
        //private List<Point> nodes = new List<Point>();

        public string name { get { return _name; } }
        public string connection1 { get { return _connection1; } }
        public string connection2 { get { return _connection2; } }
        public double x { get { return _x; } }
        public double y { get { return _y; } }
        public double z { get { return _z; } }

        public Point(string name, double x, double y, double z)
        {
            this._name = name;
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public void AddConnection(string c)
        {
            if (_connection1 == null)
            {
                _connection1 = c;
            }
            else
            {
                _connection2 = c;
            }
        }

        public void AddConnection(string c1, string c2){
            _connection1 = c1;
            _connection2 = c2;
        }

        // public void AddNode(Point node){
        //     if(!nodes.Contains(node)){
        //         nodes.Add(node);
        //         node.AddNode(this);                            
        //     }
        // }

        // public void AddNode(List<Point> nodeList){
        //     foreach(Point node in nodeList){
        //         this.AddNode(node);
        //     }
        // }

    }
}