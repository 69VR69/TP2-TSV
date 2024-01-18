using TP2_TSV.Actors;

namespace TP2_TSV
{
    public class Graph
    {
        public Dictionary<Graph, float> Childs { get; set; }
        public IActor Element { get; private set; }

        public Graph(IActor element)
        {
            Element = element;
            Childs = new();
        }

        public override string ToString()
        {
            return $"Graph: {Element}";
        }
    }
}
