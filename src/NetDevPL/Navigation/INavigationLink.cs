namespace NetDevPL.Navigation
{
    public interface INavigationLink
    {
        int Index { get; set; }

        string Class { get; set; }

        string Url { get; set; }
    }
}