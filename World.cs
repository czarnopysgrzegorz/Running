using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
namespace Running;

public class World
{
    private List<Person>? people;
    private string? userFolderPath;
    public List<Person> People
    {
        get { return people; }
        set { people = value; }
    }
    public string UserFolderPath
    {
        get { return userFolderPath; }
        set { userFolderPath = value;  }
    }

    public World()
    {
        people = new List<Person>();
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        userFolderPath = Path.Combine(documentsPath, "RunningUsers");
        CreateDataFolder();
    }
    public string FullFilePath(string name)
    {
        name = ChangeUserNameToFile(name);
        string fullPath = Path.Combine(UserFolderPath, name);
        return fullPath;
    }
    protected void CreateDataFolder()
    {
        string myFolderPath = UserFolderPath;
        if (!Directory.Exists(myFolderPath))
        {
            Directory.CreateDirectory(myFolderPath);
        }
    }
    public void CreateUsersFromFolder()
    {
        List<string> fileNames = GetFileNames(UserFolderPath);
        foreach (string fileName in fileNames)
        {
            string name = ChangeFileNamesToUsers(fileName);
            Person person = new Person();
            person.Name = name;
            person.Rank = new Rank();
            person.PersonalBest = new PR();
            person.Runs = new List<Run>();
            person.NormalRunAmount = new int();
            person.Zone2Amount = new int();
            People.Add(person);
        }
    }
    public List<string> GetFileNames(string folderPath)
    {
        List<string> fileNames = new List<string>();
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }
        }
        else
        {
            Console.WriteLine("The specified folder does not exist.");
        }
        return fileNames;
    }
    
    
    public Person AddPerson()
    {
        Console.WriteLine("Type your name: ");
        string name = Console.ReadLine();
        while (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
        {
            Console.WriteLine("Type the correct name using only letters");
            name = Console.ReadLine();
        }
        int ind = ContainsPerson(name);
        if (ind == -1)
        {
            Person person = new Person();
            person.Name = name;
            if (CreatePersonFile(person) == 0)
            {
                People.Add(person);
                Console.WriteLine($"Person named {name} was added");
                return person;
            }
        }
        Console.WriteLine($"Person named {name} already exist");
        return null;
    }

    private int ContainsPerson(string name)
    {
        int res = -1;
        for (int i = 0; i < People.Count; i++)
        {
            if (name.Equals(People[i].Name, StringComparison.OrdinalIgnoreCase))
            {
                res = i;
                return res;
            }
        }
        return res;
    }

    public void RemovePerson()
    {
        Console.WriteLine("What is the name of the person you want to remove?");
        string name = Console.ReadLine();
        int ind = ContainsPerson(name);
        if (ind > -1)
        {
            People.RemoveAt(ind);
            RemovePersonFile(name);
            Console.WriteLine($"Person named {name} was removed");
            return;
        }

        Console.WriteLine($"Person named {name} was not found");
    }

    public void RemovePersonFile(string name)
    {
        string path = FullFilePath(name);
        File.Delete(path);
    }

    public Person LogAsPerson()
    {
        Console.WriteLine("Type your name: ");
        string name = Console.ReadLine();
        int ind = ContainsPerson(name);
        if (ind > -1)
        {
            ScanFile(People[ind]);
            Console.WriteLine($"Successfully logged in as {name}");
            return People[ind];
        }

        Console.WriteLine($"Person named {name} was not found");
        return null;
    }

    public void ScanFile(Person person)
    {
        if (person == null)
        {
            throw new ArgumentNullException(nameof(person));
        }
        string pathe = FullFilePath(person.Name);
        if (string.IsNullOrEmpty(pathe))
        {
            throw new ArgumentException("The file path is invalid.");
        }
        string path = FullFilePath(person.Name);
        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split(',');
            if (parts.Length == 4) // Zone 2
            {
                Zone2 zone2 = new Zone2();
                zone2.Date = (DateTime.Parse(parts[0]));
                string dis = parts[1];
                double dist = ConvertDistanceStringToDouble(dis);
                zone2.Distance = dist;
                string tim = parts[2];
                double time = ConvertTimeStringToDouble(tim);
                int timee = ConvertTimeToSeconds(time);
                zone2.Time = timee;
                string pac = parts[3];
                double pace = ConvertTimeStringToDouble(pac);
                int pacee = ConvertTimeToSeconds(pace);
                zone2.Pace = pacee;
                zone2.IsZone2 = true;
                zone2.Owner = person;
                if (person.Runs == null)
                {
                    person.Runs = new List<Run>();
                }
                person.Runs.Add(zone2);
                person.Zone2Amount++;
            }
            if (parts.Length == 5) // Normal
            {
                Normal normal = new Normal();
                normal.Date = (DateTime.Parse(parts[0]));
                string dis = parts[1];
                double dist = ConvertDistanceStringToDouble(dis);
                normal.Distance = dist;
                string tim = parts[2];
                double time = ConvertTimeStringToDouble(tim);
                int timee = ConvertTimeToSeconds(time);
                normal.Time = timee;
                string pac = parts[3];
                double pace = ConvertTimeStringToDouble(pac);
                int pacee = ConvertTimeToSeconds(pace);
                normal.Pace = pacee;
                normal.MaxBPM = int.Parse(parts[4]);
                normal.IsZone2 = false;
                normal.Owner = person;
                if (person.Runs == null)
                {
                    person.Runs = new List<Run>();
                }
                person.Runs.Add(normal);
                person.NormalRunAmount++;
            }
        }
    }
    public static int ConvertTimeToSeconds(double timeValue)
    {
        int temp = (int)timeValue;
        double temp2 = timeValue - temp;
        if (temp2 < 0.60)
        {
            temp2 = temp2 * 100;
            int outTime = (temp * 60) + (int)temp2;
            return outTime;
        }
        else
        {
            Console.WriteLine("Invalid time input.");
            return 0;
        }
    }

    public static double ConvertTimeToMinutes(int timeValue)
    {
        int mins = timeValue / 60;
        double secs = timeValue % 60;
        secs = secs / 100;
        double outing = mins + secs;
        return outing;
    }

    public static string ConvertDistanceToString(double distance)
    {
        double temp = distance;
        int km = (int)temp;
        double m = distance - km;
        m = m * 1000;
        int mt = (int)Math.Round(m);
        string stringDistance = $"{km}.{mt:D2}";
        return stringDistance;
    }

    public double BufferDistance(double first)
    {
        double temp = first;
        int km = (int)temp;
        double m = first - km;
        m = m * 1000;
        int mt = (int)Math.Round(m);
        km = km * 1000;
        int total = km + mt;
        double result = total;
        result = result / 1000;
        return result;
    }
    public static string ConvertTimeToString(double time)
    {
        double temp = time;
        int m = (int)temp;
        double s = time - m;
        s = s * 100;
        int sec = (int)s;
        string stringTime = $"{m}:{sec:D2}";
        return stringTime;
    }

    public static double ConvertTimeStringToDouble(string time)
    {
        double res = 0;
        var parts = time.Split(':');
        if (double.TryParse(parts[0], out double timeValue))
        {
            res = timeValue;
        }

        if (double.TryParse(parts[1], out double timeValuesec))
        {
            float sec;
            timeValuesec = timeValuesec / 100;
        }

        return res + timeValuesec;
    }

    public static double ConvertDistanceStringToDouble(string distance)
    {
        double res = 0;
        if (double.TryParse(distance, out double result))
        {
            return result;
        }
        var parts = distance.Split('.');
        if (double.TryParse(parts[0], out double distanceValue))
        {
            res = distanceValue;
        }

        if (double.TryParse(parts[1], out double distanceValueM))
        {
            if (distanceValue == 0)
            {
            }
            else
            {
                distanceValueM = distanceValueM / 100;
                res = res + distanceValueM;
            }
        }
        return res;
    }

    private int CreatePersonFile(Person person)
    {
        int exist = 0;
        string name = person.Name;
        string path = FullFilePath(name);
        if (File.Exists(path))
        {
            exist = 1;
            return exist;
        }
        else
        {
            StreamWriter writer = new StreamWriter(path);
            return exist;
        }
        return exist;
    }

    public void ShowUsers()
    {
        List<string> fileNames = GetFileNames(UserFolderPath);
        foreach (string fileName in fileNames)
        {
            Console.WriteLine(ChangeFileNamesToUsers(fileName));
        }
    }
    public string ChangeFileNamesToUsers(string fileName)
    {
        string name = fileName;
        int dotIndex = name.IndexOf('.');
        name = dotIndex > 0 ? name.Substring(0, dotIndex) : name;
        return name;
    }

    public string ChangeUserNameToFile(string userName)
    {
        string fileName = userName;
        fileName = fileName + ".csv";
        return fileName;
    }
    
    public void RemoveRunFromFile(Person person)
    {
        string path = FullFilePath(person.Name);
        try
        {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length > 0)
            {
                lines = lines.Take(lines.Length - 1).ToArray();
                File.WriteAllLines(path, lines);
                Console.WriteLine("Successfully removed last run");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}