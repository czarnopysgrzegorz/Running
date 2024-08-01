namespace Running;

public class Person : World
{
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    private Rank rank;
    public Rank Rank
    {
        get { return rank; }
        set { rank = value; }
    }
    private List<Run> runs;
    public List<Run> Runs
    {
        get { return runs; }
        set { runs = value; }
    }
    private PR personalBest;
    public PR PersonalBest
    {
        get { return personalBest; }
        set { personalBest = value; }
    }
    private int normalRunAmount;
    public int NormalRunAmount
    {
        get { return normalRunAmount; }
        set { normalRunAmount = value; }
    }
    private int zone2Amount;
    public int Zone2Amount
    {
        get { return zone2Amount; }
        set { zone2Amount = value; }
    }
    public void AddRun(Person person)
    {
        int which = WhichRun();
        if (which == 1)
        {
            Zone2 zone2 = new Zone2();
            zone2.AddARun(person);
        }
        if (which == 2)
        {
            Normal normal = new Normal();
            normal.AddARun(person);
        }
    }
    public void RemoveLastRun()
    {
        Console.WriteLine("Are you sure you want to remove your last run? Press 0 to cancel or any button to proceed");
        string inp = Console.ReadLine();
        if (inp.CompareTo("0") == 0)
        {
            return;
        }
        if (Runs.Count > 0)
        {
            if (Runs.Last().IsZone2)
            {
                Zone2Amount--;
            }
            if (!Runs.Last().IsZone2)
            {
                NormalRunAmount--;
            }
            RemoveRunFromFile(this);
            Runs.RemoveAt(Runs.Count - 1);
            PersonalBest.UpdatePR(this, 1);
            Rank.DownloadElo();
        }
        else
        {
            Console.WriteLine("There are no runs to remove");
            return;
        }
    }
    public void ShowRuns()
    {
        if (Runs.Count() == 0)
        {
            Console.WriteLine("There are no runs yet");
            return;
        }
        for (int i = 0; i < Runs.Count; i++)
        {
            Console.WriteLine($"Run {i + 1}: {Runs[i].ToString()}");
        }
    }

    public void ShowNormalRuns()
    {
        if (normalRunAmount > 0)
        {
            for (int i = 0; i < Runs.Count; i++)
            {
                if (!Runs[i].IsZone2)
                {
                    Console.WriteLine(Runs[i].ToString());
                }
            }
        }
        else
        {
            Console.WriteLine("There are no normal runs");
            return;
        }
    }
    public void ShowZone2Runs()
    {
        if (zone2Amount > 0)
        {
            for (int i = 0; i < Runs.Count; i++)
            {
                if (Runs[i].IsZone2)
                {
                    Console.WriteLine(Runs[i].ToString());
                }
            }
        }
        else
        {
            Console.WriteLine("There are no Zone 2 runs");
            return;
        }
    }
    public int WhichRun()
    {
        Console.WriteLine("Was it a Zone 2 run? 1 - yes, 2 - no");
        string read = Console.ReadLine();
        while (read.CompareTo("1") != 0 && read.CompareTo("2") != 0)
        {
            Console.WriteLine("Type the correct number");
            read = Console.ReadLine();
        }
        if (read.CompareTo("1") == 0)
        {
            return 1;
        }
        if (read.CompareTo("2") == 0)
        {
            return 2;
        }
        return 0;
    }
    public void showPR()
    {
        Console.WriteLine(PersonalBest.ToString());
    }

    public void ConsecutiveDays()
    {
        for (int i = 0; i < Runs.Count(); i++)
        {
            
        }
    }
}
