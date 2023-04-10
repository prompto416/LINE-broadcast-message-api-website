using LineService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;

namespace LineService.Controllers
{
    public class ChatBotController : Controller
    {
        public IActionResult Index()
        {

            List<NewQuestion> questions = new List<NewQuestion>();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from new_question", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Extract your data 
                    //Person person = new Person();
                    NewQuestion newQuestion = new NewQuestion();
                    if (reader["reply_id"] != DBNull.Value)
                    {
                        newQuestion.reply_id = Convert.ToInt32(reader["reply_id"]);
                    }
                    else
                    {
                        newQuestion.reply_id = -1;
                    }
                    newQuestion.question = reader["question"].ToString();
                    newQuestion.reply_group = reader["reply_group"].ToString();
                    newQuestion.auto_increment = Convert.ToInt32(reader["auto_increment"]);

                    questions.Add(newQuestion);


                }
                reader.Close();




            }
            List<ReplyGroup> replyGroups = new List<ReplyGroup>();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from reply_group", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Extract your data 
                    //Person person = new Person();
                    ReplyGroup replyGroup = new ReplyGroup();
                    replyGroup.reply_group = reader["group_name"].ToString();


                    replyGroups.Add(replyGroup);


                }
                reader.Close();




            }

            var viewModel = new MyViewModel
            {
                Questions = questions,
                ReplyGroup = replyGroups
            };


            return View(viewModel);
        }





        

        [HttpPost]
        public JsonResult AddReplyGroup(string groupName)
        {
            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password=";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("INSERT INTO reply_group (group_name) VALUES (@GroupName)", connection))
                {
                    command.Parameters.AddWithValue("@GroupName", groupName);
                    command.ExecuteNonQuery();
                }
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult SetReplyGroup(string replyGroup, string question)
        {
            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password="; // Replace with your connection string
            string query = "UPDATE new_question SET reply_group = @replyGroup WHERE question = @question";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@replyGroup", replyGroup);
                    command.Parameters.AddWithValue("@question", question);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteQuestion(string question)
        {
            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password="; // Replace with your connection string
            string query = "DELETE FROM new_question WHERE question = @question";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@question", question);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine(question);

            return RedirectToAction("Index");
        }



    }
}
