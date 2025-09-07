
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
// Dictionary to store elapsed time for each task (by index)
var taskElapsedTimes = new Dictionary<int, TimeSpan>();

while (true)
{
    // Clear the console so the menu is always at the top
    Console.Clear();

    // Display the main menu again
    Console.WriteLine("What would you like to do today ?");
    Console.WriteLine("1. Add a new task");
    Console.WriteLine("2. View all tasks");
    Console.WriteLine("3. Mark a task as completed");
    Console.WriteLine("4. Start a task");
    Console.WriteLine("5. Exit");// Display menu options

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
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    var status = tasks[i].IsCompleted ? "[X]" : "[ ]";
                    // Show due date if available
                    var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                    // Show allocated hours if available
                    var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                    Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}{due}{hours}");
                }
            }
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            break;




        case "3":
            // Mark a task as completed
            // List all open (not completed) tasks
            Console.WriteLine("Open tasks:");
            bool hasOpen = false;
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].IsCompleted)
                {
                    hasOpen = true;
                    var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                    var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                    Console.WriteLine($"{i + 1}. {tasks[i].Description}{due}{hours}");
                }
            }
            if (!hasOpen)
            {
                Console.WriteLine("No open tasks to complete.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
                break;
            }
            Console.Write("Enter task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count && !tasks[taskNumber - 1].IsCompleted)
            {
                var task = tasks[taskNumber - 1];
                // Update the task as completed
                tasks[taskNumber - 1] = (task.Description, true, task.DueDate, task.AllocatedHours);
                // Show what was completed
                Console.WriteLine($"Task completed: {task.Description}");
                // Show elapsed time if available
                if (taskElapsedTimes.TryGetValue(taskNumber - 1, out var elapsed))
                {
                    Console.WriteLine($"Time spent on this task: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}");
                }
                else
                {
                    Console.WriteLine("No timer was recorded for this task.");
                }
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid task number.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
            break;




        case "4":
            // Start a timer for a task
            if (stopwatch.IsRunning)
            {
                Console.WriteLine("A timer is already running. Please stop it before starting another task.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
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
                // Store the elapsed time for this task
                taskElapsedTimes[currentTaskIndex.Value] = elapsed;
                Console.WriteLine($"Timer stopped. Elapsed time: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid task number.");
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
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
