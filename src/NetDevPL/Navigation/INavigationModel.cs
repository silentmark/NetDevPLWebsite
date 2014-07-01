using System;
using System.Collections.Generic;

namespace NetDevPL.Navigation
{
    public interface INavigationModel : IDisposable
    {
        IEnumerable<INavigationLink> Links { get; }

        void Add(IEnumerable<INavigationLink> links);

        IEnumerable<INavigationLink> GetLinks(NavbarContentType contentType);
    }
}