
using System;
using System.Collections.Generic;

enum Priority { Low, Medium, High }

class Program
{
    static void Main()
    {
        // Dictionary to store elapsed time for each task (by index)
        var taskElapsedTimes = new Dictionary<int, TimeSpan>();

        // List to store all tasks. Each task has a description, completion status, due date, allocated hours, and priority.
        var tasks = new List<(string Description, bool IsCompleted, DateTime? DueDate, double? AllocatedHours, Priority Priority)>();

        // Index of the currently timed task (if any)
        int? currentTaskIndex = null;

        // Stopwatch for timing how long a user spends on a task
        var stopwatch = new System.Diagnostics.Stopwatch();

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
            Console.WriteLine("5. Edit a task");
            Console.WriteLine("6. View overdue tasks");
            Console.WriteLine("7. View completed task history");
            Console.WriteLine("8. Exit");// Display menu options

            // Prompt user for menu choice
            Console.Write("Enter your choice (1-8): ");
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
                    // Prompt for priority
                    Console.Write("Enter priority (Low, Medium, High): ");
                    Priority priority = Priority.Medium;
                    var priorityInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(priorityInput) && Enum.TryParse(priorityInput, true, out Priority parsedPriority))
                    {
                        priority = parsedPriority;
                    }
                    // Add the new task to the list
                    tasks.Add((description, false, dueDate, allocatedHours, priority));
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
                            var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                            var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                            var prio = $" | Priority: {tasks[i].Priority}";
                            Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}{due}{hours}{prio}");
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
                            var prio = $" | Priority: {tasks[i].Priority}";
                            Console.WriteLine($"{i + 1}. {tasks[i].Description}{due}{hours}{prio}");
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
                        // If the timer is running for this task, stop and log the time
                        if (stopwatch.IsRunning && currentTaskIndex == (taskNumber - 1))
                        {
                            stopwatch.Stop();
                            var elapsed = stopwatch.Elapsed;
                            if (taskElapsedTimes.ContainsKey(currentTaskIndex.Value))
                                taskElapsedTimes[currentTaskIndex.Value] += elapsed;
                            else
                                taskElapsedTimes[currentTaskIndex.Value] = elapsed;
                            Console.WriteLine($"Timer automatically stopped for this task. Elapsed time: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}");
                        }
                        // Update the task as completed
                        tasks[taskNumber - 1] = (task.Description, true, task.DueDate, task.AllocatedHours, task.Priority);
                        // Show what was completed
                        Console.WriteLine($"Task completed: {task.Description}");
                        // Show elapsed time if available
                        if (taskElapsedTimes.TryGetValue(taskNumber - 1, out var elapsedTotal))
                        {
                            Console.WriteLine($"Time spent on this task: {elapsedTotal.Hours:D2}:{elapsedTotal.Minutes:D2}:{elapsedTotal.Seconds:D2}");
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
                        // Accumulate the elapsed time for this task
                        if (taskElapsedTimes.ContainsKey(currentTaskIndex.Value))
                            taskElapsedTimes[currentTaskIndex.Value] += elapsed;
                        else
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
                    // Edit a task
                    Console.WriteLine("Tasks:");
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        var status = tasks[i].IsCompleted ? "[X]" : "[ ]";
                        var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                        var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                        var prio = $" | Priority: {tasks[i].Priority}";
                        Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}{due}{hours}{prio}");
                    }
                    Console.Write("Enter task number to edit: ");
                    if (int.TryParse(Console.ReadLine(), out int editNum) && editNum > 0 && editNum <= tasks.Count)
                    {
                        var t = tasks[editNum - 1];
                        Console.Write($"New description (leave blank to keep '{t.Description}'): ");
                        var newDesc = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newDesc)) newDesc = t.Description;
                        Console.Write($"New due date (yyyy-MM-dd, leave blank to keep '{(t.DueDate.HasValue ? t.DueDate.Value.ToString("yyyy-MM-dd") : "none")}'): ");
                        var newDueInput = Console.ReadLine();
                        DateTime? newDue = t.DueDate;
                        if (!string.IsNullOrWhiteSpace(newDueInput))
                        {
                            if (DateTime.TryParse(newDueInput, out DateTime parsedNewDue))
                                newDue = parsedNewDue;
                            else
                                Console.WriteLine("Invalid date format. Keeping previous due date.");
                        }
                        Console.Write($"New allocated hours (leave blank to keep '{(t.AllocatedHours.HasValue ? t.AllocatedHours.Value.ToString() : "none")}'): ");
                        var newHoursInput = Console.ReadLine();
                        double? newHours = t.AllocatedHours;
                        if (!string.IsNullOrWhiteSpace(newHoursInput))
                        {
                            if (double.TryParse(newHoursInput, out double parsedNewHours))
                                newHours = parsedNewHours;
                            else
                                Console.WriteLine("Invalid hours format. Keeping previous value.");
                        }
                        Console.Write($"New priority (Low, Medium, High, leave blank to keep '{t.Priority}'): ");
                        var newPrioInput = Console.ReadLine();
                        Priority newPrio = t.Priority;
                        if (!string.IsNullOrWhiteSpace(newPrioInput) && Enum.TryParse(newPrioInput, true, out Priority parsedNewPrio))
                            newPrio = parsedNewPrio;
                        tasks[editNum - 1] = (newDesc, t.IsCompleted, newDue, newHours, newPrio);
                        Console.WriteLine("Task updated. Press ENTER to continue...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Invalid task number. Press ENTER to continue...");
                        Console.ReadLine();
                    }
                    break;

                case "6":
                    // View overdue tasks
                    Console.WriteLine("Overdue tasks:");
                    bool foundOverdue = false;
                    var now = DateTime.Now;
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        var taskDueDate = tasks[i].DueDate;
                        if (!tasks[i].IsCompleted && taskDueDate != null && taskDueDate.Value.Date < now.Date)
                        {
                            foundOverdue = true;
                            var due = $" (Due: {taskDueDate:yyyy-MM-dd})";
                            var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                            var prio = $" | Priority: {tasks[i].Priority}";
                            Console.WriteLine($"{i + 1}. {tasks[i].Description}{due}{hours}{prio}");
                        }
                    }
                    if (!foundOverdue)
                        Console.WriteLine("No overdue tasks.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    break;


                case "7":
                    // View completed task history
                    Console.WriteLine("Completed Task History:");
                    bool anyCompleted = false;
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (tasks[i].IsCompleted)
                        {
                            anyCompleted = true;
                            var due = tasks[i].DueDate != null ? $" (Due: {tasks[i].DueDate:yyyy-MM-dd})" : "";
                            var hours = tasks[i].AllocatedHours != null ? $" | Allocated hours: {tasks[i].AllocatedHours}" : "";
                            var prio = $" | Priority: {tasks[i].Priority}";
                            var actual = taskElapsedTimes.ContainsKey(i) ? $" | Actual time: {taskElapsedTimes[i].Hours:D2}:{taskElapsedTimes[i].Minutes:D2}:{taskElapsedTimes[i].Seconds:D2}" : " | Actual time: 00:00:00";
                            Console.WriteLine($"{i + 1}. {tasks[i].Description}{due}{hours}{prio}{actual}");
                        }
                    }
                    if (!anyCompleted)
                        Console.WriteLine("No completed tasks yet.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    break;

                case "8":
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
    }
}   
