using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Rack : Model3D, IUpdatable
    {
        private Point _point;
        public Point point { get { return _point; } }
        public Rack(decimal x, decimal y, decimal z, decimal rotationX, decimal rotationY, decimal rotationZ)
        {
            this.type = "rack";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }

        public Rack(Point point)
        {
            this.type = "rack";
            this.guid = Guid.NewGuid();
        
            this._x = point.x;
            this._y = point.y;
            this._z = point.z;
            
            this._point = point;
            point.AddRack(this);
        }

        public void AssignPoint(Point point){
            this._point = point;
        }
    }
}