namespace NetDevPL.Navigation
{
    public class NavigationLink : INavigationLink
    {
        public NavigationLink(int index, string link, string caption)
            : this(index, link, caption, NavbarContentType.Left, null)
        {
        }

        public NavigationLink(int index, string link, string caption, string cssClass)
            : this(index, link, caption, NavbarContentType.Left, cssClass)
        {
        }

        public NavigationLink(int index, string link, string caption, NavbarContentType contentType, string cssClass)
        {
            Index = index;
            Link = link;
            Caption = caption;
            ContentType = contentType;
            CssClass = cssClass;
        }

        public int Index { get; private set; }

        public string Link { get; private set; }

        public string Caption { get; private set; }

        public string CssClass { get; private set; }

        public NavbarContentType ContentType { get; private set; }
    }
}