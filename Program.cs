using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace DubaiPolice 
{
    class Database {

        public static void Input(string Endpoint, string[] Metadata) {

        //  Initialize variables
            FilterDefinition<BsonDocument> Filter;
            UpdateDefinition<BsonDocument> Update;
            DateTime Event;
        //  Connect database
            var Client = new MongoClient("mongodb://" + ConfigurationManager.AppSettings["MongoDB.Username"] + ":" + ConfigurationManager.AppSettings["MongoDB.Password"] + "@" + ConfigurationManager.AppSettings["MongoDB.Server"] + ":" + ConfigurationManager.AppSettings["MongoDB.Port"]);
            var Database = Client.GetDatabase(ConfigurationManager.AppSettings["MongoDB.Database"]);
        //  Select collection
            var Collection = Database.GetCollection<BsonDocument>(Endpoint);
        //  Handle document
            var Document = new BsonDocument();

            //  Register datapoint
                if (Endpoint == "Datapoint") {
                //  Validate input
                    Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                    var Datapoints = Database.GetCollection<BsonDocument>("Datapoint").Find(Filter).ToList();
                    //  Avoid duplicates
                        if (Datapoints.Count > 0)
                        throw new System.ArgumentException("Duplicate record");
                    //  Count parameters
                        if (Metadata.Length < 2)
                        throw new System.ArgumentException("Missing parameters");
                //  Build location
                    var Location = new BsonDocument();
                    //  Record event
                        Event = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        Location.Add("Event", Event);
                    //  Append coordinates
                        Location.Add("Coordinates", new BsonArray { Metadata[1], Metadata[2] });
                //  Append metadata
                    Document.Add("IdNumber", Metadata[0]);
                    Document.Add("Location", new BsonArray { Location });
                    if (Metadata.Length > 2) Document.Add("Range", Metadata[3]);
                //  Insert document
                    Collection.InsertOne(Document); }

            //  Register people
                else if (Endpoint == "People") {
                //  Validate input
                    Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                    var People = Database.GetCollection<BsonDocument>("People").Find(Filter).ToList();
                    //  Avoid duplicates
                        if (People.Count != 0)
                        throw new System.ArgumentException("Duplicate record");
                    //  Count parameters
                        if (Metadata.Length < 4)
                        throw new System.ArgumentException("Missing parameters");
                //  Append common metadata
                    Document.Add("IdNumber", Metadata[0]);
                    Document.Add("Nationality", Metadata[1]);
                    Document.Add("FirstName", Metadata[2]);
                //  Append surname/s metadata
                    if (Metadata.Length == 5) {
                    //  Middle and last name
                        Document.Add("MiddleName", Metadata[3]);
                        Document.Add("LastName", Metadata[4]); }
                    //  Just last name
                        else Document.Add("LastName", Metadata[3]);
                //  Insert document
                    Collection.InsertOne(Document); }

            //  Register object
                else if (Endpoint == "Object") {
                //  Validate input
                    if (Metadata.Length < 2)
                    throw new System.ArgumentException("Missing parameters");
                //  Build ownership
                    var Ownership = new BsonDocument();
                    //  Retrieve owners
                        Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[1]);
                        var Owners = Database.GetCollection<BsonDocument>("People").Find(Filter).ToList();
                    //  Append metadata
                        if (Owners.Count != 0) {
                        //  Record event
                            Event = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            Ownership.Add("Event", Event);
                        //  Append first owner
                            Ownership.Add("ObjectId", Owners.First()["_id"]); }
                    //  Handle exceptions
                        else if (Metadata[1].Length == 15) throw new System.ArgumentException("Invalid ID");
                        else if (Owners.Count == 0) Ownership = null;
                //  Build attributes
                    var Attributes = new BsonDocument();
                    //  Build tagcloud
                        var Tagcloud = new BsonDocument();
                        //  Skip parameters
                            if (Ownership != null) {
                                foreach (string Attribute in Metadata.Skip(2).ToArray())
                                Tagcloud.Add(Attribute.Split(":")[0], Attribute.Split(":")[1]); }
                        //  Append directly
                            else if (Ownership == null) {
                                foreach (string Attribute in Metadata.Skip(1).ToArray())
                                Tagcloud.Add(Attribute.Split(":")[0], Attribute.Split(":")[1]); }
                    //  Append metadata
                        if (Tagcloud.Count() != 0) {
                        //  Record event
                            Event = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                            Attributes.Add("Event", Event);
                        //  Append metadata
                            Attributes.Add("Tags", Tagcloud); }
                    //  Handle exceptions
                        else if (Tagcloud.Count() == 0) Attributes = null;
                //  Handle object
                    Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                    var Objects = Collection.Find(Filter).ToList();
                    //  Create object
                        if (Objects.Count == 0) {
                        //  Append metadata
                            Document.Add("IdNumber", Metadata[0]);
                            if (Ownership != null) Document.Add("Ownership", new BsonArray { Ownership });
                            if (Attributes != null) Document.Add("Attributes", new BsonArray { Attributes });
                        //  Insert document
                            Collection.InsertOne(Document); }
                    //  Update object
                        else if (Objects.Count > 0) {
                        //  Transfer ownership
                            if (Ownership != null) {
                                Update = Builders<BsonDocument>.Update.Push("Ownership", Ownership);
                                Collection.UpdateOne(Filter, Update); }
                        //  Include attributes
                            if (Attributes != null) { 
                                Update = Builders<BsonDocument>.Update.Push("Attributes", Attributes);
                                Collection.UpdateOne(Filter, Update); } } }

        //  Return signal
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Completed");
            Console.WriteLine();
            Console.ResetColor();
        //  Return menu
            Program.Main(new string[0]); }

        public static void Beacon(string Endpoint, string[] Metadata) {

        //  Initialize variables
            FilterDefinition<BsonDocument> Filter = null;
            UpdateDefinition<BsonDocument> Update;
            DateTime Event;
        //  Connect database
            var Client = new MongoClient("mongodb://" + ConfigurationManager.AppSettings["MongoDB.Username"] + ":" + ConfigurationManager.AppSettings["MongoDB.Password"] + "@" + ConfigurationManager.AppSettings["MongoDB.Server"] + ":" + ConfigurationManager.AppSettings["MongoDB.Port"]);
            var Database = Client.GetDatabase(ConfigurationManager.AppSettings["MongoDB.Database"]);
        //  Select collection
            var Collection = Database.GetCollection<BsonDocument>(Endpoint);

        //  Build location
            var Location = new BsonDocument();
            //  Record event
                Event = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                Location.Add("Event", Event);
            //  Retrieve coordinates
                if (Endpoint == "Datapoint") Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                else if (Endpoint != "Datapoint") Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[1]);
                var Datapoints = Database.GetCollection<BsonDocument>("Datapoint").Find(Filter).ToList();
                //  Validate input
                    if (Datapoints.Count == 0)
                    throw new System.ArgumentException("Invalid datapoint");
                //  Append datapoint
                    else if (Datapoints.Count != 0)
                    Location.Add("Datapoint", Datapoints.First()["_id"]);
                //  Import coordinates
                    if (Metadata.Length == 4)
                    Location.Add("Coordinates", new BsonArray { Metadata[2], Metadata[3] });
                //  Inherit coordinates
                    else if (Metadata.Length < 4)
                    Location.Add("Coordinates", Datapoints.First()["Location"].AsBsonArray.Last()["Coordinates"]);
            //  Initialize weight
                Location.Add("Weight", 100);

        //  Track datapoint
            if (Endpoint == "Datapoint") {
            //  Validate input
                if (Metadata.Length < 2)
                throw new System.ArgumentException("Missing parameters");
            //  Append location
                Update = Builders<BsonDocument>.Update.Push("Location", Location);
                Collection.UpdateOne(Filter, Update); }

        //  Track people
            if (Endpoint == "People") {
            //  Validate input
                if (Metadata.Length < 2)
                throw new System.ArgumentException("Missing parameters");
            //  Retrieve people
                Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                var People = Collection.Find(Filter).ToList();
            //  Append location
                if (People.Count != 0) {
                    Update = Builders<BsonDocument>.Update.Push("History", Location);
                    Collection.UpdateOne(Filter, Update); }
            //  Handle exeptions
                else if (People.Count == 0)
                throw new System.ArgumentException("Invalid ID"); }

        //  Track object
            else if (Endpoint == "Object") {
            //  Validate input
                if (Metadata.Length < 2)
                throw new System.ArgumentException("Missing parameters");
            //  Retrieve objects
                Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
                var Objects = Collection.Find(Filter).ToList();
            //  Append location
                if (Objects.Count != 0) {
                //  Track object
                    Update = Builders<BsonDocument>.Update.Push("History", Location);
                    Collection.UpdateOne(Filter, Update);
                //  Track owners
                    var History = Objects.First()["Ownership"].AsBsonArray;
                    //  Recalculate weight
                        var Weight = 100 / ( History.Count() + 1 );
                        Location.Set("Weight", Weight);
                    //  Update owners
                        foreach (BsonDocument Ownership in History) {
                        //  Target owner
                            Filter = Builders<BsonDocument>.Filter.Eq("_id", Ownership["ObjectId"]);
                            var Owner = Database.GetCollection<BsonDocument>("People").Find(Filter).ToList().First();
                        //  Append location
                            Update = Builders<BsonDocument>.Update.Push("History", Location);
                            Database.GetCollection<BsonDocument>("People").UpdateOne(Filter, Update); } }
            //  Handle exeptions
                else if (Objects.Count == 0)
                throw new System.ArgumentException("Invalid object"); }

        //  Return signal
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Completed");
            Console.WriteLine();
            Console.ResetColor();
        //  Return menu
            Program.Main(new string[0]); } }

    class Map {

        public static void Scheme(string Endpoint) {
        
        //  Initialize variables
            FilterDefinition<BsonDocument> Filter;
            Int32 Node = 0;
            BsonValue Location = null;
            BsonValue Range = null;
            String URL = "http://localhost/Client.php?action=scheme";
        //  Connect database
            var Client = new MongoClient("mongodb://" + ConfigurationManager.AppSettings["MongoDB.Username"] + ":" + ConfigurationManager.AppSettings["MongoDB.Password"] + "@" + ConfigurationManager.AppSettings["MongoDB.Server"] + ":" + ConfigurationManager.AppSettings["MongoDB.Port"]);
            var Database = Client.GetDatabase(ConfigurationManager.AppSettings["MongoDB.Database"]);
        //  Select collection
            var Collection = Database.GetCollection<BsonDocument>(Endpoint);

        //  Retrieve entities
            Filter = new BsonDocument();
            var Entities = Collection.Find(Filter).ToList();
        //  Build URL
            if (Entities.Count != 0) {
            //  Retrieve entity
                foreach (BsonDocument Entity in Entities) { Node++;
                if (Endpoint != "Datapoint") { Location = Entity["History"].AsBsonArray.Last(); }
                else if (Endpoint == "Datapoint") { Location = Entity["Location"].AsBsonArray.Last();
            //  Assign range (if applicable)
                if (Entity.Contains("Range") == true) Range = Entity["Range"];
                else if (Entity.Contains("Range") == false) Range = "0"; }
            //  Append metadata
                URL = URL + "&entity[" + Node + "][identifier]=" + System.Uri.EscapeDataString(Entity["IdNumber"].ToString()) + "&entity[" + Node + "][latitude]=" + Location["Coordinates"][0] + "&entity[" + Node + "][longitude]=" + Location["Coordinates"][1];
                if (Endpoint == "Datapoint") URL = URL + "&entity[" + Node + "][range]=" + Range; }
            //  Return URL
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine(URL);
                Console.WriteLine();
                Console.ResetColor();
            //  Return menu
                Program.Main(new string[0]); }
        //  Handle exeptions
            else if (Entities.Count == 0)
            throw new System.ArgumentException("Empty scheme"); }

        public static void Trace(string Endpoint, string[] Metadata) {
        
        //  Initialize variables
            FilterDefinition<BsonDocument> Filter;
            BsonArray History = null;
            Int32 Beacon = 0;
            String URL = "http://dubaipolice/Client.php?action=trace";
        //  Connect database
            var Client = new MongoClient("mongodb://" + ConfigurationManager.AppSettings["MongoDB.Username"] + ":" + ConfigurationManager.AppSettings["MongoDB.Password"] + "@" + ConfigurationManager.AppSettings["MongoDB.Server"] + ":" + ConfigurationManager.AppSettings["MongoDB.Port"]);
            var Database = Client.GetDatabase(ConfigurationManager.AppSettings["MongoDB.Database"]);
        //  Select collection
            var Collection = Database.GetCollection<BsonDocument>(Endpoint);

        //  Validate input
            if (Metadata.Length < 1)
            throw new System.ArgumentException("Missing parameters");
        //  Retrieve entity
            Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
            var Entity = Collection.Find(Filter).ToList();
        //  Build URL
            if (Entity.Count != 0) {
            //  Retrieve history
                if (Endpoint != "Datapoint") { History = Entity.First()["History"].AsBsonArray; }
                else if (Endpoint == "Datapoint") { History = Entity.First()["Location"].AsBsonArray; }
            //  Append metadata
                foreach (BsonDocument Data in History) { Beacon++;
                URL = URL + "&beacon[" + Beacon + "][event]=" + System.Uri.EscapeDataString(Data["Event"].ToLocalTime().ToString()) + "&beacon[" + Beacon + "][latitude]=" + Data["Coordinates"][0] + "&beacon[" + Beacon + "][longitude]=" + Data["Coordinates"][1]; } 
            //  Return URL
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine(URL);
                Console.WriteLine();
                Console.ResetColor();
            //  Return menu
                Program.Main(new string[0]); }
        //  Handle exeptions
            else if (Entity.Count == 0)
            throw new System.ArgumentException("Invalid ID"); }
            
        public static void Scenario(string Endpoint, string[] Metadata) {
        
        //  Validate input
            if (Metadata.Length < 1)
            throw new System.ArgumentException("Missing parameters");

        //  Initialize variables
            FilterDefinition<BsonDocument> Filter;
            BsonDocument Owner = null;
            BsonValue Ownership = null;
            BsonArray Entities = new BsonArray();
            Int32 Node = 0;
            BsonValue Location = null;
            BsonValue Range = null;
            String URL = "http://localhost/Client.php?action=scenario&realtime=false&people=" + Metadata[0];
        //  Connect database
            var Client = new MongoClient("mongodb://" + ConfigurationManager.AppSettings["MongoDB.Username"] + ":" + ConfigurationManager.AppSettings["MongoDB.Password"] + "@" + ConfigurationManager.AppSettings["MongoDB.Server"] + ":" + ConfigurationManager.AppSettings["MongoDB.Port"]);
            var Database = Client.GetDatabase(ConfigurationManager.AppSettings["MongoDB.Database"]);

        //  Retrieve datapoints
            Filter = new BsonDocument();
            var Datapoints = Database.GetCollection<BsonDocument>("Datapoint").Find(Filter).ToList();
        //  Retrieve people
            Filter = Builders<BsonDocument>.Filter.Eq("IdNumber", Metadata[0]);
            var People = Database.GetCollection<BsonDocument>("People").Find(Filter).ToList();
            //  Retrieve owner
                if (People.Count != 0)
                Owner = People.First();
            //  Handle exeptions
                else if (People.Count == 0)
                throw new System.ArgumentException("Invalid ID");
        //  Retrieve objects
            Filter = new BsonDocument();
            var Objects = Database.GetCollection<BsonDocument>("Object").Find(Filter).ToList();
            //  Filter objects
                if (Objects.Count != 0) {
                foreach (BsonDocument Object in Objects) {
                    Ownership = Object["Ownership"].AsBsonArray.Last();
                    if (Ownership["ObjectId"] == Owner["_id"]) {
            //  Append object
                Entities.Add(Object); } } }

        //  Validate elements
            if (Datapoints.Count == 0 && Entities.Count == 0)
            throw new System.ArgumentException("Empty scenario");
        //  Build URL
            else if (Datapoints.Count != 0 || Entities.Count == 0) {
            //  Append datapoints
                if (Datapoints.Count != 0) {
                //  Retrieve datapoint
                    foreach (BsonDocument Datapoint in Datapoints) { Node++;
                    Location = Datapoint["Location"].AsBsonArray.Last();
                //  Assign range
                    if (Datapoint.Contains("Range") == true) Range = Datapoint["Range"];
                    else if (Datapoint.Contains("Range") == false) Range = "0";
                //  Append metadata
                    URL = URL + "&datapoint[" + Node + "][identifier]=" + System.Uri.EscapeDataString(Datapoint["IdNumber"].ToString()) + "&datapoint[" + Node + "][latitude]=" + Location["Coordinates"][0] + "&datapoint[" + Node + "][longitude]=" + Location["Coordinates"][1] + "&datapoint[" + Node + "][range]=" + Range; }
            //  Count again
                Node = 0; }
            //  Append objects
                if (Entities.Count != 0) {
                    foreach (BsonDocument Entity in Entities) { Node++;
                    URL = URL + "&object[" + Node + "][identifier]=" + System.Uri.EscapeDataString(Entity["IdNumber"].ToString()); } } }
        //  Return URL
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(URL);
            Console.WriteLine();
            Console.ResetColor();
        //  Return menu
            Program.Main(new string[0]); } }

    class Program {

        public static void Main(string[] Arguments) {

        //  Initialize variables
            String Command = null;
            String Endpoint = null;
            String[] Metadata = null;

        //  Service flow
            if (Arguments.Count() > 0) {
            //  Validate input
                if (Arguments.Count() < 2)
                throw new System.ArgumentException("Missing parameters");
            //  Handle input
                Command = Arguments[0]; Endpoint = Arguments[1];
                if (Arguments.Count() > 2) Metadata = Arguments.Skip(2).ToArray(); }

        //  Application flow
            else if (Arguments.Count() == 0) {
            //  Render menu
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("01. Input Datapoint <Identifier> <Latitude> <Longitude> (<Range>)?");
                Console.WriteLine("02. Input People <Emirates_ID> <Nationality> <First_Name> (<Middle_Name>)? <Last_Name>");
                Console.WriteLine("03. Input Object <Object_ID> (<Emirates_ID>)? (<Label:Attribute>)?");
                Console.WriteLine();
                Console.WriteLine("04. Beacon Datapoint <Datapoint_ID> <Latitude> <Longitude>");
                Console.WriteLine("05. Beacon People <Emirates_ID> <Datapoint_ID> (<Latitude> <Longitude>)?");
                Console.WriteLine("06. Beacon Object <Object_ID> <Datapoint_ID> (<Latitude> <Longitude>)?");
                Console.WriteLine();
                Console.WriteLine("07. Scheme Datapoint");
                Console.WriteLine("08. Scheme People");
                Console.WriteLine("09. Scheme Object");
                Console.WriteLine();
                Console.WriteLine("10. Trace Datapoint <Datapoint_ID>");
                Console.WriteLine("11. Trace People <Emirates_ID>");
                Console.WriteLine("12. Trace Object <Object_ID>");
                Console.WriteLine();
                Console.WriteLine("13. Scenario People <Emirates_ID>");
                Console.WriteLine();
                Console.ResetColor();
            //  Retrieve input
                String Input = Console.ReadLine();
                String[] Parameters = Input.Split(" ");
            //  Handle input
                if (Parameters.Count() > 0) { Command = Parameters[0]; }
                if (Parameters.Count() > 1) { Endpoint = Parameters[1]; }
                if (Parameters.Count() > 2) { Metadata = Parameters.Skip(2).ToArray(); } }

        //  Execute command
            if (Command == "Input") Database.Input(Endpoint, Metadata);
            else if (Command == "Scenario") Map.Scenario(Endpoint, Metadata);
            else if (Command == "Beacon") Database.Beacon(Endpoint, Metadata);
            else if (Command == "Scheme") Map.Scheme(Endpoint);
            else if (Command == "Trace") Map.Trace(Endpoint, Metadata);
        //  Handle exeptions
            else throw new System.ArgumentException("Invalid command"); }
    
    } }
