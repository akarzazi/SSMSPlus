namespace Demo.Services
{
    using SSMSPlusCore.App;
    using System;

    public class DemoVersionProvider : IVersionProvider
    {
        public int GetBuild()
        {
            return 99;
        }

        public int[] GetBuildAndRevision()
        {
            return new int[] { 1, 99 };
        }
    }
}
