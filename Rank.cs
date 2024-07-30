namespace Running;

public class Rank : World
{
    private string division;
    public string Division
    {
        get { return division; }
        set { division = value; }
    }
    public Rank()
    {
        Division = "Undefined";
    }
}