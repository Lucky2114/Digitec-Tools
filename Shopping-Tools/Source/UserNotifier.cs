using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Tools.Source
{
    public static class UserNotifier
    {
        public static async Task NotifyUsersForProduct(string productSimpleId, string message)
        {
            //Get all users for the product => and send mail for everyone.
            var usersToSendTo = await Storage.GetInstance(null, null).GetUsersForProduct(productSimpleId);
            foreach (var user in usersToSendTo)
            {
                var email = user["Email"].ToString();
                if (email.Contains("@"))
                {
                    await EmailSender.Send(email, message, "The Price Of A Product Has Changed!");
                } else
                {
                    Console.WriteLine("Couldn't send Email. Something wrong with the data.");
                }
            }
            
        }
    }
}
