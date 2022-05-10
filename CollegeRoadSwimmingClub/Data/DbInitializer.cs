using CollegeRoadSwimmingClub.Models;

namespace CollegeRoadSwimmingClub.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CRSCContext context)
        {
            // check for prior initialization
            if (context.Roles.Any())
            {
                return;
            }

            var defaultRoles = new Role[]
            {
                new Role { Name = "Member", Users = new List<User>() },
                new Role { Name = "Coach", Users = new List<User>() },
                new Role { Name = "Administrator", Users = new List<User>() }
            };

            context.Roles.AddRange(defaultRoles);
            context.SaveChanges();

            var sampleUsers = new User[]
            {
                new User { Username = "aanderson", Password = "AQAAAAEAACcQAAAAEDU+5OnW55BMc4POCxpeIWQaRfunkLCGBhH8woHpC7Ma3x+Vr3rvmWuckVOpGToOog==" },
                new User { Username = "bbrandt", Password = "AQAAAAEAACcQAAAAEDU+5OnW55BMc4POCxpeIWQaRfunkLCGBhH8woHpC7Ma3x+Vr3rvmWuckVOpGToOog=="},
                new User { Username = "ccooper", Password = "AQAAAAEAACcQAAAAEDU+5OnW55BMc4POCxpeIWQaRfunkLCGBhH8woHpC7Ma3x+Vr3rvmWuckVOpGToOog=="},
            };

            context.Users.AddRange(sampleUsers);
            context.SaveChanges();

            defaultRoles[0].Users.Add(sampleUsers[0]);
            defaultRoles[0].Users.Add(sampleUsers[1]);
            defaultRoles[0].Users.Add(sampleUsers[2]);
            defaultRoles[1].Users.Add(sampleUsers[1]);
            defaultRoles[1].Users.Add(sampleUsers[2]);
            defaultRoles[2].Users.Add(sampleUsers[0]);

            context.SaveChanges();

            var sampleMembers = new Member[]
            {
                new Member
                {
                    FirstName = "Alan",
                    LastName = "Anderson",
                    Address1 = "123 Main Street",
                    Address2 = null,
                    Town = "Stoke-on-Trent",
                    County = "Staffordshire",
                    Postcode = "ST1 2AB",
                    Telephone = "01782 123456",
                    Email = "alan@anderson.me",
                    DateOfBirth = new DateTime(1983, 2, 17),
                    IsSwimmer = false,
                    Gender = Gender.Male,
                    User = sampleUsers[0],
                    UserId = sampleUsers[0].Id,
                    UserMemberLink = UserMemberLink.Self
                },
                new Member
                {
                    FirstName = "Archie",
                    LastName = "Anderson",
                    Address1 = "123 Main Street",
                    Address2 = null,
                    Town = "Stoke-on-Trent",
                    County = "Staffordshire",
                    Postcode = "ST1 2AB",
                    Telephone = "01782 123456",
                    Email = "alan@anderson.me",
                    DateOfBirth = new DateTime(2013, 6, 13),
                    IsSwimmer = true,
                    Gender = Gender.Male,
                    User = sampleUsers[0],
                    UserId = sampleUsers[0].Id,
                    UserMemberLink = UserMemberLink.Parent
                },
                new Member
                {
                    FirstName = "Beth",
                    LastName = "Brandt",
                    Address1 = "5 Beech Avenue",
                    Address2 = null,
                    Town = "Stoke-on-Trent",
                    County = "Staffordshire",
                    Postcode = "ST3 4CD",
                    Telephone = "01782 234567",
                    Email = "b.brandt@mail.com",
                    DateOfBirth = new DateTime(1999, 11, 25),
                    IsSwimmer = true,
                    Gender = Gender.Female,
                    User = sampleUsers[1],
                    UserId = sampleUsers[1].Id,
                    UserMemberLink = UserMemberLink.Self
                },
                new Member
                {
                    FirstName = "Cassandra",
                    LastName = "Cooper",
                    Address1 = "12a Melton House",
                    Address2 = "Barker Road",
                    Town = "Newcastle-under-Lyme",
                    County = "Staffordshire",
                    Postcode = "ST5 5HF",
                    Telephone = "01782 345678",
                    Email = "ccooper@yahoo.co.uk",
                    DateOfBirth = new DateTime(1988, 9, 2),
                    IsSwimmer = true,
                    Gender = Gender.Female,
                    User = sampleUsers[2],
                    UserId = sampleUsers[2].Id,
                    UserMemberLink = UserMemberLink.Self
                },
                new Member
                {
                    FirstName = "Chris",
                    LastName = "Cooper",
                    Address1 = "12a Melton House",
                    Address2 = "Barker Road",
                    Town = "Newcastle-under-Lyme",
                    County = "Staffordshire",
                    Postcode = "ST5 5HF",
                    Telephone = "01782 345678",
                    Email = "ccooper@yahoo.co.uk",
                    DateOfBirth = new DateTime(2012, 5, 29),
                    IsSwimmer = true,
                    Gender = Gender.Male,
                    User = sampleUsers[2],
                    UserId = sampleUsers[2].Id,
                    UserMemberLink = UserMemberLink.Parent
                }
            };

            context.Members.AddRange(sampleMembers);
            context.SaveChanges();

            var sampleClasses = new Class[]
            {
                new Class { Name = "Boys Under-13s", Gender= Gender.Male, MinAge=null, MaxAge=12},
                new Class { Name = "Senior Women", Gender = Gender.Female, MinAge=18, MaxAge=null},
                new Class { Name = "Mixed Juniors", Gender=Gender.Mixed, MinAge=7, MaxAge=11}
            };
            context.Classes.AddRange(sampleClasses);
            context.SaveChanges();

            var sampleEvents = new Event[]
            {
                new Event { Stroke= Stroke.Freestyle, Distance = 50},
                new Event { Stroke = Stroke.Freestyle, Distance = 100},
                new Event { Stroke = Stroke.Freestyle, Distance = 200},
                new Event { Stroke = Stroke.Freestyle, Distance = 400},
                new Event { Stroke = Stroke.Freestyle, Distance = 800},
                new Event { Stroke = Stroke.Freestyle, Distance = 1500},
                new Event { Stroke = Stroke.Breaststroke, Distance = 50},
                new Event { Stroke = Stroke.Breaststroke, Distance = 100},
                new Event { Stroke = Stroke.Breaststroke, Distance = 200},
                new Event { Stroke = Stroke.Backstroke, Distance = 50},
                new Event { Stroke = Stroke.Backstroke, Distance = 100},
                new Event { Stroke = Stroke.Backstroke, Distance = 200},
                new Event { Stroke = Stroke.Butterfly, Distance = 50},
                new Event { Stroke = Stroke.Butterfly, Distance = 100},
                new Event { Stroke = Stroke.Butterfly, Distance = 200},
                new Event { Stroke = Stroke.Medley, Distance = 100},
                new Event { Stroke = Stroke.Medley, Distance = 200},
                new Event { Stroke = Stroke.Medley, Distance = 400}
            };

            context.Events.AddRange(sampleEvents);
            context.SaveChanges();

            var sampleGalas = new Gala[]
            {
                new Gala {
                    Name = "Easter Gala 2022",
                    StartDate=new DateTime(2022, 4, 16),
                    EndDate=new DateTime(2022, 4, 17),
                    Location="College Road Baths"
                },
                new Gala
                {
                    Name = "Winter Gala 2022",
                    StartDate = new DateTime(2022, 2, 5),
                    EndDate=new DateTime(2022, 2, 6),
                    Location="Haversham Leisure Centre"
                },
                new Gala
                {
                    Name = "CRSC Charity Exhibition",
                    StartDate = new DateTime(2022, 3, 12),
                    EndDate=new DateTime(2022, 3, 12),
                    Location="College Road Baths"
                }
            };

            context.Galas.AddRange(sampleGalas);
            context.SaveChanges();

            var sampleRaces = new Race[]
            {
                new Race
                {
                    Class = sampleClasses[0],
                    ClassId = sampleClasses[0].Id,
                    DateTime = new DateTime(2022, 4, 16),
                    Event = sampleEvents[0],
                    EventId = sampleEvents[0].Id,
                    Gala = sampleGalas[0],
                    GalaId = sampleGalas[0].Id
                },
                new Race
                {
                    Class = sampleClasses[0],
                    ClassId = sampleClasses[0].Id,
                    DateTime = new DateTime(2022, 4, 16),
                    Event = sampleEvents[1],
                    EventId = sampleEvents[1].Id,
                    Gala = sampleGalas[0],
                    GalaId = sampleGalas[0].Id
                },
                new Race
                {
                    Class = sampleClasses[1],
                    ClassId = sampleClasses[1].Id,
                    DateTime = new DateTime(2022, 4, 16),
                    Event = sampleEvents[2],
                    EventId = sampleEvents[2].Id,
                    Gala = sampleGalas[1],
                    GalaId = sampleGalas[1].Id
                }
            };

            context.Races.AddRange(sampleRaces);
            context.SaveChanges();

            var sampleRaceResults = new RaceResult[]
            {
                new RaceResult {
                    Race = sampleRaces[0],
                    RaceId = sampleRaces[0].Id,
                    Position = 2,
                    Swimmer = sampleMembers[1],
                    SwimmerId = sampleMembers[1].Id,
                    Time = new TimeSpan(0, 0, 1, 35, 120)
                },
            };

            context.RaceResults.AddRange(sampleRaceResults);
            context.SaveChanges();

            var sampleTrainingResults = new TrainingResult[]
            {
                new TrainingResult {
                    Swimmer = sampleMembers[0],
                    SwimmerId = sampleMembers[0].Id,
                    Time = new TimeSpan(0, 0, 1, 34, 220),
                    Event = sampleEvents[0],
                    EventId = sampleEvents[0].Id,
                    Date = new DateTime(2022, 1, 20)
                }
            };

            context.TrainingResults.AddRange(sampleTrainingResults);
            context.SaveChanges();
        }
    }
}
