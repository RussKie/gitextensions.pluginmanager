﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Logging;
using NuGet.Configuration;
using NuGet.Frameworks;
using PackageManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManager.Services
{
    public class InstallService
    {
        private static void EnsureConfigDeleted(string configFilePath)
        {
            string path = Path.Combine(Environment.CurrentDirectory, configFilePath);

            if (File.Exists(path))
                File.Delete(path);
        }

        public static IInstallService Create(string extractPath)
        {
            var frameworks = new List<NuGetFramework>() { NuGetFramework.AnyFramework };

            var frameworkFilter = new NuGetFrameworkFilter(frameworks);
            var packageFilter = new DependencyNuGetPackageFilter(
                new List<Args.Dependency>()
                {
                    new Args.Dependency("GitExtensions.Extensibility", null)
                },
                frameworks
            );
            var install = new NuGetInstallService(
                new NuGetSourceRepositoryFactory(),
                new DefaultLog(),
                extractPath,
                new NuGetPackageContentService(new DefaultLog()),
                new NuGetPackageVersionService(
                    new NuGetPackageContentService(new DefaultLog()),
                    new DefaultLog(),
                    packageFilter,
                    frameworkFilter
                ),
                packageFilter,
                frameworkFilter
            );

            return install;
        }
    }
}
