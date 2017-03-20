using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TicketingSystem.DB;
using TicketingSystem.DB.ViewModel;

namespace TicketingSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string ticketIdString = string.Empty;
            Regex regex = new Regex(@"(##Ticket#[0-9])\w*##", RegexOptions.IgnoreCase);
            var matchedString = regex.Match("##ticasadasdket#12#############");
            System.Console.WriteLine(matchedString.Value);
            var ticketNo = new Regex(@"\d+").Match(matchedString.Value).Value;
            System.Console.WriteLine(int.Parse(ticketNo));
        }
    }
}
