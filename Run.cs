using System.Text.RegularExpressions;
namespace Running;

public abstract class Run : World
{
    private DateTime date;
    public DateTime Date
    {
        get { return date; }
        set { date = value; }
    }

    private double distance;
    public double Distance
    {
        get { return distance; }
        set { distance = value; }
    }

    private int time;
    public int Time
    {
        get { return time; }
        set { time = value; }
    }

    private int pace;
    public int Pace
    {
        get { return pace; }
        set { pace = value; }
    }

    protected Person owner;
    public Person Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    private bool isZone2;
    public bool IsZone2
    {
        get { return isZone2; }
        set { isZone2 = value; }
    }

    public abstract void AddARun(Person owner);

    public void AddDistance(Run run)
    {
        Console.WriteLine("What was your distance?");
        string reed = Console.ReadLine();
        while (!Regex.IsMatch(reed, @"^[0-9,]+$"))
        {
            Console.WriteLine("Invalid distance input. Try: x,xx");
            reed = Console.ReadLine();
        }
        if (double.TryParse(reed, out double distanceValue) && distanceValue > 0)
        {
            double buffer = BufferDistance(distanceValue);
            run.Distance = buffer;
            return;
        }
    }

    public void AddTime(Run run)
    {
        Console.WriteLine("What was your time?");
        string reed2 = Console.ReadLine();
        while (!Regex.IsMatch(reed2, @"^[0-9,]+$"))
        {
            Console.WriteLine("Invalid time input. Try: x,xx");
            reed2 = Console.ReadLine();
        }
        if (double.TryParse(reed2, out double timeValue) && timeValue > 0)
        {
            timeValue = Math.Round(timeValue, 2);
            int outTime = ConvertTimeToSeconds(timeValue);
            run.Time = outTime;
            return;
        }
    }

    public void AddPace(Run run)
    {
        double paceo = (run.Time / run.Distance);
        run.Pace = (int)paceo;
        return;
    }

    public void AddBPM(Normal normal)
    {
        Console.WriteLine("What was your max BPM?");
        string reed3 = Console.ReadLine();
        while (!Regex.IsMatch(reed3, @"^[0-9]+$"))
        {
            Console.WriteLine("Invalid BPM input.");
            reed3 = Console.ReadLine();
        }
        if (int.TryParse(reed3, out int BPMValuee))
        {
            normal.MaxBPM = BPMValuee;
            return;
        }
    }
    public void AddDate(Run run)
    {
        DateTime currentDate = DateTime.Now.Date;
        run.Date = currentDate;
    }
    
    public void AddRunToFile()
    {
        string path = FullFilePath(Owner.Name);
        string csvContent = "Error";
        if (this.isZone2)
        {
            csvContent = $"{Date.ToString("yyyy-MM-dd")},{ConvertDistanceToString(Distance)},{ConvertTimeToString(ConvertTimeToMinutes(Time))},{ConvertTimeToString(ConvertTimeToMinutes(Pace))}";
        }
        if (this is Normal normalRun)
        { 
            csvContent = $"{Date.ToString("yyyy-MM-dd")},{ConvertDistanceToString(Distance)},{ConvertTimeToString(ConvertTimeToMinutes(Time))},{ConvertTimeToString(ConvertTimeToMinutes(Pace))},{normalRun.MaxBPM}";
        }
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(csvContent);
        }
    }
}