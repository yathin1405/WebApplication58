using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication58.Models;

namespace WebApplication58.Models
{
    public class BookEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookID { get; set; }
        public string Email { get; set; }
        [Required]
        [Display(Name = "Guest Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string GName { get; set; }
        [Required]
        [Display(Name = "Number of attendies")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int NumAtt { get; set; }
        [Required]
        [Display(Name = "Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        [Required]
        [Display(Name = "Number of days")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int Dduration { get; set; }
        [Required]
        [Display(Name = "Number of nights")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int Nduration { get; set; }
        [Required]
        [Display(Name = "Event Type")]
        public string EventId { get; set; }
        public virtual AddEvents addevents { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Departure Date")]
        public DateTime CheckoutDate { get; set; }
        public double DayDiscount { get; set; }
        [Required]
        [Display(Name = "Day Basic Price")]
        public double DayBasicCost { get; set; }
        public double NightDiscount { get; set; }
        [Required]
        [Display(Name = "Night Basic Price")]
        public double NightBasicCost { get; set; }
        [Required]
        [Display(Name = "Total Amount")]
        public double TotalAmount { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        internal object eventsk;

        public bool dateChekeer(DateTime arrivalDate)
        {
            bool check = false;
            var outDate = (from r in db.bookEvent
                           where r.EventId == EventId
                           select r.CheckoutDate
                         ).FirstOrDefault();
            if (arrivalDate >= outDate)
            {
                check = true;
            }
            return check;
        }
        public DateTime outDate()
        {
            var outDate = (from r in db.bookEvent
                           where r.EventId == EventId
                           select r.CheckoutDate
                         ).FirstOrDefault();
            return outDate;
        }
        public int getmaxNUmAtt()
        {
            var don = (from m in db.addEventD
                       where m.EventId == EventId
                       select m.MaxNumAttNum).FirstOrDefault();
            return don;
        }


        public int getMDQA()
        {
            var don = (from a in db.addEventD
                       where a.EventId == EventId
                       select a.MDQA).FirstOrDefault();
            return don;
        }
        public string getType()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.EventType).FirstOrDefault();
            return don;

        }
        public double getDPrice()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.DEventP).FirstOrDefault();
            return don;
        }
        public double getNPrice()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.NEventP).FirstOrDefault();
            return don;
        }
        public int getMax()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.MaxNumAttNum).FirstOrDefault();
            return don;
        }
        public double getDayLdisc()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.DayLessD).FirstOrDefault();
            return don;
        }
        public double getDayGdisc()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.DayGreaterD).FirstOrDefault();
            return don;
        }


        public double getNightLdisc()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.NightLessD).FirstOrDefault();
            return don;
        }
        public double getNightGdisc()
        {
            var don = (from e in db.addEventD
                       where e.EventId == EventId
                       select e.NightGreaterD).FirstOrDefault();
            return don;
        }


        public double CalcDayBasicCost()
        {
            double Dcost = 0;
            //  double Ncost = 0;
            if (getType() == "Meeting")
            {
                Dcost = getDPrice() * Dduration * NumAtt;
                // Ncost = getNPrice() * Nduration * NumAtt;
            }
            else if (getType() == "Wedding")
            {
                Dcost = getDPrice() * Dduration * NumAtt;
                //  Ncost = getNPrice() * Nduration * NumAtt;

            }
            else if (getType() == "Party")
            {
                Dcost = getDPrice() * Dduration * NumAtt;
                //  Ncost = getNPrice() * Nduration * NumAtt;

            }
            return Dcost;
        }



        public double CalcNightBasicCost()
        {
            // double Dcost = 0;
            double Ncost = 0;
            if (getType() == "Meeting")
            {
                //Dcost = getDPrice() * Dduration * NumAtt;
                Ncost = getNPrice() * Nduration * NumAtt;
            }
            else if (getType() == "Wedding")
            {
                // Dcost = getDPrice() * Dduration * NumAtt;
                Ncost = getNPrice() * Nduration * NumAtt;

            }
            else if (getType() == "Party")
            {
                //  Dcost = getDPrice() * Dduration * NumAtt;
                Ncost = getNPrice() * Nduration * NumAtt;

            }
            return Ncost;


        }


        public double CalcDayDiscount()
        {
            double disc = 0;
            if (NumAtt >= getMDQA() && NumAtt <= getMax() / 2)
            {
                disc = CalcDayBasicCost() * getDayLdisc() / 100;
            }
            else if (NumAtt > getMax() / 2)
            {
                disc = CalcDayBasicCost() * getDayGdisc() / 100;
            }
            return disc;
        }


        public double CalcNightDiscount()
        {
            double disc = 0;
            if (NumAtt >= getMDQA() && NumAtt <= getMax() / 2)
            {
                disc = CalcNightBasicCost() * getNightLdisc() / 100;
            }
            else if (NumAtt > getMax() / 2)
            {
                disc = CalcNightBasicCost() * getNightGdisc() / 100;
            }
            return disc;
        }

        public double CalcTotalAmountDue()
        {
            return (CalcDayBasicCost() - CalcDayDiscount()) + (CalcNightBasicCost() - CalcNightDiscount());
        }

        public DateTime CheckoutD()
        {

            var endDate = date;
            // var endDate = new DateTime();
            endDate = endDate.AddDays(Nduration + Dduration);



            return endDate;
        }

    }

}


