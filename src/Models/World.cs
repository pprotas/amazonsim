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
        private Graph pointGraph;

        public World()
        {
            Point a = new Point(-10, 0, 0);
            Point b = new Point(-5, 0, 0);
            Point c = new Point(-5, 0, 5);
            Point d = new Point(0, 0, 5);
            Point e = new Point(5, 0, 5);
            Point f = new Point(5, 0, 0);
            Point g = new Point(5, 0, -5);
            Point h = new Point(0, 0, -5);
            Point i = new Point(-5, 0, -5);
            Point j = new Point(0, 0, 0);

            a.AddNode(b);
            b.AddNode(new List<Point>() { c, i });
            c.AddNode(d);
            d.AddNode(new List<Point>() { e, j });
            e.AddNode(f);
            f.AddNode(g);
            h.AddNode(new List<Point>() { i, j, g });

            List<Point> pointList = new List<Point>() { a, b, c, d, e, f, g, h, i, j };
            pointGraph = new Graph((pointList));

            Robot r = CreateRobot(a);
            r.AddTask(new RobotMove(pointGraph, e));
            r.AddTask(new RobotMove(pointGraph, a));
            Robot r2 = CreateRobot(10, 0, 10);

            Rack p = CreateRack(5, 0, 5);
        }

        private Robot CreateRobot(decimal x, decimal y, decimal z)
        {
            Robot r = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(r);
            return r;
        }

        private Robot CreateRobot(Point point)
        {
            Robot r = new Robot(point);
            worldObjects.Add(r);
            return r;
        }

        private Rack CreateRack(decimal x, decimal y, decimal z)
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