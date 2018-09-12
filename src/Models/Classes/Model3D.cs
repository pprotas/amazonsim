using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Model3D : IUpdatable
    {
        protected double _x = 0;
        protected double _y = 0;
        protected double _z = 0;
        protected double _rX = 0;
        protected double _rY = 0;
        protected double _rZ = 0;

        public string type;
        public Guid guid;
        public double x { get { return _x; } }
        public double y { get { return _y; } }
        public double z { get { return _z; } }
        public double rotationX { get { return _rX; } }
        public double rotationY { get { return _rY; } }
        public double rotationZ { get { return _rZ; } }

        public bool needsUpdate = true;

        public Model3D()
        {
            this.type = "model3d";
            this.guid = Guid.NewGuid();

            this._x = 0;
            this._y = 0;
            this._z = 0;

            this._rX = 0;
            this._rY = 0;
            this._rZ = 0;

        }
        public Model3D(double x, double y, double z, double rotationX, double rotationY, double rotationZ)
        {
            this.type = "model3d";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }

        public virtual void Move(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;

            needsUpdate = true;
        }

        public virtual void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;

            needsUpdate = true;
        }

        public virtual bool Update(int tick)
        {
            if (needsUpdate)
            {
                needsUpdate = false;
                return true;
            }
            return false;
        }
    }
}