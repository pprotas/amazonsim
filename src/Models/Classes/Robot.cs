using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Robot : Model3D, IUpdatable
    {
        private Point _desiredPoint;
        public Point desiredPoint { get { return _desiredPoint;} }
        private Point _currentPoint;
        public Point currentPoint { get { return _currentPoint;} }
        public Robot(decimal x, decimal y, decimal z, decimal rotationX, decimal rotationY, decimal rotationZ)
        {
            this.type = "robot";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }

        public Robot(Point point){
            this.type = "robot";
            this.guid = Guid.NewGuid();

            this._x = point.x;
            this._y = point.y;
            this._z = point.z;

            _currentPoint = point;
        }

        public void AssignPoint(Point point)
        {
            _desiredPoint = point;
            needsUpdate = true;
        }
        
        public void CurrentPoint(Point point){
            _currentPoint = point;
        }
    }
}