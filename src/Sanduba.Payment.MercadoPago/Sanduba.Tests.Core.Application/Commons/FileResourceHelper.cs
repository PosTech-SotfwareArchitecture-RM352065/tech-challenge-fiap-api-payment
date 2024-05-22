using System;
using System.Reflection;

namespace Sanduba.Test.Unit.Commons
{
    internal static class FileResourceHelper
    {
        internal static string GetResource(string resourceName)
        {
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new ArgumentException($"Resource {resourceName} not found.");
            }

            using var reader = new System.IO.StreamReader(resourceStream);
            return reader.ReadToEnd();
        }
    }
}
