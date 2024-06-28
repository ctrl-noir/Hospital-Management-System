using static System.Console;
using System;
using System.Transactions;

class Patient
{
    public string Name {get; set;}
    public int Age {get; set;}
    public string Condition {get; set;}

    public Patient(string name, int age, string condition)
    {
        Name = name;
        Age = age;
        Condition = condition;
    }

    //Method to print the patients data
    public void Display()
    {
        WriteLine($"Name: {Name}, Age: {Age}, Condition: {Condition}");
    }
}

class Hospital
{
    private Patient[,] wards;
    //declaration of second array to keep track of number of patients for each ward
    private int[] wardCapacities;

    public Hospital(int numberOfWards, int wardCapacity)
    {
        wards = new Patient[numberOfWards, wardCapacity];
        wardCapacities = new int[numberOfWards];
    }

    public void AddPatient(int ward, string name, int age, string condition){

        if (ward < 0 || ward >= wards.GetLength(0)){

            WriteLine("Invalid ward number.");
            return;
        }

        if (wardCapacities[ward] >= wards.GetLength(1)){

            WriteLine("The ward is full, unable to add more patients.");
            return;
        }

        wards[ward, wardCapacities[ward]] = new Patient(name, age, condition);
        wardCapacities[ward]++;
    }

    public void DisplayWardInformation(){

        for(int i = 0; i < wards.GetLength(0); i++){
            
            int bedsOccupied = 0;
            for(int j = 0; j < wards.GetLength(1); j++){

                if(wards[i,j] != null){
                    bedsOccupied++;
                }
            }
            WriteLine($"Ward {i + 1}: {bedsOccupied} of {wards.GetLength(1)} beds occupied");

        } 
    }

    public void searchPatient(string name){
        bool foundPatient = false;
        for(int i = 0; i < wards.GetLength(0); i++){

            for(int j = 0; j < wards.GetLength(1); j++){

                if(wards[i,j] != null && wards[i,j].Name.Equals(name, StringComparison.OrdinalIgnoreCase)){

                    WriteLine($"Patient has been found in ward {i + 1}, Bed {j + 1}:");
                    wards[i,j].Display();
                    foundPatient = true;
                }
            }
        }
        if(!foundPatient){
            WriteLine($"Patient {name} has not been found in any ward");
        }
    }

    public void dischargePatient(string name){
        for(int i = 0; i < wards.GetLength(0); i++){
            for(int j = 0; j < wards.GetLength(1); j++){
                if(wards[i, j] != null && wards[i,j].Name.Equals(name, StringComparison.OrdinalIgnoreCase)){
                    
                    WriteLine($"Patient {name} discharged from hospital ward");
                    wards[i,j] = null;
                    wardCapacities[i]--;
                    return;
                }
            }
        }
        WriteLine($"Patient {name} not found in any ward");
    }

    public void DisplayPatients()
    {
        for (int i = 0; i < wards.GetLength(0); i++)
        {
            Console.WriteLine($"Ward {i + 1}:");
            for (int j = 0; j < wards.GetLength(1); j++)
            {
                if (wards[i, j] != null)
                {
                    //calling method from Patient class
                    wards[i, j].Display();
                }
                else
                {
                    WriteLine("Empty Bed");
                }
            }
            WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Hospital hospital = new Hospital(3, 4);
        //Main method is the entry point and provides menu-drive interface
        //so as to allow users to interact with the hospital management system
        int option;
        //while loop to allow the menu to iterate
        while (true)
        {
            WriteLine("----THE METROPOLITAN GENERAL HOSPITAL MANAGEMENT SYSTEM---");
            WriteLine("--------------HOSPITAL MANAGEMENT SYSTEM MENU-------------");
            WriteLine("1. Add a Patient\n2. Display Patients\n3. Discharge a Patient\n4. Display Ward Information\n5. Search for a Patient\n6. Exit");
            WriteLine("Enter your choice: ");
            option = Convert.ToInt32(ReadLine());

            switch (option)
            {
                case 1:
                //User Input for patient Data
                    Write("Enter Patient Name: ");
                    string name = ReadLine();

                    Write("Enter Patient Age: ");
                    int age = int.Parse(ReadLine());

                    Write("Enter Patient Condition: ");
                    string condition = ReadLine();
                    
                    Write("Enter Ward Number (1-3): ");
                    int ward = int.Parse(ReadLine()) - 1;

                    hospital.AddPatient(ward, name, age, condition);
                break;
                case 2:
                    hospital.DisplayPatients();
                break;
                case 3:
                    WriteLine("Enter the name of the patient to be discharged: ");
                    string nameDischarge = Console.ReadLine();
                    hospital.dischargePatient(nameDischarge);
                break;
                case 4:
                    hospital.DisplayWardInformation();
                break;
                case 5:
                    WriteLine("Enter the Patients name you wish to search: ");
                    string nameSearch = Console.ReadLine();
                    hospital.searchPatient(nameSearch);
                break;
                case 6:
                    return;
                default:
                    WriteLine("Invalid choice. Please try again.");
                break;
            }
        }
    }
}


