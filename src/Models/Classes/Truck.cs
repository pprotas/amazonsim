using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models {
    public class Truck : Model3D, IUpdatable {
        public Truck(decimal x, decimal y, decimal z, decimal rotationX, decimal rotationY, decimal rotationZ) {
            this.type = "truck";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;
        }
    }
}