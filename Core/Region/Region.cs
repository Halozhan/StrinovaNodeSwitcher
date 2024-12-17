namespace Core.Region
{
    public class Region(string name)
    {
        public string Name { get; set; } = name;
        public readonly List<Node.Node> Nodes = [];
    }
}
