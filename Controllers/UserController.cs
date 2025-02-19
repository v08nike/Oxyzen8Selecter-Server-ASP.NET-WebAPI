﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Oxygen8SelectorServer.Models;

namespace Oxygen8SelectorServer.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [ActionName("UpdatePassword")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string UpdatePassword([FromBody]dynamic userInfo)
        {
            return UserModel.UpdatePassword(userInfo);
        }

        [HttpPost]
        [ActionName("NewPassword")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public bool NewPassword([FromBody]dynamic userInfo)
        {
            return UserModel.NewPassword(userInfo);
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [ActionName("SaveResetPassword")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public bool IsEmailExist([FromBody]dynamic info)
        {
            DataTable dt = AuthModel.GetUserByEmail(info.email.ToString());
            if (dt.Rows.Count > 0)
            {
                UserModel.SaveSetPasswrodRequestInfo(info.email.ToString(), 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [ActionName("CompleteResetPassword")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public bool CompleteResetPassword([FromBody]dynamic info)
        {
            DataTable dt = AuthModel.GetUserByEmail(info.email.ToString());
            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["request_reset_password"]) == 1)
            {
                UserModel.SaveSetPasswrodRequestInfo(info.email.ToString(), 0);
                return true;
            }
            else
            {
                return false;
            }
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