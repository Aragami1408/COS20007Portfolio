using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SwinAdventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player? player = null;
            Command lookCommand = new LookCommand();
            Command moveCommand = new MoveCommand();
            
            // Player creation menu
            while (player == null)
            {
                Console.Write("Please enter your name -> ");
                string? playerName = Console.ReadLine();
                Console.Write("How would you describe yourself? -> ");
                string? playerDescription = Console.ReadLine();
                Console.Write("You are {0}, {1}.\nIs this correct? (yes/no) -> ", playerName, playerDescription);
                bool confirmationMenuLoop = true;
                while (confirmationMenuLoop)
                {
                    string? decision = Console.ReadLine().ToLower();
                    switch (decision)
                    {
                        case "yes":
                            player = new Player(playerName, playerDescription);
                            confirmationMenuLoop = false;
                            break;
                        case "no":
                            confirmationMenuLoop = false;
                            break;
                        default:
                            Console.Write("Invalid option: please enter yes or no. -> ");
                            break;
                    }
                }
            }

            player.Inventory.Put(new Item(new string[] {"sword", "diamond_sword"}, "Diamond sword", "Sword made of diamond"));
            player.Inventory.Put(new Item(new string[] {"pickaxe", "diamond_pickaxe"}, "Diamond pickaxe", "Pickaxe made of diamond"));

            Bag bag1 = new Bag(new string[] {"bag_1"}, "Bag 1", "Just a small bag");
            bag1.Inventory.Put(new Item(new string[] {"chest", "wooden_chest"}, "Wooden chest", "Small container made of wooden planks"));
            player.Inventory.Put(bag1);
            
            Location loc1 = new Location(new string[] {"hakurei", "jinja"}, "hakurei jinja", "Shrine of Hakurei");
            Location loc2 = new Location(new string[] {"moriya", "jinja"}, "moriya jinja", "Shrine of Moriya");
            Location loc3 = new Location(new string[] {"netherworld"}, "netherworld", "Netherworld");

            Path path1 = new Path(new string[] { "hakurei", "path" });
            Path path2 = new Path(new string[] { "moriya", "path" });
            Path path3 = new Path(new string[] { "netherworld", "path" });

            loc1.Path = path1;  
            path1.SetLocation("west", loc2);
            path1.SetLocation("north", loc3);

            loc2.Path = path2;
            path2.SetLocation("east", loc1);
            path2.SetLocation("north_east", loc3);

            loc3.Path = path3;
            path3.SetLocation("south", loc1);
            path3.SetLocation("south_west", loc3);

            player.Location = loc1;
            
            // Introduction text
            Console.WriteLine("------------------------------");
            Console.WriteLine("Welcome to Swin Adventure!");
            Console.WriteLine("You have arrived in {0}", player.Location.Name);

            // Game loop
            bool gameLoop = true;
            while (gameLoop)
            {
                Console.Write("Command -> ");
                string? playerInput = Console.ReadLine();
                string[] inputToPass = playerInput.Split(new char[] {  }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("");
                foreach (string input in inputToPass)
                {
                    if (lookCommand.AreYou(input))
                    {
                        Console.WriteLine(lookCommand.Execute(player, inputToPass));
                    }
                    else if (moveCommand.AreYou(input))
                    {
                        Console.WriteLine(moveCommand.Execute(player, inputToPass));
                    }
                    else
                    {
                        Console.WriteLine("Wrong command or command not supported yet");
                    }
                }
            }
        }
    }
}
