namespace Running;

public class Zone2 : Run
{
    public override void AddARun(Person owner)
    {
        PR personalBest = new PR();
        IsZone2 = true;
        Owner = owner;
        AddZone2(owner, personalBest);
        owner.PersonalBest.UpdatePR(owner, 0);
        owner.PersonalBest.CheckForPR(this, owner);
        Console.WriteLine("----------------------------");
        owner.Zone2Amount++;
    }

    public void AddZone2(Person owner, PR personalBest)
    {
        AddDistance(this);
        AddTime(this);
        AddPace(this);
        AddDate(this);
        owner.Runs.Add(this);
        AddRunToFile();
        int gain = owner.Rank.UpdateElo(this);
        Console.WriteLine("----------------------------");
        if (gain > -1)
        {
            Console.WriteLine($"+{gain}");
        }
        else
        {
            Console.WriteLine($"{gain}");
        }
    }
    
    public override string ToString()
    {
        return $"Zone 2 Run: Distance: {ConvertDistanceToString(Distance)} km, Time: {ConvertTimeToString(ConvertTimeToMinutes(Time))}, Zone 2 Pace: {ConvertTimeToString(ConvertTimeToMinutes(Pace))}";
    }
}