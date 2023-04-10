using LineService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Routing;
using System.Windows;

using System.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Org.BouncyCastle.Utilities.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LineService.Controllers

{
    [Authorize]
    public class HomeController : Controller
    {

        [Obsolete]
        IHostingEnvironment _env;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IHostingEnvironment environment)
        {
            _logger = logger;
            _env = environment;
        }

        public IActionResult Index()
        {

            //List<Person> persons = new List<Person>();
            List<Customer> customers = new List<Customer>();
            List<Schedule> schedules = new List<Schedule>();
            //Connect to mysql test 
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from person", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        //Extract your data 
                        //Person person = new Person();
                        Customer customer = new Customer();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);

                    customer.userId = reader["user_Id"].ToString();
                    customer.displayName = reader["display_Name"].ToString();
                    customer.pictureUrl = reader["picture_Url"].ToString();
                    customer.language = reader["language"].ToString();
                    customer.editName = reader["edit_Name"].ToString();
                    customer.tag = reader["tag"].ToString();
                    customers.Add(customer);

                    }
                    reader.Close();
                    cmd = new MySqlCommand("select * from schedule", con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //Extract your data 
                        //Person person = new Person
                        Schedule schedule = new Schedule();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);
                    schedule.startDate = reader["start_date"].ToString();
                    schedule.endDate = reader["end_date"].ToString();
                    schedule.time = reader["time"].ToString();
                    schedule.repeat_interval = reader["repeat_interval"].ToString();
                    schedule.message = reader["message"].ToString();
                    schedule.target = reader["target"].ToString();
                    schedule.increment = Convert.ToInt32(reader["increment"]);
                    schedules.Add(schedule);
             

                    }
                    reader.Close();
            }




                //end of mysql test
                return View(customers);
                
        }

       

        public List<string> executeSqlQuery(string wantedDisplayName)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();

                List<string> idList = new List<string>();
                string sqlCommand = parseSqlQuery(wantedDisplayName);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "'OR `display_Name` LIKE 'Poop')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE `display_Name` LIKE '%"+wantedDisplayName+"%'", con);
                MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                string targetUserId = "0";
                while (reader.Read())
                {
                    
                    //Extract your data 
                    //Person person = new Person();
                    Customer targetCustomer = new Customer();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);

                    targetCustomer.userId = reader["user_Id"].ToString();
                    targetCustomer.displayName = reader["display_Name"].ToString();
                    targetCustomer.pictureUrl = reader["picture_Url"].ToString();
                    targetCustomer.language = reader["language"].ToString();
                    targetCustomer.editName = reader["edit_Name"].ToString();
                    //Console.WriteLine(targetCustomer.userId);
                    targetUserId = targetCustomer.userId;
                    idList.Add(targetCustomer.userId);

                }
                reader.Close();
                //return targetUserId;
                return idList;
            }
            
        }

        public string parseSqlQuery(string wantedDisplayName)
        {
            

            if (wantedDisplayName.Length <= 0)
            {
                return "";
            }
            string[] words = wantedDisplayName.Split(',');

            List<string> nameList = new List<string>();

            string finalres = "";
            foreach (var word in words)
            {
                //string finalres = $"{word}";
                //finalres = finalres + ($"{word}");
                nameList.Add(word);
                //console.write($"{word}");
            }


            //string final = "SELECT * FROM `person` WHERE (`display_Name` LIKE '" + nameList[0] + "'OR `display_Name` LIKE 'Poop'";
            //string fake = "SELECT * FROM `person` WHERE (`display_Name` LIKE '" + nameList[0] + "'";
            //string adder = "OR `display_Name` LIKE '{0}'";
            //string final = "SELECT * FROM `person` WHERE (`edit_Name` LIKE '" + nameList[0] + "'OR `edit_Name` LIKE 'Poop'";
            string fake = "SELECT * FROM `person` WHERE (`edit_Name` LIKE '" + nameList[0] + "'";
            string adder = "OR `edit_Name` LIKE '{0}'";
            //string match = final;
            nameList.RemoveAt(0);


            for (int i = 0; i < nameList.Count; i++)
            {
                fake += string.Format(adder, nameList[i]);
            }

            fake = fake + ")";

            return fake;
        }
        public IActionResult BoardCast()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var imagePath = @"\Images\";
                var uploadPath = _env.WebRootPath + imagePath;

                // Create Directory
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //Create Uniq file name
                var uniqFileName = Guid.NewGuid().ToString();
                uniqFileName = "uploadedImage";
                var filename = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".")[1].ToLower());

                // Code Added
                string[] files = Directory.GetFiles(uploadPath, "uploadedImage*");
                filename = "uploadedImage" + (files.Length + 1).ToString() + "." + file.FileName.Split(".")[1].ToLower();

                string fullPath = uploadPath + filename;

                imagePath = imagePath + @"\";
                var filePath = @".." + Path.Combine(imagePath, filename);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                ViewData["FileLocation"] = filePath;
            }
            ExecuteClient();

            return View("../Home/BoardCast");

        }



        [HttpPost]
        public IActionResult getInputValue(string userInput, string message,string startDate,string endDate,string time,bool monday,bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            string repeatInterval = "";
            List<bool> tickedDays = new List<bool>()
            {
                monday,
                tuesday,
                wednesday,
                thursday,
                friday,
                saturday,
                sunday,
            };
            List<string> dayOfWeek = new List<string>()
            {
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "saturday",
                "sunday",
            };
            
            for (int i = 0; i < tickedDays.Count; i++)
            {
                if (tickedDays[i] == true)
                {
                    repeatInterval += (dayOfWeek[i] + ",");
                }
            }

           


            List<string> userinId = executeSqlQuery(userInput);

            if ((startDate != null) || (time != null))
            {
                string tempQuery = "INSERT INTO `schedule` (`start_date`, `end_date`, `time`, `repeat_interval`, `message`,`target`) VALUES('{0}', '{1}', '{2}', '{3}', '{4}',{5});";
                tempQuery = "INSERT INTO `schedule` (`start_date`, `end_date`, `time`, `repeat_interval`, `message`, `target`, `increment`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', NULL);";
                tempQuery = string.Format(tempQuery,startDate,endDate,time,repeatInterval,message,userInput);
                ExecuteQuery(tempQuery);

                return RedirectToAction("BoardCast");
            }


            foreach (var id in userinId)
            {
                Console.WriteLine(id);
                sendLineMessage(message, id);
            }

          
      
            return RedirectToAction("BoardCast");
        }
        
        public IActionResult editNameSqlQuery(string nickname,string targetEditName)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();

                
                //string targetEditName = "Katang";
                

                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "'OR `display_Name` LIKE 'Poop')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE `display_Name` LIKE '%"+wantedDisplayName+"%'", con);
                string editNameCommand = "UPDATE `person` SET `edit_Name` = '{0}' WHERE `person`.`display_Name` = '{1}'";
                MySqlCommand cmd = new MySqlCommand(string.Format(editNameCommand,nickname,targetEditName), con);

                MySqlDataReader reader = cmd.ExecuteReader();


                return RedirectToAction("Index");
            }
        }

        public IActionResult editTagSqlQuery(string tagInput, string targetEditName)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();


                //string targetEditName = "Katang";


                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "'OR `display_Name` LIKE 'Poop')", con);
                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE `display_Name` LIKE '%"+wantedDisplayName+"%'", con);
                string editNameCommand = "UPDATE `person` SET `tag` = '{0}' WHERE `person`.`display_Name` = '{1}'";
                MySqlCommand cmd = new MySqlCommand(string.Format(editNameCommand, tagInput, targetEditName), con);

                MySqlDataReader reader = cmd.ExecuteReader();


                return RedirectToAction("Index");
            }
        }

        public IActionResult addTag(object sender, EventArgs e,string tagInput,string inputType)
        {
            //List<Person> persons = new List<Person>();
            List<Customer> customers = new List<Customer>();
            List<Schedule> schedules = new List<Schedule>();
            //Connect to mysql test 
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from person", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Extract your data 
                    //Person person = new Person();
                    Customer customer = new Customer();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);

                    customer.userId = reader["user_Id"].ToString();
                    customer.displayName = reader["display_Name"].ToString();
                    customer.pictureUrl = reader["picture_Url"].ToString();
                    customer.language = reader["language"].ToString();
                    customer.editName = reader["edit_Name"].ToString();
                    customer.tag = reader["tag"].ToString();
                    customers.Add(customer);

                }
                reader.Close();
                cmd = new MySqlCommand("select * from schedule", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //Extract your data 
                    //Person person = new Person
                    Schedule schedule = new Schedule();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);
                    schedule.startDate = reader["start_date"].ToString();
                    schedule.endDate = reader["end_date"].ToString();
                    schedule.time = reader["time"].ToString();
                    schedule.repeat_interval = reader["repeat_interval"].ToString();
                    schedule.message = reader["message"].ToString();
                    schedule.target = reader["target"].ToString();
                    schedule.increment = Convert.ToInt32(reader["increment"]);
                    schedules.Add(schedule);


                }
                reader.Close();
            }
            
            foreach (var i in customers)
            {
                
                string temp = Request.Form[i.userId];
                if (temp != null)
                {
                    Console.WriteLine(i.userId + "success");
                    using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
                    {
                        con.Open();


                        //string targetEditName = "Katang";


                        //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "')", con);
                        //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "'OR `display_Name` LIKE 'Poop')", con);
                        //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE `display_Name` LIKE '%"+wantedDisplayName+"%'", con);
                        
                        string editNameCommand = "UPDATE `person` SET `tag` = '{0}' WHERE `person`.`display_Name` = '{1}'";
                        if (inputType == "Add Tag")
                        {
                            editNameCommand = "UPDATE `person` SET `tag` = CONCAT(`tag`, ',{0}') WHERE `person`.`display_Name` = '{1}'";
                        }
                        
                        MySqlCommand cmd = new MySqlCommand(string.Format(editNameCommand, tagInput, i.displayName), con);
                        

                        MySqlDataReader reader = cmd.ExecuteReader();


                       
                    }

                }
                else
                {
                    Console.WriteLine(i.displayName+"failed");
                }

            }
            
           
           

            // Do whatever processing you need to do here
            return RedirectToAction("Index");
        }



        //Client method for socket connection 
        public void sendLineMessage(string message,string userId)
        {
            string doubleQuote = @"""";
            
            userId = string.Format("{0}"+userId+"{1}",'"','"');
            //string message = "Enter your message";
            string formattedMessage = string.Format("{0}{1}{2}", doubleQuote, message ,doubleQuote);
            var client = new RestClient("https://api.line.me/v2/bot/message/push");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer 8jcva04mhIBVnMFqCfaQe5NNIkpAA5xm/uq2IOPRpGzHcSCCYQV+FbAhqI06FxzPorWCpL6tXSFcSJBHp0OESgaoomYPFofuQ71uO5Q0NzmBiMsVkBnIgeArfezFw+dgJRgAvzHPN054amxDLexEcAdB04t89/1O/w1cDnyilFU=");
            request.AddHeader("Content-Type", "application/json");


            //            var body = @"{
            //" + "\n" +
            //            @"    ""to"": ""Uc993a7060da875cc7c48196773ff4175"",
            //" + "\n" +
            //            @"    ""messages"":[
            //" + "\n" +
            //            @"        {
            //" + "\n" +
            //            @"            ""type"":""text"",
            //" + "\n" +
            //            @"            ""text"":""{message}""
            //" + "\n" +
            //            @"        }
            //" + "\n" +
            //            @"    ]
            //" + "\n" +
            //            @"}";
            var body = @"
            {

                ""to"": " + userId + @",

                ""messages"":[

                    {

                        ""type"":""text"",

                        ""text"":" + formattedMessage + @" 

                    }

                ]

            }
            ";







            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            
        }
        public static void ExecuteClientBackup()
        {

            try
            {

                // Establish the remote endpoint
                // for the socket. This example
                // uses port 11111 on the local
                // computer.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                // Creation TCP/IP Socket using
                // Socket Class Constructor
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    // Connect Socket to the remote
                    // endpoint using method Connect()
                    sender.Connect(localEndPoint);

                    // We print EndPoint information
                    // that we are connected
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of message that
                    // we will send to Server
                    byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
                    int byteSent = sender.Send(messageSent);

                    // Data buffer
                    byte[] messageReceived = new byte[1024];

                    // We receive the message using
                    // the method Receive(). This
                    // method returns number of bytes
                    // received, that we'll use to
                    // convert them to string
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    // Close Socket using
                    // the method Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        [HttpPost]
        public void ExecuteQuery([FromBody] string query)
        {
            using (var connection = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                connection.Open();

                using (var command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IActionResult ExecuteClient()
        {

            try
            {

                // Establish the remote endpoint
                // for the socket. This example
                // uses port 11111 on the local
                // computer.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                ipHost = Dns.GetHostEntry("0.tcp.ap.ngrok.io");
                IPAddress ipAddr = ipHost.AddressList[0];
                System.Console.WriteLine(ipAddr);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 18608);

                // Creation TCP/IP Socket using
                // Socket Class Constructor
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);


                try
                {

                    // Connect Socket to the remote
                    // endpoint using method Connect()
                    sender.Connect(localEndPoint);

                    // We print EndPoint information
                    // that we are connected
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());

                    // Creation of message that
                    // we will send to Server
                    byte[] messageSent = Encoding.ASCII.GetBytes("python -u mouse_bot.py");
                    int byteSent = sender.Send(messageSent);

                    // Data buffer
                    byte[] messageReceived = new byte[1024];

                    // We receive the message using
                    // the method Receive(). This
                    // method returns number of bytes
                    // received, that we'll use to
                    // convert them to string
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    // Close Socket using
                    // the method Close()
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
            return RedirectToAction("Index");
        }

        

    }
}
