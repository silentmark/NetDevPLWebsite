using System;

namespace NetDevPL.Navigation
{
    public class NavigationModel : INavigationModel
    {
        public static INavigationModel Instance { get; private set; }

        public System.Collections.Generic.IEnumerable<INavigationLink> Links
        {
            get { throw new System.NotImplementedException(); }
        }

        public NavigationModel()
        {
            Instance = this;
        }

        public void Dispose()
        {
            Instance = null;
            GC.SuppressFinalize(this);
        }
    }
}