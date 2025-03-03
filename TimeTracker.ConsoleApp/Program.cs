using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Database;
using Task = TimeTracker.Database.Task;

namespace TimeTracker.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        using var context = new TimeTrackerContext("Server=.;Database=TimeTrackerDB;User ID=sa;Password=sasa@123;TrustServerCertificate=True;");

        while (true)
        {
            Console.WriteLine("1. Create Client");
            Console.WriteLine("2. Create Project");
            Console.WriteLine("3. Create Task");
            Console.WriteLine("4. Create User");
            Console.WriteLine("5. Log Time Entry");
            Console.WriteLine("6. View Time Entries");
            Console.WriteLine("7. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateClient(context);
                    break;
                case "2":
                    CreateProject(context);
                    break;
                case "3":
                    CreateTask(context);
                    break;
                case "4":
                    CreateUser(context);
                    break;
                case "5":
                    LogTimeEntry(context);
                    break;
                case "6":
                    ViewTimeEntries(context);
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void CreateClient(TimeTrackerContext context)
    {
        Console.Write("Enter client name: ");
        var name = Console.ReadLine();

        var client = new Client { Name = name };
        context.Clients.Add(client);
        context.SaveChanges();

        Console.WriteLine($"Client created with ID: {client.Id}");
    }

    static void CreateProject(TimeTrackerContext context)
    {
        Console.Write("Enter project name: ");
        var name = Console.ReadLine();

        Console.Write("Enter client ID: ");
        var clientId = int.Parse(Console.ReadLine());
        var client = context.Clients.Find(clientId);

        if (client == null)
        {
            Console.WriteLine("Client not found.");
            return;
        }

        var project = new Project { Name = name, Client = client, BillableRate = 100 };
        context.Projects.Add(project);
        context.SaveChanges();

        Console.WriteLine($"Project created with ID: {project.Id}");
    }

    static void CreateTask(TimeTrackerContext context)
    {
        Console.Write("Enter task name: ");
        var name = Console.ReadLine();

        Console.Write("Enter project ID: ");
        var projectId = int.Parse(Console.ReadLine());
        var project = context.Projects.Find(projectId);

        if (project == null)
        {
            Console.WriteLine("Project not found.");
            return;
        }

        var task = new Task { Name = name, Project = project };
        context.Tasks.Add(task);
        context.SaveChanges();

        Console.WriteLine($"Task created with ID: {task.Id}");
    }

    static void CreateUser(TimeTrackerContext context)
    {
        Console.Write("Enter user name: ");
        var name = Console.ReadLine();

        Console.Write("Enter user role: ");
        var role = Console.ReadLine();

        Console.Write("Enter billable rate: ");
        var billableRate = decimal.Parse(Console.ReadLine());

        var user = new User { Name = name, Role = role, BillableRate = billableRate };
        context.Users.Add(user);
        context.SaveChanges();

        Console.WriteLine($"User created with ID: {user.Id}");
    }

    static void LogTimeEntry(TimeTrackerContext context)
    {
        Console.Write("Enter user ID: ");
        var userId = int.Parse(Console.ReadLine());
        var user = context.Users.Find(userId);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write("Enter task ID: ");
        var taskId = int.Parse(Console.ReadLine());
        var task = context.Tasks.Find(taskId);

        if (task == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        Console.Write("Enter start time (yyyy-MM-dd HH:mm): ");
        var startTime = DateTime.Parse(Console.ReadLine());

        Console.Write("Enter end time (yyyy-MM-dd HH:mm): ");
        var endTime = DateTime.Parse(Console.ReadLine());

        var timeEntry = new TimeEntry
        {
            User = user,
            Task = task,
            StartTime = startTime,
            EndTime = endTime
        };
        context.TimeEntries.Add(timeEntry);
        context.SaveChanges();

        Console.WriteLine($"Time entry logged with ID: {timeEntry.Id}, Billable Amount: {timeEntry.BillableAmount:C}");
    }

    static void ViewTimeEntries(TimeTrackerContext context)
    {
        var timeEntries = context.TimeEntries
            .Include(te => te.User)
            .Include(te => te.Task)
            .ToList();

        foreach (var entry in timeEntries)
        {
            Console.WriteLine($"ID: {entry.Id}, User: {entry.User.Name}, Task: {entry.Task.Name}, Start: {entry.StartTime}, End: {entry.EndTime}, Billable Amount: {entry.BillableAmount:C}");
        }
    }
}