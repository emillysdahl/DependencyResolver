namespace test2.Models
{
    public class Dependency
    {
        public Package Package { get; set; }
        public List<Package> DependentPackages { get; set; }

        public Dependency(Package package, List<Package> dependentPackage)
        {
            Package = package;
            DependentPackages = dependentPackage;
        }
    }
}
