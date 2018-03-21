using System;
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
    public List<int> OrganizerIDs;
    public string Tags;
    public Link[] Links;
    public string Description;
    public Requirement[] Requirements;
    public List<int> VolunteerIDs;
}

public class AppData 
{
    public const string MAPS_API_KEY = "AIzaSyDtOGHPkTIRv18aUot0G-GeQ0lgrpqvOMo";
    public const string GEOCODE_API_KEY = "AIzaSyC51TwSVwidbd6Qu9LpWVgeHPTxoSuWYuQ";

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
                    Path = "https://www.facebook.com"
                }
            },
            About = "HSC Coordinator",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Ardisj Nesson",
            Links = new Link[] { },
            About = "City of Winnipeg Employee",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Haroun Titterington",
            Links = new Link[] { },
            About = "Government of Manitoba Employee",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Felizio Martini",
            Links = new Link[] { },
            About = "Human Rights Museum Volunteer Outreach Coordinator",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Colin Smith",
            Links = new Link[] { },
            About = "Human Rights Activist",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Tabby Archuleta",
            Links = new Link[] { },
            About = "Senior Citizen",
            PreviousExperienceIDs = new int[] { }
        },
        new User()
        {
            Name = "Clarke Mishkin",
            Links = new Link[] { },
            About = "WAG Volunteer Director",
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
            OrganizerIDs = new List<int>() { 0 },
            Tags = "Medicine CustomerService Exercise",
            Links = new Link[] 
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Direct or escort patients and/or families/friends to the appropriate areas as needed, provide information, support to patients and visitors.",
            Requirements = new Requirement[] { Requirement.BackgroundCheck },
            VolunteerIDs = new List<int>() { 7 }
        },
        new EventInfo
        {
            Name = "Fun at the Forks",
            Time = GetRandomFutureDate(),
            Location = "The Forks, Winnipeg MB",
            Latitude = 49.8873789f,
            Longitude = -97.1333737f,
            OrganizerIDs = new List<int>() { 1 },
            Tags = "Fun",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.theforks.com/" }
            },
            Description = "Come out The Forks to help facilitate winter activities!",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>() { 7 }
        },
        new EventInfo
        {
            Name = "Legislative Attendant",
            Time = GetRandomFutureDate(),
            Location = "Manitoba Legislative Building",
            Latitude = 49.889359f,
            Longitude = -97.1545129f,
            OrganizerIDs = new List<int>() { 2 },
            Tags = "Gov Manitoba",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Help out at the legislature.",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Museum Guide",
            Time = GetRandomFutureDate(),
            Location = "Canadian Museum for Human Rights",
            Latitude = 49.8864282f,
            Longitude = -97.1472173f,
            OrganizerIDs = new List<int>() { 3, 4 },
            Tags = "Museum HumanRights",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Volunteer to help in showcasing the human rights museum.",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Help with Shoveling",
            Time = GetRandomFutureDate(),
            Location = "145 Oakwood Ave",
            Latitude = 49.8663082f,
            Longitude = -97.1284419f,
            OrganizerIDs = new List<int>() { 5 },
            Tags = "Winter",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "I am a senior citizen who is unable to shovel my driveway.",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "WAG Volunteer",
            Time = GetRandomFutureDate(),
            Location = "Winnipeg Art Gallery",
            Latitude = 49.8902412f,
            Longitude = -97.1483334f,
            OrganizerIDs = new List<int>() { 6 },
            Tags = "WAG",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Come help out in the Winnipeg Art Gallery!",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Science Fair Judge",
            Time = GetRandomFutureDate(),
            Location = "General Wolfe Junior High School",
            Latitude = 49.8943475f,
            Longitude = -97.1699769f,
            OrganizerIDs = new List<int>() { 6 },
            Tags = "Education",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Come help judge!",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Event Volunteer",
            Time = GetRandomFutureDate(),
            Location = "Balmoral Hall School",
            Latitude = 49.8790499f,
            Longitude = -97.1581215f,
            OrganizerIDs = new List<int>() { 6 },
            Tags = "Education",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Come help out!",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Help with Moving",
            Time = GetRandomFutureDate(),
            Location = "918 Alfred Avenue",
            Latitude = 49.9250862f,
            Longitude = -97.1695618f,
            OrganizerIDs = new List<int>() { 6 },
            Tags = "Exercise",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Come help out!",
            Requirements = new Requirement[] { },
            VolunteerIDs = new List<int>()
        },
        new EventInfo
        {
            Name = "Museum Volunteer",
            Time = GetRandomFutureDate(),
            Location = "The Fire Fighters Museum of Winnipeg",
            Latitude = 49.9021431f,
            Longitude = -97.1355803f,
            OrganizerIDs = new List<int>() { 6 },
            Tags = "Museum",
            Links = new Link[]
            {
                new Link() { Type = Link.LinkType.Default, Path = "http://www.hsc.mb.ca" }
            },
            Description = "Come help out!",
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
