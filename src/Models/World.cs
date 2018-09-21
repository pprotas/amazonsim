using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        public Graph pointGraph;
        public World()
        {
            Point a = new Point("a", -10, 0, 0);
            Point b = new Point("b", -5, 0, 0);
            Point c = new Point("c", -5, 0, 5);
            Point d = new Point("d", 0, 0, 5);
            Point e = new Point("e", 5, 0, 5);
            Point f = new Point("f", 5, 0, 0);
            Point g = new Point("g", 5, 0, -5);
            Point h = new Point("h", 0, 0, -5);
            Point i = new Point("i", -5, 0, -5);
            Point j = new Point("j", 0, 0, 0);

            a.AddConnection("b");
            b.AddConnection("c", "i");
            c.AddConnection("b", "d");
            d.AddConnection("c", "e", "j");
            e.AddConnection("d", "f");
            f.AddConnection("e", "g");
            g.AddConnection("f", "h");
            h.AddConnection("g", "j"); 
            i.AddConnection("h", "b");
            j.AddConnection("d", "h");

            List<Point> pointList = new List<Point>() { a, b, c, d, e, f, g, h, i, j };
            pointGraph = new Graph((pointList));

            Robot r = CreateRobot(-10, 0, 0);
            //r.Move(c.x, c.y, c.z);
            Robot r2 = CreateRobot(10, 0, 10);

            Rack p = CreateRack(5, 0, 5);
        }

        private Robot CreateRobot(double x, double y, double z)
        {
            Robot r = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(r);
            return r;
        }

        private Rack CreateRack(double x, double y, double z)
        {
            Rack r = new Rack(x, y, z, 0, 0, 0);
            worldObjects.Add(r);
            return r;
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs)
        {
            foreach (Point p in pointGraph.points)
            {
                obs.OnNext(new SendPoint(p));
            }
            foreach (Model3D m3d in worldObjects)
            {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }
        }

        public bool Update(int tick)
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                Model3D u = worldObjects[i];

                if (u is IUpdatable)
                {
                    bool needsCommand = ((IUpdatable)u).Update(tick);

                    if (needsCommand)
                    {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }

            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}