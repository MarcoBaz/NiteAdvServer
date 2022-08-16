using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NiteAdvServerCore.Managers;

public static class SMSManager
{
    private static string TwilioSender = "+12058946514";
    const string accountSID = "AC39a9e8ffd79a6cff24230dbca96dc09e"; 
    const string authToken = "3fadeb136dda176837be4f0026efc42a";
  
    //ritona il codice di verifica
    
   /* public static bool SendSMSRegisterUser(User user)
    {
        // Use your account SID and authentication token instead
        // of the placeholders shown here.


        // Initialize the TwilioClient.
        TwilioClient.Init(accountSID, authToken);

        try
        {
            string phoneNumber = user.PhoneNumber.Trim(' ', '-', '/');
            //gestire +39 per l'italia
            if (user.IdNation == 118 && phoneNumber.Substring(0,3) != "+39")
                phoneNumber = "+39" + phoneNumber;


            string link = String.Format("https://www.mobyalps.com/pages/confirm/{0}", user.Id + 10274);
            string messageBody = "MobyAlps registrazione: Per completare la registrazione è necessario cliccare sul seguente link: " + link;
            // Send an SMS message.
            var message = MessageResource.Create(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(TwilioSender),
                body: messageBody);

            return true;
        }
        catch (TwilioException ex)
        {
            // An exception occurred making the REST call
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static bool SendFreeSMSToUser(User user, string resultText)
    {
        // Use your account SID and authentication token instead
        // of the placeholders shown here.


        // Initialize the TwilioClient.
        TwilioClient.Init(accountSID, authToken);

        try
        {
            if (String.IsNullOrWhiteSpace(user.PhoneNumber))
                return false;

            string phoneNumber = user.PhoneNumber.Trim(' ','-', '/');
            //gestire +39 per l'italia
            if (user.IdNation == 118 && phoneNumber.Substring(0, 3) != "+39")
                phoneNumber = "+39" + phoneNumber;
        

            // Send an SMS message.
            var message = MessageResource.Create(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(TwilioSender),
                body: resultText);

            return true;
        }
        catch (TwilioException ex)
        {
            // An exception occurred making the REST call
            Console.WriteLine(ex.Message);
            return false;
        }
    }*/
    
}
