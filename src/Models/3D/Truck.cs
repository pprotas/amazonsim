using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class Truck : Model3D, IUpdatable
    {
        private List<Rack> _racks;
        public List<Rack> racks { get { return _racks; } }

        private List<TruckTask> tasks = new List<TruckTask>();

        private bool _loadable;
        public bool loadable { get { return _loadable; } }

        public Truck(decimal x, decimal y, decimal z, decimal rotationX, decimal rotationY, decimal rotationZ)
        {
            this.type = "truck";
            this.guid = Guid.NewGuid();

            this._x = x;
            this._y = y;
            this._z = z;

            this._rX = rotationX;
            this._rY = rotationY;
            this._rZ = rotationZ;

            this._racks = new List<Rack>();
            this._loadable = false;
        }

        public Truck(Point point)
        {
            this.type = "truck";
            this.guid = Guid.NewGuid();

            this._x = point.x;
            this._y = point.y;
            this._z = point.z;

            this._racks = new List<Rack>();
            this._loadable = false;
        }
        public void AddRack(Rack rack)
        {
            _racks.Add(rack);
            rack.AssignPoint(null);
            rack.Move(this.x, this.y + 0.2m, this.z);
        }

        public void RemoveRack(Robot r)
        {
            if (_racks.Last() != null && r.rack == null)
            {
                r.AssignRack(_racks.Last());
                _racks.Remove(_racks.Last());
            }
        }
        public void RemoveRack(Point point)
        {
            if (_racks.Last() != null && point.rack == null)
            {
                point.AddRack(_racks.Last());
                _racks.Remove(_racks.Last());
            }
        }
        public void SwitchLoadable()
        {
            if (loadable == false)
            {
                this._loadable = true;
            }
            else
            {
                this._loadable = false;
            }
        }
        public void Move(Point point)
        {
            if (this.x < point.x)
            {
                this.Move(this.x + 0.2m, this.y, this.z);
            }
            else if (this.x > point.x)
            {
                this.Move(this.x - 0.2m, this.y, this.z);
            }

            if (this.z < point.z)
            {
                this.Move(this.x, this.y, this.z + 0.2m);
            }
            else if (this.z > point.z)
            {
                this.Move(this.x, this.y, this.z - 0.2m);
            }

            if (_racks != null)
            {
                foreach (Rack rack in _racks)
                {
                    rack.Move(this.x, this.y + 0.4m, this.z);
                }
            }
        }

        public void AddTask(TruckTask task)
        {
            tasks.Add(task);
        }

        public override bool Update(int tick)
        {
            if (tasks != null && tasks.Any())
            {
                if (tasks.First().TaskComplete(this))
                {
                    tasks.RemoveAt(0);

                    if (tasks.Count == 0)
                    {
                        tasks = null;
                    }
                }
                if (tasks != null)
                {
                    tasks.First().StartTask(this);
                }
            }
            return base.Update(tick);
        }
    }
}