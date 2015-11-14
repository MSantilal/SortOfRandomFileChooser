using System;
using System.Reflection;

namespace SortOfRandomFileChooser
{
    public class ApplicationInfo
    {
        public string AssemblyTitle { get; private set; }
        public string AssemblyCopyright { get; private set; }
        public string AssemblyVersion { get; private set; }

        public ApplicationInfo()
        {
            AssemblyTitle = ((AssemblyTitleAttribute) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false)).Title;
            AssemblyCopyright = ((AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
            AssemblyVersion = ((AssemblyFileVersionAttribute) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version;
        }
    }
}
