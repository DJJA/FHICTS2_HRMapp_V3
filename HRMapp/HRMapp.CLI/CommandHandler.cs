using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.Logic;
using HRMapp.Models;
using HRMapp.Models.Exceptions;

namespace HRMapp.CLI
{
    static class CommandHandler
    {
        public static void HandleCommand(string command)
        {
            switch (command.Trim().ToLower())
            {
                case "show employees":
                    ShowEmployees();
                    break;
                case "add employee":
                    AddEmployee();
                    break;
                case "help":
                    ShowCommands();
                    break;
                default:
                    Console.WriteLine("Enter a valid command. Enter 'Help' for info.");
                    break;
            }
        }

        private static void AddEmployee()
        {
            string firstName, lastName;
            Console.Clear();
            Console.WriteLine("Add new employee");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Firstname:");
            firstName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Lastname:");
            lastName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine($"Adding {firstName} {lastName} to the system...");
            try
            {
                new EmployeeLogic().Add(new ProductionWorker(0, firstName, lastName));
                Console.WriteLine($"{firstName} {lastName} was sucessfully added to the system!");
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
                Console.WriteLine("Press a key to try again...");
                Console.ReadKey();
                AddEmployee();
            }
            catch (DBException dbEx)
            {
                Console.WriteLine(dbEx.Message);
                Console.WriteLine("Press a key to try again...");
                Console.ReadKey();
                AddEmployee();
            }
        }

        private static void ShowEmployees()
        {
            Console.Clear();
            Console.WriteLine("Employees:");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();

            var employees = new EmployeeLogic().GetAll;
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id.ToString().PadRight(3)} {Employee.GetTypeOfEmployee(employee).PadRight(28)} {employee.FirstName} {employee.LastName}");
            }
        }

        private static void ShowCommands()
        {
            Console.Clear();
            Console.WriteLine("Available commands:");
            Console.WriteLine();
            Console.WriteLine("show employees");
            Console.WriteLine("add employee");
        }
    }
}
