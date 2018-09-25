using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models {
    public class Rack : Model3D, IUpdatable {
        public Rack(decimal x, decimal y, decimal z, decimal rotationX, decimal rotationY, decimal rotationZ) {
            this.type = "rack";
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