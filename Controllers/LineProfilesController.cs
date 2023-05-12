//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using LineService.Data;
//using LineService.Models;
//using MySql.Data.MySqlClient;

//namespace LineService.Controllers
//{
//    public class LineProfilesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public LineProfilesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: LineProfiles
//        public async Task<IActionResult> Index()
//        {


//            //List<Person> persons = new List<Person>();
//            List<Customer> customers = new List<Customer>();
//            List<Schedule> schedules = new List<Schedule>();
//            //Connect to mysql test 
//            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password=;convert zero datetime=True"))
//            {
//                con.Open();
//                MySqlCommand cmd = new MySqlCommand("select * from person", con);
//                MySqlDataReader reader = cmd.ExecuteReader();

//                while (reader.Read())
//                {
//                    //Extract your data 
//                    //Person person = new Person();
//                    Customer customer = new Customer();


//                    //person.Id = Convert.ToInt32(reader["id"]);
//                    //person.FirstName = reader["first_name"].ToString();
//                    //person.LastName = reader["last_name"].ToString();
//                    //person.Age = Convert.ToInt32(reader["age"]);
//                    //persons.Add(person);

//                    customer.userId = reader["user_Id"].ToString();
//                    customer.displayName = reader["display_Name"].ToString();
//                    customer.pictureUrl = reader["picture_Url"].ToString();
//                    customer.language = reader["language"].ToString();
//                    customer.editName = reader["edit_Name"].ToString();
//                    customers.Add(customer);

//                }
//                reader.Close();
//                cmd = new MySqlCommand("select * from schedule", con);
//                reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    //Extract your data 
//                    //Person person = new Person
//                    Schedule schedule = new Schedule();


//                    //person.Id = Convert.ToInt32(reader["id"]);
//                    //person.FirstName = reader["first_name"].ToString();
//                    //person.LastName = reader["last_name"].ToString();
//                    //person.Age = Convert.ToInt32(reader["age"]);
//                    //persons.Add(person);
//                    schedule.startDate = reader["start_date"].ToString();
//                    schedule.endDate = reader["end_date"].ToString();
//                    schedule.time = reader["time"].ToString();
//                    schedule.repeat_interval = reader["repeat_interval"].ToString();
//                    schedule.message = reader["message"].ToString();
//                    schedule.target = reader["target"].ToString();
//                    schedule.increment = Convert.ToInt32(reader["increment"]);
//                    schedules.Add(schedule);


//                }
//                reader.Close();
//            }




//            //end of mysql test
//            return View(schedules);
//            //return View(await _context.LineProfile.ToListAsync());
//        }

//        public IActionResult deleteScheduleSqlQuery(string scheduleId)
//        {
//            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
//            {
//                con.Open();


//                //string targetEditName = "Katang";


//                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "')", con);
//                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE (`display_Name` LIKE '" + wantedDisplayName + "'OR `display_Name` LIKE 'Poop')", con);
//                //MySqlCommand cmd = new MySqlCommand("SELECT * FROM `person` WHERE `display_Name` LIKE '%"+wantedDisplayName+"%'", con);
//                //string editNameCommand = "UPDATE `person` SET `edit_Name` = '{0}' WHERE `person`.`display_Name` = '{1}'";
//                string editNameCommand = "DELETE FROM `schedule` WHERE `schedule`.`increment` = {0};";
//                MySqlCommand cmd = new MySqlCommand(string.Format(editNameCommand, scheduleId), con);
//                Console.WriteLine(string.Format(editNameCommand, scheduleId));
//                MySqlDataReader reader = cmd.ExecuteReader();
                

//                return RedirectToAction("");
//            }
//        }

//        // GET: LineProfiles/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var lineProfile = await _context.LineProfile
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (lineProfile == null)
//            {
//                return NotFound();
//            }

//            return View(lineProfile);
//        }

//        // GET: LineProfiles/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: LineProfiles/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,userId,displayName,pictureUrl,FirstName,SurName,FullName")] LineProfile lineProfile)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(lineProfile);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(lineProfile);
//        }

//        // GET: LineProfiles/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var lineProfile = await _context.LineProfile.FindAsync(id);
//            if (lineProfile == null)
//            {
//                return NotFound();
//            }
//            return View(lineProfile);
//        }

//        // POST: LineProfiles/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,displayName,pictureUrl,FirstName,SurName,FullName")] LineProfile lineProfile)
//        {
//            if (id != lineProfile.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(lineProfile);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!LineProfileExists(lineProfile.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(lineProfile);
//        }

//        // GET: LineProfiles/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var lineProfile = await _context.LineProfile
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (lineProfile == null)
//            {
//                return NotFound();
//            }

//            return View(lineProfile);
//        }

//        // POST: LineProfiles/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var lineProfile = await _context.LineProfile.FindAsync(id);
//            _context.LineProfile.Remove(lineProfile);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool LineProfileExists(int id)
//        {
//            return _context.LineProfile.Any(e => e.Id == id);
//        }
//    }
//}
