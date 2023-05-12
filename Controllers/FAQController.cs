using LineService.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace LineService.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {

            List<Reply> replies = new List<Reply>();
            //Connect to mysql test 
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from replies", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Extract your data 
                    //Person person = new Person();
                    Reply reply = new Reply();


                    //person.Id = Convert.ToInt32(reader["id"]);
                    //person.FirstName = reader["first_name"].ToString();
                    //person.LastName = reader["last_name"].ToString();
                    //person.Age = Convert.ToInt32(reader["age"]);
                    //persons.Add(person);

                    reply.ReplyId = Convert.ToInt32(reader["reply_id"]);
                    reply.ReplyGroup = reader["reply_group"].ToString();
                    reply.Question = reader["question"].ToString();
                    reply.Answer = reader["answer"].ToString();
                    reply.Keyword = reader["Keyword"].ToString();
                    reply.Keyword1 = reader["Keyword_1"].ToString();
                    replies.Add(reply);

                }
                reader.Close();

            }




            //end of mysql test
            return View(replies);


        }

        [HttpPost]
        public IActionResult UpdateQuestion(int replyId, string question)
        {
            // Connect to the MySQL database
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE replies SET question = @question WHERE reply_id = @replyId", con);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@replyId", replyId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }


        }



        [HttpPost]
        public IActionResult UpdateAnswer(int replyId, string answer) // Changed 'question' to 'answer'
        {
            // Connect to the MySQL database
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE replies SET answer = @answer WHERE reply_id = @replyId", con); // Changed '@question' to '@answer'
                cmd.Parameters.AddWithValue("@answer", answer); // Changed '@question' to '@answer'
                cmd.Parameters.AddWithValue("@replyId", replyId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateKeyword(int replyId, string keyword) // Changed 'question' to 'keyword'
        {
            // Connect to the MySQL database
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE replies SET Keyword = @keyword WHERE reply_id = @replyId", con); // Changed '@question' to '@keyword'
                cmd.Parameters.AddWithValue("@keyword", keyword); // Changed '@question' to '@keyword'
                cmd.Parameters.AddWithValue("@replyId", replyId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateKeyword1(int replyId, string keyword1) // Changed 'question' to 'keyword1'
        {
            // Connect to the MySQL database
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE replies SET Keyword_1 = @keyword1 WHERE reply_id = @replyId", con); // Changed '@question' to '@keyword1'
                cmd.Parameters.AddWithValue("@keyword1", keyword1); // Changed '@question' to '@keyword1'
                cmd.Parameters.AddWithValue("@replyId", replyId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }


        [HttpPost]
        public IActionResult AddData(string question, string answer, string keyword)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
                MySqlCommand maxIdCmd = new MySqlCommand("SELECT MAX(reply_id) FROM replies", con);
                int maxId = Convert.ToInt32(maxIdCmd.ExecuteScalar());
                int newId = maxId + 1;

                MySqlCommand insertCmd = new MySqlCommand("INSERT INTO replies (reply_id, question, answer, Keyword) VALUES (@replyId, @question, @answer, @keyword)", con);
                insertCmd.Parameters.AddWithValue("@replyId", newId);
                insertCmd.Parameters.AddWithValue("@question", question);
                insertCmd.Parameters.AddWithValue("@answer", answer);
                insertCmd.Parameters.AddWithValue("@keyword", keyword);

                int result = insertCmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }





        }

        [HttpPost]
        public IActionResult DeleteData(int replyId)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=deletemedb;port=3306;password="))
            {
                con.Open();
              
                MySqlCommand cmd = new MySqlCommand("DELETE FROM replies WHERE reply_id = @replyId", con);
                cmd.Parameters.AddWithValue("@replyId", replyId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

    }
}
