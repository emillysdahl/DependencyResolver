using test2.Models;

namespace test2
{
    public static class PackageInstallerGraph
    {
        public static bool IsInstallationValid(string input)
        {
            // split the input string by newline character to get an array of strings
            string[] inputLines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // number of packages to install
            int N = int.Parse(inputLines[0]);

            // populate the packeges to install
            List<Package> packagesToInstall = GetPackages(inputLines, N);

            // check if there are lines left to read dependencies
            if (inputLines.Length <= N + 1)
            {
                return true;
            }

            // M lines (are of the form p1,v1,p2,v2 indicating that package p1 in
            // version v1 depends on p2 in version v2. 
            int M = int.Parse(inputLines[N + 1]);

            // populate the different dependencies.
            List<Dependency> dependencies = GetDependencies(inputLines, N, M);

            var graph = ToGraph(dependencies);
            graph.Print();
            // actual check for whether they are valid
            return CheckIfInstallationsAreValid(graph, packagesToInstall);
        }


        private static bool CheckIfInstallationsAreValid(Graph graph, List<Package> packagesToInstall)
        {
            foreach (var package in packagesToInstall)
            {
                if (graph.GetEdgeCount(package.Name+package.Version) > 1)
                {
                    return false;
                }

                if (graph.GetImplicitCount(package.Name+package.Version) > 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static Graph ToGraph(List<Dependency> dependencies)
        {

            var edges = new List<Tuple<string, string>>();
            foreach (var dependency in dependencies) 
            {
                foreach (var dep in dependency.DependentPackages)
                {
                    var tuple = Tuple.Create(dependency.Package.Name + dependency.Package.Version, dep.Name+dep.Version);
                    
                    edges.Add(tuple);
                }
            }

            var graph = new Graph();

            foreach (var edge in edges)
            {
                graph.AddEdge(edge.Item1, edge.Item2);
            }
            
            return graph;
        }

        private static List<Package> GetPackages(string[] inputLines, int N)
        {
            List<Package> packagesToReturn = new();

            for (int i = 1; i <= N; i++)
            {
                var packageDetails = inputLines[i].Split(',');
                packagesToReturn.Add(new Package(packageDetails[0], packageDetails[1]));
            }

            return packagesToReturn;
        }

        private static List<Dependency> GetDependencies(string[] inputLines, int N, int M)
        {
            List<Dependency> dependenciesToReturn = new();

            for (int i = N + 2; i < N + M + 2; i++)
            {
                var dependencyDetails = inputLines[i].Split(',');

                var packageName = dependencyDetails[0];
                var packageVersion = dependencyDetails[1];

                var relevantPackageDependencies = new List<Package>();
                for (int j = 2; j < dependencyDetails.Length - 1; j += 2)
                {
                    var dependentPackageName = dependencyDetails[j];
                    var dependentPackageVersion = dependencyDetails[j + 1];
                    relevantPackageDependencies.Add(new Package(dependentPackageName, dependentPackageVersion));
                }

                dependenciesToReturn.Add(new Dependency(new Package(packageName, packageVersion),
                    relevantPackageDependencies));
            }

            return dependenciesToReturn;
        }
    }
}
