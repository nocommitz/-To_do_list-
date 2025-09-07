
// Import the System namespace for basic functionality
using System;



// Display the main menu to the user
Console.WriteLine("What would you like to do today ?");


Console.WriteLine("1. Add a new task");
Console.WriteLine("2. View all tasks");
Console.WriteLine("3. Mark a task as completed");
Console.WriteLine("4. Start a task");
Console.WriteLine("5. Exit");// Display menu options


// List to store all tasks. Each task has a description, completion status, due date, and allocated hours.
var tasks = new List<(string Description, bool IsCompleted, DateTime? DueDate, double? AllocatedHours)>();

// Stopwatch for timing how long a user spends on a task
var stopwatch = new System.Diagnostics.Stopwatch();

// Index of the currently timed task (if any)
int? currentTaskIndex = null;

// Main program loop
while (true)
{
    // Prompt user for menu choice
    Console.Write("Enter your choice (1-5): ");
    var choice = Console.ReadLine();

    // Handle user menu choice
    switch (choice)
    {


        case "1":
            // Add a new task
            Console.Write("Enter task description: ");
            var description = Console.ReadLine() ?? string.Empty;
            // Prompt for due date
            Console.Write("Enter due date (yyyy-MM-dd) or leave blank: ");
            var dueDateInput = Console.ReadLine();
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(dueDateInput))
            {
                if (DateTime.TryParse(dueDateInput, out DateTime parsedDate))
                {
                    dueDate = parsedDate;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Due date will be left empty.");
                }
            }
            // Prompt for allocated hours
            Console.Write("Enter allocated hours (e.g., 2.5) or leave blank: ");
            var hoursInput = Console.ReadLine();
            double? allocatedHours = null;
            if (!string.IsNullOrWhiteSpace(hoursInput))
            {
                if (double.TryParse(hoursInput, out double parsedHours))
                {
                    allocatedHours = parsedHours;
                }
                else
                {
                    Console.WriteLine("Invalid hours format. Allocated hours will be left empty.");
                }
            }
            // Add the new task to the list
            tasks.Add((description, false, dueDate, allocatedHours));
            Console.WriteLine("Task added.");
            break;



        case "2":
            // View all tasks
            Console.WriteLine("Tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                var status = tasks[i].IsCompleted ? "[X]" : "[ ]";
                // Show due date if available
                var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                // Show allocated hours if available
                var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}{due}{hours}");
            }
            break;



        case "3":
            // Mark a task as completed
            Console.Write("Enter task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                var task = tasks[taskNumber - 1];
                // Update the task as completed
                tasks[taskNumber - 1] = (task.Description, true, task.DueDate, task.AllocatedHours);
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
            break;



        case "4":
            // Start a timer for a task
            if (stopwatch.IsRunning)
            {
                Console.WriteLine("A timer is already running. Please stop it before starting another task.");
                break;
            }
            Console.Write("Enter task number to start: ");
            if (int.TryParse(Console.ReadLine(), out int startTaskNumber) && startTaskNumber > 0 && startTaskNumber <= tasks.Count)
            {
                currentTaskIndex = startTaskNumber - 1;
                stopwatch.Restart();
                Console.WriteLine($"Timer started for task: {tasks[currentTaskIndex.Value].Description}");
                Console.WriteLine("Press ENTER when you have completed the task to stop the timer...");
                Console.ReadLine();
                stopwatch.Stop();
                var elapsed = stopwatch.Elapsed;
                Console.WriteLine($"Timer stopped. Elapsed time: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
            break;


        case "5":
            // Exit the application
            Console.WriteLine("Exiting...");
            Console.WriteLine("Goodbye!");
            return;


        default:
            // Handle invalid choices
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
