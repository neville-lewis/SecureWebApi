using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheService.Models
{
    public class TheUserLoggedIn
    {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<string> Claims { get; set; }


        public TheUserLoggedIn()
        {
            //BUILDING A DUMMY LIST OF CLAIMS HERE. THIS SHOULD BE SET BY CODE (outside this class) AFTER DOING A DATASOURCE LOOKUP.
            Claims = new List<string>();
            Claims.Add("ReadData");
            Claims.Add("EditRecord");
            Claims.Add("DeleteRecord");
            Claims.Add("CreateUser");
            Claims.Add("Purge");

        }
    }
}