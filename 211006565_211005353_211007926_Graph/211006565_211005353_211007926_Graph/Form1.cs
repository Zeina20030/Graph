using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _211006565_211005353_211007926_Graph
{
    public partial class Form1 : Form
    {
        Graph<char> graph;
        public Form1()
        {
            InitializeComponent();
            graph = new Graph<char>(5);
            this.Paint += new PaintEventHandler(Form1_Paint);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graph.DrawGraph(e.Graphics);
        }

        private void AddEdgeButton_Click(object sender, EventArgs e)
        {
            char vertex1 = char.Parse(sourcetextedge.Text);
            char vertex2 = char.Parse(destinationtextedge.Text);
            Point position1 = graph.GetPosition(vertex1);
            Point position2 = graph.GetPosition(vertex2);
            graph.AddEdge(vertex1, vertex2, position1, position2);
            sourcetextedge.Text = "";
            destinationtextedge.Text = "";
            Refresh();
        }

        private void AddVertexButton_Click(object sender, EventArgs e)
        {
            char vertex = char.Parse(vertextext.Text);
            Point position = new Point(int.Parse(XPos.Text), int.Parse(YPos.Text));
            graph.AddVertex(vertex, position);
            vertextext.Text = "";
            XPos.Text = "";
            YPos.Text = "";
            Refresh();
        }

        private void RemoveEdgeButton_Click(object sender, EventArgs e)
        {
            char vertex1 = char.Parse(sourcetextremoveedge.Text);
            char vertex2 = char.Parse(destinationtextremoveedge.Text);
            graph.RemoveEdge(vertex1, vertex2);
            sourcetextremoveedge.Text = "";
            destinationtextremoveedge.Text = "";
            Refresh();
        }

        private void RemoveVertexButton_Click(object sender, EventArgs e)
        {
            char vertex = char.Parse(sourcevertexremove.Text);
            graph.RemoveVertex(vertex);
            sourcevertexremove.Text = "";
            Refresh();
        }

        private void searchbutton_Click(object sender, EventArgs e)
        {
            char s = char.Parse(searchtextbox.Text);
            if (graph.Search(s))
                MessageBox.Show(s + " is found in the graph");
            else
                MessageBox.Show(s + " is not found in the graph");
            searchtextbox.Text = "";
        }

        private void SplitButton_Click(object sender, EventArgs e)
        {
            foreach (var list in graph.AdjacencyList)
            {
                if (list.Count > 0)
                {
                    var vertex = list[0]; // Get the vertex from the adjacency list
                    graph.RemoveAllEdges(vertex); // Remove all edges for this vertex
                }
            }
            Refresh();
        }

        private void getneighborsbutton_Click(object sender, EventArgs e)
        {
            char vertex = char.Parse(getneighborstextbox.Text);
            List<char> neighborList = graph.Neighbors(vertex);
            getneighborstextbox.Text = "";
            string s = "";
            if (neighborList.Count == 0)
            {
                MessageBox.Show("The vertex has no neighbors ");
                return;
            }
            foreach (var neighbor in neighborList)
            {
                s += " " + neighbor;
            }
            MessageBox.Show(s + " are the neighbors of " + vertex);
        }
    }
    interface IGraph<T>
    {
        void AddVertex(T vertex, Point Pos);
        void RemoveVertex(T vertex);
        List<T> Neighbors(T vertex);
    }
    abstract class GRAPHx<T>
    {
        public abstract void AddEdge(T vertex1, T vertex2, Point position1, Point position2);
        public abstract void RemoveEdge(T vertex1, T vertex2);
    }
    class Graph<T> : GRAPHx<T>, IGraph<T>
    {
        List<List<T>> adjacencylist;
        int numberofvertices;
        Dictionary<T, Point> nodePositions;
        public Graph(int numberofvertices)
        {
            this.numberofvertices = numberofvertices;
            adjacencylist = new List<List<T>>();
            for (int i = 0; i < numberofvertices; i++)
            {
                adjacencylist.Add(new List<T>());
            }
            nodePositions = new Dictionary<T, Point>();
        }
        public Point GetPosition(T vertex)
        {
            if (nodePositions.ContainsKey(vertex))
                return nodePositions[vertex];
            else
                throw new Exception("it is not found");
        }
        public List<List<T>> AdjacencyList
        {
            get { return adjacencylist; }
            set { adjacencylist = value; }
        }
        public int Numberofvertices
        {
            get { return numberofvertices; }
            set { numberofvertices = value; }
        }
        public void RemoveAllEdges(T vertex)
        {
            foreach (var list in adjacencylist)
            {
                for (int i = 1; i < list.Count(); i++)
                {
                    RemoveEdge(list[0], list[i]);
                }
            }
        }



        public void AddVertex(T vertex, Point position)
        {
            foreach (var list in adjacencylist)
            {
                if (list.Count == 0)
                {
                    list.Add(vertex);
                }
            }
            List<T> newlist = new List<T>();
            newlist.Add(vertex);
            adjacencylist.Add(newlist);
            if (!nodePositions.ContainsKey(vertex))
            {
                nodePositions[vertex] = position;
            }
        }
        public void RemoveVertex(T vertex)
        {
            List<List<T>> RemoveX = new List<List<T>>();
            foreach (var list in adjacencylist)
            {
                if (list[0].Equals(vertex))
                {
                    RemoveX.Add(list);
                }
            }
            foreach (var Removex in RemoveX)
            {
                adjacencylist.Remove(Removex);
            }
            if (nodePositions.ContainsKey(vertex))
            {
                nodePositions.Remove(vertex);
            }

        }
        public bool Search(T targetVertex)
        {
            bool found = false;
            foreach (var list in adjacencylist)
            {
                foreach (var vertex in list)
                {
                    if (vertex.Equals(targetVertex))
                    {
                        found = true;
                        break;
                    }
                }

            }
            if (found)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public void printgraph()
        {
            for (int i = 0; i < adjacencylist.Count; i++)
            {
                Console.WriteLine("Vertex " + (i + 1) + " connection: ");
                foreach (var list in adjacencylist[i])
                {
                    Console.Write(list + " -->");
                }
                Console.WriteLine();
            }
        }
        public override void AddEdge(T vertex1, T vertex2, Point position1, Point position2)
        {
            bool vertex1found = false;
            for (int i = 0; i < numberofvertices; i++)
            {
                if (adjacencylist[i].Count > 0 && adjacencylist[i][0].Equals(vertex1))
                {
                    vertex1found = true;

                    if (adjacencylist[i].Contains(vertex2))
                    {
                        MessageBox.Show(vertex1 + " is already connected to " + vertex2);
                        return;
                    }

                    adjacencylist[i].Add(vertex2);
                    return;
                }
            }
            if (vertex1found == false)
            {
                AddVertex(vertex1, position1);
                foreach (var list in adjacencylist)
                {
                    if (list.Count > 0 && list[0].Equals(vertex1))
                    {
                        list.Add(vertex2);
                        if (!nodePositions.ContainsKey(vertex1))
                        {
                            nodePositions[vertex1] = position1;
                        }
                        if (!nodePositions.ContainsKey(vertex2))
                        {
                            nodePositions[vertex2] = position2;
                        }
                        return;
                    }
                }

            }
        }
        public override void RemoveEdge(T vertex1, T vertex2)
        {
            foreach (var list in adjacencylist)
            {
                if (list[0].Equals(vertex1))
                {
                    if (list.Contains(vertex2))
                    {
                        list.Remove(vertex2);
                    }
                }
            }

        }
        public void DrawGraph(Graphics g)
        {
            int radius = 20; // Radius of nodes

            // Draw vertices (nodes)
            foreach (var node in nodePositions)
            {
                int x = node.Value.X;
                int y = node.Value.Y;

                g.FillEllipse(Brushes.White, x - radius, y - radius, 2 * radius, 2 * radius);
                g.DrawString(node.Key.ToString(), new Font("Arial", 10), Brushes.Black, x - 7, y - 7);
            }
            // Draw edges between vertices
            foreach (var list in adjacencylist)
            {
                for (int i = 1; i < list.Count(); i++)
                {
                    // Check if both vertices exist in the nodePositions dictionary
                    if (nodePositions.ContainsKey(list[0]) && nodePositions.ContainsKey(list[i]))
                    {
                        Point sourcePoint = nodePositions[list[0]];
                        Point targetPoint = nodePositions[list[i]];

                        g.DrawLine(Pens.White, sourcePoint.X, sourcePoint.Y, targetPoint.X, targetPoint.Y);
                    }
                    else
                    {
                        MessageBox.Show("unable to draw edge between " + list[0] + "and " + list[i]);
                    }
                }
            }
        }

        public List<T> Neighbors(T vertex)
        {
            List<T> neighbors = new List<T>();
            foreach (var list in adjacencylist)
            {

                if (list.Contains(vertex))
                {
                    foreach (var neighbor in list)
                    {
                        if (!neighbor.Equals(vertex))
                        {
                            neighbors.Add(neighbor);
                        }
                    }
                    break;
                }
            }
            return neighbors;
        }
    }
}
