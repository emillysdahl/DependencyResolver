namespace test2.Models
{
    public class Package
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public Package(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
