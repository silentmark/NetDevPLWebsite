namespace NetDevPL.Navigation
{
    public interface INavigationLink
    {
        int Index { get; }

        string Link { get; }

        string CssClass { get; }
    }
}