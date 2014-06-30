namespace NetDevPL.Navigation
{
    public class NavigationLink : INavigationLink
    {
        public NavigationLink(int index, string link)
            : this(index, link, null)
        {
            Index = index;
            Link = link;
        }

        public NavigationLink(int index, string link, string cssClass)
        {
            Index = index;
            Link = link;
            CssClass = cssClass;
        }

        public int Index { get; private set; }

        public string Link { get; private set; }

        public string CssClass { get; private set; }
    }
}