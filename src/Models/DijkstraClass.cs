using System;
using System.Collections.Generic;
using System.Linq;

namespace Models{
    static class DijkstraClass {
        //Dijkstra algorithme die de kortste route vanaf beginpunt naar eindpunt in een graph returnt
        public static List<Point> Dijkstra(Graph pointList, Point startPoint, Point endPoint)
        {
            List<Point> unvisited = new List<Point>();
            List<Point> visited = new List<Point>();
            List<Point> result = new List<Point>();

            // De startPoint moet als eerste aan de beurt komen dus wordt ie alvast toegevoegd aan de list.
            unvisited.Add(startPoint);
            foreach (Point p in pointList.points)
            {
                if (p == startPoint)
                {
                    // Doe niks als het de startPoint is
                }
                else if (p == endPoint)
                {
                    // Idem met de endpoint
                }
                else
                {
                    unvisited.Add(p);
                    p.SetCost(decimal.MaxValue);
                }
            }
            // Hier wordt de endPoint aan het eind van de list toegevoegd, zodat het als laatste wordt berekend.
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
                    // Stelling van Pythagoras om de afstand te berekenen naar de buren van de current node ( niet nodig in onze simulatie omdat er geen diagonale
                    // punten zijn, toch heb ik het toegevoegd).
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
                        // De kortste unvisited point in de graph wordt als volgende berekend in de volgende iteratie van de foreach loop.
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
}