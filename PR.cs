using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Running;

public class PR : World
{
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

    public int zone2Pace;
    public int Zone2Pace
    {
        get { return zone2Pace; }
        set { zone2Pace = value; }
    }

    private int bpm;
    public int BPM
    {
        get { return bpm; }
        set { bpm = value; }
    }
    private int concecutive;
    public int Concecutive
    {
        get { return concecutive; }
        set { concecutive = value; }
    }
    public void UpdatePR(Person owner, int removed)
    {
        int firstRun = 0;
        int firstNormal = 0;
        int firstZone2 = 0;
        PR updatedPR = new PR();
        if (owner.Runs.Count == 0)
        {
            updatedPR.Distance = 0;
            updatedPR.Time = 0;
            updatedPR.Pace = 0;
            updatedPR.Zone2Pace = 0;
            updatedPR.BPM = 0;
            updatedPR.concecutive = 0;
            owner.PersonalBest = updatedPR;
            return;
        }
        for (int i = 0; i < owner.Runs.Count; i++)
        {
            if (owner.Runs[i].IsZone2)
            {
                firstZone2++;
            }

            if (!owner.Runs[i].IsZone2)
            {
                firstNormal++;
            }
            if (removed == 1)
            {
                owner.PersonalBest = new PR();
            }
            IsRunPR(owner.Runs[i], owner, updatedPR, i, firstNormal, firstZone2, removed);
        }
    }
    
    public void IsRunPR(Run run, Person owner, PR pr, int firstRun, int firstNormal, int firstZone2, int removed)
    {
        if (run.Distance > owner.PersonalBest.Distance || owner.Runs.Count == 0 || firstRun == 0)
        {
            pr.Distance = run.Distance;
        }
        else
        {
            pr.Distance = owner.PersonalBest.Distance;
        }
        if (run.Time > owner.PersonalBest.Time || owner.Runs.Count == 0 || firstRun == 0)
        {
            pr.Time = run.Time;
        }
        else
        {
            pr.Time = owner.PersonalBest.Time;
        }
        if (run is Normal normalRun)
        {
            if (normalRun.Pace < owner.PersonalBest.Pace || owner.NormalRunAmount == 0 || firstNormal == 1 || removed == 1)
            {
                pr.Pace = run.Pace;
            }
            else
            {
                pr.Pace = owner.PersonalBest.Pace;
            }
            if (normalRun.MaxBPM > owner.PersonalBest.BPM || owner.NormalRunAmount == 0 || firstNormal == 1)
            {
                pr.BPM = normalRun.MaxBPM;
            }
            else
            {
                pr.BPM = owner.PersonalBest.BPM;
            }

            if (removed != 1)
            {
                pr.Zone2Pace = owner.PersonalBest.Zone2Pace;
            }
        }
        if (run.IsZone2)
        {
            if (run.Pace < owner.PersonalBest.Zone2Pace || owner.Zone2Amount == 0 || firstZone2 == 1 || removed == 1)
            {
                pr.Zone2Pace = run.Pace;
            }
            else
            {
                pr.Zone2Pace = owner.PersonalBest.Zone2Pace;
            }

            if (removed != 1)
            {
                pr.Pace = owner.PersonalBest.Pace;
                pr.BPM = owner.PersonalBest.BPM;
            }
        }
        owner.PersonalBest = pr;
    }
    public void CheckForPR(Run run, Person owner)
    {
        if (owner.Runs.Count > 0)
        {
            if (run.Distance == owner.PersonalBest.Distance)
            {
                Console.WriteLine("Congrats! This is your furthest distance!");
            }
            if (run.Time == owner.PersonalBest.Time)
            {
                Console.WriteLine("Congrats! This is your longest run!");
            }
            if (run is Normal normalRun)
            {
                if (normalRun.Pace == owner.PersonalBest.Pace)
                {
                    Console.WriteLine("Congrats! This is your fastest pace!");
                }
                if (normalRun.MaxBPM == owner.PersonalBest.BPM)
                {
                    Console.WriteLine("Congrats! This is your highest BPM!");
                }
            }
            if (run.IsZone2)
            {
                if (run.Pace == owner.PersonalBest.Zone2Pace)
                {
                    Console.WriteLine("Congrats! This is your fastest zone 2 pace!");
                }
            }
            if (owner.ConsecutiveDays() > 1 && owner.ConsecutiveDays() == owner.PersonalBest.Concecutive)
            {
                Console.WriteLine($"Congrats! This is your longest streak of {owner.PersonalBest.Concecutive} days!");
            }
            if (owner.ConsecutiveDays() > 1 && owner.ConsecutiveDays() != owner.PersonalBest.Concecutive)
            {
                Console.WriteLine($"This is your {owner.ConsecutiveDays()}th day in a row!");
            }
        }
    }
    public override string ToString()
    {
        return $"PRs: Distance: {ConvertDistanceToString(Distance)} Time: {ConvertTimeToString(ConvertTimeToMinutes(Time))} Pace: {ConvertTimeToString(ConvertTimeToMinutes(Pace))} Zone 2 Pace: {ConvertTimeToString(ConvertTimeToMinutes(Zone2Pace))} Max BPM: {BPM} Longest streak: {Concecutive}";
    }
}