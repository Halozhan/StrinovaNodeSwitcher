namespace Core.Region
{
    public class Region
    {
        public string Name { get; set; }
        public readonly List<Node.Node> Nodes = [];

        public Region(string name)
        {
            Name = name;
        }
    }
}
