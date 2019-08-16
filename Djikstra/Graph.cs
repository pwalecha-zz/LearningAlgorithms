using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Djikstra
{
    public class Graph
    {
        internal List<Vertex> Nodes = new List<Vertex>();
        internal List<Edge> Edges = new List<Edge>();

        public class Vertex
        {
            public Vertex(string name)
            {
                VertexName = name;
            }

            public string VertexName { get; set; }
        }

        public class Edge
        {
            public int weightage { get; set; }

            public Vertex FromEdge { get; set; }

            public Vertex ToEdge { get; set; }
        }

        public void AddVertex(string vertexName)
        {
            Nodes.Add(new Vertex(vertexName));
        }

        public void AddEdge(string fromVertexName, string toVertextName, int edgeWeight)
        {
            var fromNode = Nodes.FirstOrDefault(x => x.VertexName == fromVertexName);
            var toNode = Nodes.FirstOrDefault(x => x.VertexName == toVertextName);
            Edges.Add(new Edge()
            {
                FromEdge = fromNode,
                ToEdge = toNode,
                weightage = edgeWeight
            });
        }
    }

    public class CityFlights
    {
        private List<Tuple<string, int>> _distFromSource = new List<Tuple<string, int>>();
        private Graph _graph;
        public void SetUpCityFlights()
        {
            _graph = new Graph();
            _graph.AddVertex("Atlanta");
            _graph.AddVertex("Boston");
            _graph.AddVertex("Chicago");
            _graph.AddVertex("Denver");
            _graph.AddVertex("El Peso");

            _graph.AddEdge("Atlanta", "Boston", 100);
            _graph.AddEdge("Boston", "Chicago", 120);
            _graph.AddEdge("Chicago", "El Peso", 80);
            _graph.AddEdge("Atlanta", "Denver", 160);
            _graph.AddEdge("Boston", "Denver", 180);
            _graph.AddEdge("Denver", "El Peso", 140);
            _graph.AddEdge("Denver", "Chicago", 40);
            _graph.AddEdge("El Peso", "Boston", 100);
        }

        private void TraverseShortest(string nextVertex, List<Tuple<string, int>> toCitiesCost, List<string> visitedVertices)
        {
            visitedVertices.Add(nextVertex);
            var edgesFromCurrentVertex = _graph.Edges.Where(x => x.FromEdge.VertexName == nextVertex);

            if (edgesFromCurrentVertex.Count() == 0)
                return;

            foreach (var edge in edgesFromCurrentVertex)
            {
                var toVertext = edge.ToEdge.VertexName;
                int index = 0;
                var currCost = edge.weightage;
                var toCity = toCitiesCost.FirstOrDefault(x => x.Item1 == toVertext);
                var fromStartingToCurrentVertext = toCitiesCost.FirstOrDefault(x => x.Item1 == nextVertex);
                int costToNextVertext = 0;

                if (fromStartingToCurrentVertext != null)
                    costToNextVertext = fromStartingToCurrentVertext.Item2;

                if (toCity != null)
                {
                    foreach (var tuple in toCitiesCost)
                    {
                        if (tuple.Item1 == toVertext && toCity.Item2  > currCost + costToNextVertext)
                        {
                            toCitiesCost[index] = Tuple.Create(tuple.Item1, edge.weightage + costToNextVertext);
                            break;
                        }
                        index++;
                    }
                }
                else
                    toCitiesCost.Add(new Tuple<string, int>(toVertext, edge.weightage + costToNextVertext));

            }

            if (_graph.Nodes.Count() == visitedVertices.Count())
                return;
            

            string consecutiveVertex = nextVertex;
            int minCost = 0;
            foreach (var city in toCitiesCost)
            {
                if (visitedVertices.Contains(city.Item1))
                    continue;
                if (minCost == 0 || city.Item2 < minCost)
                {
                    minCost = city.Item2;
                    consecutiveVertex = city.Item1;
                }
            }

            TraverseShortest(consecutiveVertex, toCitiesCost, visitedVertices);
        
        }


        public int GetCheapestConnectingFlightFrom(string fromCity, string toCity)
        {
            int cheapestCost = 0;
            string startingVertex = fromCity;
            List<Tuple<string,int>> toCitiesCost= new List<Tuple<string, int>>();
            List<string> visitedVertices = new List<string>();
            var totalVertices = _graph.Nodes.Count();
            
            TraverseShortest(fromCity, toCitiesCost, visitedVertices);

            cheapestCost = toCitiesCost.FirstOrDefault(x => x.Item1 == toCity).Item2;
            return cheapestCost;
        }


    }

}
