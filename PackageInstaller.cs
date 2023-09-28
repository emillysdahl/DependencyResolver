using System;
using System.ComponentModel;
using test2.Models;

namespace test2
{
    public static class PackageInstaller
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

            // actual check for whether they are valid
            return CheckIfInstallationsAreValid(packagesToInstall, dependencies);
        }

        private static bool CheckIfInstallationsAreValid(List<Package> packagesToInstall, List<Dependency> dependencies)
        {
            foreach (var package in packagesToInstall)
            {
                var relevantDependencies = dependencies.Where(d => d.Package.Name == package.Name && d.Package.Version == package.Version).ToList();

                // If more than one version of a package is required the installation is invalid.
                if (relevantDependencies.Count > 1)
                {
                    return false;
                }
                else if (relevantDependencies.Count == 1 && relevantDependencies[0].DependentPackages.Count > 1)
                {
                    return false;
                }
                // Check for implicit dependencies.
                else if (relevantDependencies.Count == 1)
                {
                    var relevantDependencyDependencies = relevantDependencies[0].DependentPackages;

                    foreach (var dependency in relevantDependencyDependencies)
                    {
                        // hop over as it dependt on itself, which is fine (still no multiple dependencies).
                        if (dependency.Name == package.Name && dependency.Version == package.Version)
                        {
                            continue;
                        }

                        // could have used LINQ, but for readability it is for loop
                        foreach (var d in dependencies)
                        {
                            bool relevantDependencyIsPackage = d.Package.Name == dependency.Name && d.Package.Version == dependency.Version;
                            bool dependentPackageIsPackage = d.DependentPackages.Any(dp => dp.Name == package.Name && dp.Version == package.Version);

                            // if we find the correct dependency in the dependency list. 
                            // and we find dependency that it not the package it self,
                            // we have multiple (implicit) dependencies.
                            if (relevantDependencyIsPackage && !dependentPackageIsPackage)
                            {
                                return false;
                            }

                            // TO-DO:
                            // implicit dependencies with earlier versions.
                            // e.g. input007, input009
                        }                        
                    }
                }
            }

            return true;
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
