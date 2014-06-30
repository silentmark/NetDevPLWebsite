using System.Collections.Generic;

namespace NetDevPL.Navigation
{
    public interface INavigationModel
    {
        IEnumerable<INavigationLink> Links { get; }
    }
}