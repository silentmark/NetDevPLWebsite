using System;
using System.Collections.Generic;

namespace NetDevPL.Navigation
{
    public interface INavigationModel : IDisposable
    {
        IEnumerable<INavigationLink> Links { get; }
    }
}