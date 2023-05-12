using LineService.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace LineService.Controllers
{
    public class GroupedController : Controller
    {
        public IActionResult Index()
        {

            List<NewQuestion> questions = new List<NewQuestion>();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from grouped_question", con);
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

            var viewModel2 = new MyViewModel
            {
                Questions = questions,
                ReplyGroup = replyGroups
            };


            return View(viewModel2);
        }

        public void DeleteReplyGroup(string groupName)
        {
            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password=";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Delete the group
                using (MySqlCommand command = new MySqlCommand("DELETE FROM reply_group WHERE group_name = @GroupName", connection))
                {
                    command.Parameters.AddWithValue("@GroupName", groupName);
                    command.ExecuteNonQuery();
                }
            }


        }

        [HttpPost]
        public ActionResult ModifyGroupedQuestion(string replyGroup, string question, string SetReplyGroupButton)
        {
            if (SetReplyGroupButton == "delete")
            {
                DeleteReplyGroup(replyGroup);
                return RedirectToAction("Index");
            }

            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password=";
            string updateQuery = "UPDATE grouped_question SET reply_group = @replyGroup WHERE question = @question";
            //string transferQuery = "INSERT INTO grouped_question SELECT * FROM grouped_question WHERE question = @question";
            //string deleteQuery = "DELETE FROM grouped_question WHERE question = @question";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
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
            string query = "DELETE FROM grouped_question WHERE question = @question";

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

        [HttpPost]
        public JsonResult AddReplyGroup(string groupName, string question)
        {
            string connectionString = "server=localhost;user=root;database=deletemedb;port=3306;password=";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Insert the new group
                using (MySqlCommand command = new MySqlCommand("INSERT INTO reply_group (group_name) VALUES (@GroupName)", connection))
                {
                    command.Parameters.AddWithValue("@GroupName", groupName);
                    command.ExecuteNonQuery();
                }

            }



            return Json(new { success = true });
        }



    }
}
