using System;


Console.WriteLine("What would you like to do today ?");
Console.WriteLine("1. Add a new task");
Console.WriteLine("2. View all tasks");
Console.WriteLine("3. Mark a task as completed");
Console.WriteLine("4. Exit");// Display menu options

var tasks = new List<(string Description, bool IsCompleted)>();
while (true)// Main loop
{
    Console.Write("Enter your choice (1-4): ");
    var choice = Console.ReadLine();// Read user input

    switch (choice)
    {
        case "1":
            Console.Write("Enter task description: ");
            var description = Console.ReadLine() ?? string.Empty;
            tasks.Add((description, false));
            Console.WriteLine("Task added.");
            break;

        case "2":
            Console.WriteLine("Tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                var status = tasks[i].IsCompleted ? "[X]" : "[ ]";
                Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}");
            }
            break;

        case "3":
            Console.Write("Enter task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                var task = tasks[taskNumber - 1];
                tasks[taskNumber - 1] = (task.Description, true);
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
            break;// Mark a task as completed

        case "4":
            Console.WriteLine("Exiting...");
            Console.WriteLine("Goodbye!");
            return;// Exit the application

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break; // Handle invalid choices
    }
}
