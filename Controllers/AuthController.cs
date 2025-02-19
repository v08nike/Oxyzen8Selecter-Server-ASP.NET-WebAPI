﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Oxygen8SelectorServer.Models;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace Oxygen8SelectorServer.Controllers
{
    public class AuthController : ApiController
    {

        [HttpGet]
        [ActionName("SessionValue")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string SessionValue()
        {
            var Session = HttpContext.Current.Session;
            return Session["UAL"].ToString();
        }

        [HttpPost]
        [ActionName("Login")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // POST api/auth/login
        public object Login([FromBody]ClsLoginParams info)
        {
            DataTable dt = AuthModel.GetUserByEmail(info.email);

            var Session = HttpContext.Current.Session;

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["access"]) == 1)
                {
                    if (CalculateMD5Hash(info.password).ToUpper() == dt.Rows[0]["password"].ToString().ToUpper())
                    {
                        Session["userId"] = Convert.ToInt32(dt.Rows[0]["id"]);
                        Session["UAL"] = Convert.ToInt32(dt.Rows[0]["access_level"]);
                        Session["representativeID"] = Convert.ToInt32(dt.Rows[0]["customer_id"]);

                        long expiredTime = DateTime.Now.Millisecond + 5184000000L;

                        return new { action = "success", data = dt, accessToken = JwtManager.GenerateToken(Newtonsoft.Json.JsonConvert.SerializeObject(new { exp = expiredTime  })) };
                    } else
                    {
                        return new { action = "incorrect_password"};
                    }
                } else
                {
                    return new { action = "no_user_access" };
                }
            } else
            {
                return new { action = "no_user_exist" };
            }
        }

        [HttpPost]
        [ActionName("sendrequest")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public dynamic SendMailEmailVerification([FromBody]dynamic info)
        {
            string email = info.email.ToString();
            string subject = info.subject.ToString();
            string emailBody = info.emailBody.ToString();
            try
            {
                using (MailMessage mail = new MailMessage("innovdes2016@gmail.com", email))
                {

                    mail.Subject = subject;
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    smtp.Credentials = new NetworkCredential( "innovdes2016@gmail.com", "wemagoeblrayaxjt");

                    smtp.Send(mail);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}