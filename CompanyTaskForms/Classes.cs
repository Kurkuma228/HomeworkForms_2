using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CompanyTaskForms
{
    [XmlRoot("GlobalCompany")]
    public class GlobalCompany
    {
        public Employee Employee { get; set; }
        public Office Office { get; set; }
        public Project Project { get; set; }
    }
    public class Employee
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string EmploymentDate { get; set; }

        public Address Address { get; set; }
        public Contact Contact { get; set; }

        public Employee()
        {
            Address = new Address();
            Contact = new Contact();
        }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }

        public Address()
        {
            City = string.Empty;
            Street = string.Empty;
        }
    }

    public class Contact
    {
        public string Email { get; set; }
        public string Phone { get; set; }

        public Contact()
        {
            Email = string.Empty;
            Phone = string.Empty;
        }
    }
    public class Office
    {
        public Location Location { get; set; }
        public OfficeAddress Address { get; set; }
        public int EmployeesCount { get; set; }
        public string EstablishedDate { get; set; }
        public string Manager { get; set; }
        public Departments Departments { get; set; }

        public Office()
        {
            Location = new Location();
            Address = new OfficeAddress();
            Departments = new Departments();
        }
    }

    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }

        public Location()
        {
            Country = string.Empty;
            City = string.Empty;
        }
    }

    public class OfficeAddress
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public OfficeAddress()
        {
            Street = string.Empty;
            PostalCode = string.Empty;
        }
    }

    public class Departments
    {
        private List<string> departmentList;

        public Departments()
        {
            departmentList = new List<string>();
        }

        [XmlElement("Department")]
        public string[] DepartmentArray
        {
            get { return departmentList.ToArray(); }
            set
            {
                if (value != null)
                    departmentList.AddRange(value);
            }
        }

        public List<string> GetDepartmentList()
        {
            return departmentList;
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal BudgetUSD { get; set; }

        public TeamMembers Team { get; set; }
        public Regions Regions { get; set; }

        public Project()
        {
            Team = new TeamMembers();
            Regions = new Regions();
        }
    }

    public class TeamMembers
    {
        private List<Member> members;

        public TeamMembers()
        {
            members = new List<Member>();
        }

        [XmlElement("Member")]
        public Member[] MemberArray
        {
            get { return members.ToArray(); }
            set
            {
                if (value != null)
                    members.AddRange(value);
            }
        }

        public List<Member> GetMembers()
        {
            return members;
        }
    }

    public class Member
    {
        public string Name { get; set; }
        public string Role { get; set; }

        public Member()
        {
            Name = string.Empty;
            Role = string.Empty;
        }
    }

    public class Regions
    {
        private List<string> regionList;

        public Regions()
        {
            regionList = new List<string>();
        }

        [XmlElement("Region")]
        public string[] RegionArray
        {
            get { return regionList.ToArray(); }
            set
            {
                if (value != null)
                    regionList.AddRange(value);
            }
        }

        public List<string> GetRegionList()
        {
            return regionList;
        }
    }
}
