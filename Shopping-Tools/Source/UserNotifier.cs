using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Tools.Source
{
    public static class UserNotifier
    {
        /// <summary>
        /// Notify all users that are registered for the product.
        /// </summary>
        /// <param name="productSimpleId">
        ///    The ID of the product
        /// </param>
        /// <param name="message">$
        ///    The message to be sent to the users
        /// </param>
        /// <returns>
        ///    The amount of users notified
        /// </returns>
        public static async Task<int> NotifyUsersForProduct(string productSimpleId, string message)
        {
            //Get all users for the product => and send mail for everyone.
            var usersToSendTo = await new Storage().GetUsersForProduct(productSimpleId);
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

            return await Task.FromResult(usersToSendTo.Count);
        }
    }
}
