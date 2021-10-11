using WebApplication58.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class logic
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int countBookings(string email)
        {
            //int count = 0;

            List<Reservation> app = db.reservations.ToList().FindAll(x => x.applicationUser.Email.Equals(email));
            return app.Count;
        }

        public bool isFree(string email)
        {
            bool free = false;

            if (countBookings(email) >= 4)
            {
                free = true;
            }

            return free;
        }
    }
}