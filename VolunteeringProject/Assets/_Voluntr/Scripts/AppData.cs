﻿using System;
using System.Collections.Generic;

public struct User {
    public string Name;
    public Link[] Links;
    public string About;
    public int[] PreviousExperienceIDs;

    public int ID {  get { return AppData.Users.IndexOf(this); } }
}

public struct Link {
    public enum LinkType {
        Default,
        Facebook,
    }

    public LinkType Type;
    public string Path;
}

public enum Requirement {
    BackgroundCheck
}

public struct EventInfo {
    public string Name;
    public DateTime Time;
    public string Location;
    public float Latitude;
    public float Longitude;
    public int[] OrganizerIDs;
    public string Tags;
    public Link[] Links;
    public string Description;
    public Requirement[] Requirements;
    public List<int> VolunteerIDs;
}

public class AppData 
{
    public static List<User> Users = new List<User>()
    {
        new User()
        {
            Name = "Gasper Haar",
            Links = new Link[]
            {
                new Link()
                {
                    Type = Link.LinkType.Facebook,
                    Path = ""
                }
            },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Ardisj Nesson",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Haroun Titterington",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Felizio Martini",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Colin Smith",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Tabby Archuleta",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Clarke Mishkin",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Conni Todorovic",
            Links = new Link[] { },
            About = "Volunteer",
            PreviousExperienceIDs = new int[] { 0 }
        },
    };

    public static List<EventInfo> Events = new List<EventInfo>()
    {
        new EventInfo
        {
            Name = "HSC Ambassador",
            Time = GetRandomFutureDate(),
            Location = "820 Sherbrook Street, Winnipeg MB",
            Latitude = 49.903159f,
            Longitude = -97.157414f,
            OrganizerIDs = new int[] { 0 },
            Tags = "Medicine CustomerService Exercise",
            Links = new Link[] 
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Direct or escort patients and/or families/friends to the appropriate areas as needed, provide information, support to patients and visitors.",
            Requirements = new Requirement[] { Requirement.BackgroundCheck },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event 2",
            Time = GetRandomFutureDate(),
            Location = "UofM",
            Latitude = 0,
            Longitude = 0,
            OrganizerIDs = new int[] { 1 },
            Tags = "",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event 3",
            Time = GetRandomFutureDate(),
            Location = "UofM",
            Latitude = 0,
            Longitude = 0,
            OrganizerIDs = new int[] { 2 },
            Tags = "",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "UofM",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event 4",
            Time = GetRandomFutureDate(),
            Location = "UofM",
            Latitude = 0,
            Longitude = 0,
            OrganizerIDs = new int[] { 3, 4 },
            Tags = "",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event 5",
            Time = GetRandomFutureDate(),
            Location = "UofM",
            Latitude = 0,
            Longitude = 0,
            OrganizerIDs = new int[] { 5 },
            Tags = "",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event 6",
            Time = GetRandomFutureDate(),
            Location = "UofM",
            Latitude = 0,
            Longitude = 0,
            OrganizerIDs = new int[] { 6 },
            Tags = "",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
    };

    public static DateTime GetRandomFutureDate(int maxDays=30, int maxHours=24, int maxMinutes=60, int maxSeconds=60) {
        DateTime dateTime = DateTime.Now;

        dateTime = dateTime.AddDays(UnityEngine.Random.Range(0, maxDays));
        dateTime = dateTime.AddHours(UnityEngine.Random.Range(0, maxHours));
        dateTime = dateTime.AddMinutes(UnityEngine.Random.Range(0, maxMinutes));
        dateTime = dateTime.AddSeconds(UnityEngine.Random.Range(0, maxSeconds));

        return dateTime;
    }
}
