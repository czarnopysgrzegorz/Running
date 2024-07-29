namespace Running;

public class Login : World
{
    public void LoginisLive()
    {
        CreateDataFolder();
        bool running = true;
        while (running)
        {
            CreateUsersFromFolder();
            Console.WriteLine("1. Show users");
            Console.WriteLine("2. Add User");
            Console.WriteLine("3. Remove User");
            Console.WriteLine("4. Log in");
            Console.WriteLine("5. Exit");
            var choice = Console.ReadLine();
            switch (choice)
            {   case "1":
                    ShowUsers();
                    break;
                case "2":
                    AddPerson();
                    break;
                case "3":
                    RemovePerson();
                    break;
                case "4":
                    Person a = LogAsPerson();
                    if (a != null)
                    {
                        a.PersonalBest.UpdatePR(a, 0);
                        running = false;
                        Usage use = new Usage();
                        use.UseIsLive(a, this);
                    }
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}