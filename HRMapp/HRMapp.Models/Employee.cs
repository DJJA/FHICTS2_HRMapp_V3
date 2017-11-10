using System;
using System.Collections.Generic;
using System.Text;

namespace HRMapp.Models
{
    public abstract class Employee
    {
        private string firstName, lastName, emailAddress;
        private int phoneNumber;

        public int Id { get; private set; }

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    firstName = value;
                }
                else
                {
                    throw new ArgumentException("Voornaam moet ingevuld worden.");
                }
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Achternaam moet ingevuld worden.");
                }
                lastName = value;
            }
        }

        public int PhoneNumber
        {
            get => phoneNumber;
            private set
            {
                if (value.ToString().Length != 10)
                {
                    throw new ArgumentException("Een telefoonnummer moet 10 cijfers lang zijn.");
                }
                phoneNumber = value;

            }
        }

        public string EmailAddress
        {
            get => emailAddress;
            private set
            {
                if (!ValidateEmailAddress(value))
                {
                    throw new ArgumentException("Emailadres voldoet niet aan het juiste formaat.");
                }
                emailAddress = value;
            }
        }

        public string Street { get; private set; }
        public string HouseNumber { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }

        protected Employee(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        protected Employee(int id, string firstName, string lastName, int phoneNumber, string emailAddress,
            string street, string houseNumber, string zipCode, string city)
            : this(id, firstName, lastName)
        {
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Street = street;
            HouseNumber = houseNumber;
            ZipCode = zipCode;
            City = city;
        }

        private bool ValidateEmailAddress(string emailAddress)
        {
            if (!emailAddress.Contains("@")) return false;
            int indexOfAtSymbol = emailAddress.IndexOf('@');
            if (indexOfAtSymbol < 1) return false;
            if (!emailAddress.Substring(indexOfAtSymbol).Contains(".")) return false;
            int indexOfDotAfterAtSymbol = emailAddress.IndexOf('.', indexOfAtSymbol);
            if ((indexOfAtSymbol + 1) - indexOfDotAfterAtSymbol < 1) return false;
            if ((indexOfDotAfterAtSymbol + 1) - (emailAddress.Length - 1) < 1) return false;
            return true;
        }
    }
}
