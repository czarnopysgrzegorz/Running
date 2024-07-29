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
            Console.WriteLine($"{person.Name} - {person.Rank.Division}");
            Console.WriteLine("1. Add a run");
            Console.WriteLine("2. Remove last run");
            Console.WriteLine("3. Show all runs");
            Console.WriteLine("4. Show all normal runs");
            Console.WriteLine("5. Show all zone 2 runs");
            Console.WriteLine("6. Show Personal records");
            Console.WriteLine("7. Exit");
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
                    running = false;
                    login.LoginisLive();
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}