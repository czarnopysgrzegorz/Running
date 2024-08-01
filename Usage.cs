namespace Running;

public class Usage : World
{
    public void UseIsLive(Person person, Login login)
    {
        bool running = true;
        while (running)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            if (person.Name == null)
            {
                throw new ArgumentNullException(nameof(person.Name), "Person's name cannot be null.");
            }

            if (person.Rank == null)
            {
                throw new ArgumentNullException(nameof(person.Rank), "Person's rank cannot be null.");
            }

            if (person.Rank.Division == null)
            {
                throw new ArgumentNullException(nameof(person.Rank.Division), "Person's rank division cannot be null.");
            }
            Console.WriteLine($"{person.Name} - {person.Rank.Elo}");
            Console.WriteLine("1. Add a run");
            Console.WriteLine("2. Remove last run");
            Console.WriteLine("3. Show all runs");
            Console.WriteLine("4. Show all normal runs");
            Console.WriteLine("5. Show all zone 2 runs");
            Console.WriteLine("6. Show Personal records");
            Console.WriteLine("7. Show Elo representation of a run");
            Console.WriteLine("8. Logout");
            Console.WriteLine("9. Exit program");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    person.AddRun(person);
                    break;
                case "2":
                    person.RemoveLastRun();
                    break;
                case "3":
                    person.ShowRuns();
                    break;
                case "4":
                    person.ShowNormalRuns();
                    break;
                case "5":
                    person.ShowZone2Runs();
                    break;
                case "6":
                    person.showPR();
                    break;
                case "7":
                    Console.WriteLine("Type elo");
                    string e = Console.ReadLine();
                    int elo = int.Parse(e);
                    person.Rank.ShowEloRepresentationOfRuns();
                    break;
                case "8":
                    running = false;
                    login.LoginisLive(0);
                    break;
                case "9":
                    running = false;
                    login.LoginisLive(1);
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}