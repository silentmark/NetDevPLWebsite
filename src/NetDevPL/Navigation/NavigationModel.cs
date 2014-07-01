using System.Collections.Generic;
using System.Linq;

namespace NetDevPL.Navigation
{
    public class NavigationModel : Disposable, INavigationModel
    {
        public static INavigationModel Instance { get; private set; }

        private readonly List<INavigationLink> _links;

        public IEnumerable<INavigationLink> Links
        {
            get { return _links; }
        }

        public void Add(IEnumerable<INavigationLink> links)
        {
            _links.AddRange(links);
        }

        public IEnumerable<INavigationLink> GetLinks(NavbarContentType contentType)
        {
            return Links.Where(l => l.ContentType.Equals(contentType)).OrderBy(l => l.Index);
        }

        public NavigationModel()
        {
            Instance = this;
            _links = new List<INavigationLink>();
        }

        protected override void Dispose(bool disposing)
        {
            _links.Clear();
            Instance = null;
        }
    }
}