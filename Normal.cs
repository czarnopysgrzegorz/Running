namespace Running;

public class Normal : Run
{
    private int maxBPM;

    public int MaxBPM
    {
        get { return maxBPM; }
        set { maxBPM = value; }
    }

    public override void AddARun(Person owner)
    {
        IsZone2 = false;
        Owner = owner;
        AddNormal(owner);
        AddRunToFile();
        owner.PersonalBest.UpdatePR(owner, 0);
        owner.PersonalBest.CheckForPR(this, owner);
        owner.NormalRunAmount++;
    }
    public void AddNormal(Person owner)
    {
        AddDistance(this);
        AddTime(this);
        AddPace(this);
        AddBPM(this);
        AddDate(this);
        owner.Runs.Add(this);
        owner.Rank.UpdateElo(this);
        
    }

    public override string ToString()
    {
        return
            $"Distance: {ConvertDistanceToString(Distance)} km, Time: {ConvertTimeToString(ConvertTimeToMinutes(Time))}, Pace: {ConvertTimeToString(ConvertTimeToMinutes(Pace))}, Max BPM: {MaxBPM}";
    }
}