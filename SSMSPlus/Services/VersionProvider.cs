namespace SSMSPlus.Services
{
    using SSMSPlusCore.App;
    using System;

    public class VersionProvider : IVersionProvider
    {
        public int GetBuild()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.Build;
        }

        public int[] GetBuildAndRevision()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return new int[] { version.Build, version.Revision };
        }
    }
}

