namespace NetDevPL.Navigation
{
    public interface INavigationLink
    {
        int Index { get; }

        string Link { get; }

        string Caption { get; }

        string CssClass { get; }

        NavbarContentType ContentType { get; }
    }
}