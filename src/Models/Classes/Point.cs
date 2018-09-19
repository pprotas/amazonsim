using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Point {
        private double _x;
        private double _y;
        private double _z;
        private List<Point> nodes = new List<Point>();

        public double x {get {return _x;}}
        public double y{get {return _y;}}
        public double z{get {return _z;}}

        public Point(double x, double y, double z){
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public void AddNode(Point node){
            if(!nodes.Contains(node)){
                nodes.Add(node);
                node.AddNode(this);                            
            }
        }

        public void AddNode(List<Point> nodeList){
            foreach(Point node in nodeList){
                this.AddNode(node);
            }
        }
    }
}