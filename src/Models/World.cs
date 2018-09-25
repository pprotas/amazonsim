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
            r.AssignPoint(e);
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

        List<Point> route = new List<Point>();
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
                        if (u is Robot)
                        {
                            if (((Robot)u).desiredPoint != null)
                            {
                                route = Dijkstra(pointGraph, ((Robot)u).currentPoint, ((Robot)u).desiredPoint);
                                if (IsOnPoint(((Robot)u), route[1]))
                                {
                                    ((Robot)u).CurrentPoint(route[1]);
                                    route = Dijkstra(pointGraph, ((Robot)u).currentPoint, ((Robot)u).desiredPoint);
                                }
                                Move(((Robot)u), route[1]);     
                            }
                        }
                    }
                    SendCommandToObservers(new UpdateModel3DCommand(u));
                }
            }


            return true;
        }

        public void Move(Robot robot, Point point)
        {
            if (robot.x < point.x)
            {
                robot.Move(robot.x + 0.1m, robot.y, robot.z);
            }
            else if (robot.x > point.x)
            {
                robot.Move(robot.x - 0.1m, robot.y, robot.z);
            }
            
            if (robot.z < point.z)
            {
                robot.Move(robot.x, robot.y, robot.z + 0.1m);
            }
            else if (robot.z > point.z)
            {
                robot.Move(robot.x, robot.y, robot.z - 0.1m);
            }
        }

        public bool IsOnPoint(Robot robot, Point point)
        {
            if (robot.x == point.x)
            {
                if (robot.y == point.y)
                {
                    if (robot.z == point.z)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<Point> Dijkstra(Graph pointList, Point startPoint, Point endPoint)
        {
            List<Point> unvisited = new List<Point>();
            List<Point> visited = new List<Point>();
            List<Point> result = new List<Point>();

            unvisited.Add(startPoint);
            foreach (Point p in pointList.points)
            {
                if (p == startPoint)
                {

                }
                else if (p == endPoint)
                {

                }
                else
                {
                    unvisited.Add(p);
                    p.SetCost(decimal.MaxValue);
                }
            }
            endPoint.SetCost(decimal.MaxValue);
            unvisited.Add(endPoint);

            unvisited[0].SetCost(0);
            unvisited[0].SetPath(startPoint);

            Point current = null;
            while (unvisited.Any())
            {
                current = unvisited[0];

                visited.Add(current);
                unvisited.Remove(current);


                foreach (Point p in current.nodes)
                {
                    double tempDistance = Math.Sqrt(Math.Pow(Convert.ToDouble((current.x - p.x)), 2) + Math.Pow(Convert.ToDouble((current.z - p.z)), 2));
                    decimal distance = Convert.ToDecimal(tempDistance);
                    if (distance + current.cost < p.cost)
                    {
                        p.SetCost(current.cost + distance);
                        p.SetPath(current);
                    }
                }

                Point shortest = null;
                foreach (Point p in unvisited)
                {
                    if (shortest == null)
                    {
                        shortest = p;
                    }
                    if (shortest.cost > p.cost && !visited.Contains(p))
                    {
                        shortest = p;
                    }
                }

                foreach (Point p in unvisited.ToList())
                {
                    if (p == shortest)
                    {
                        unvisited.Remove(p);
                        unvisited.Insert(0, p);
                    }
                }
            }

            foreach (Point p in visited)
            {
                if (p == endPoint)
                {
                    result.Add(p);
                }
            }
            while (result[0] != startPoint)
            {
                foreach (Point p in visited)
                {
                    if (p == result[0].path)
                    {
                        result.Insert(0, p);
                    }
                }
            }

            return result;
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