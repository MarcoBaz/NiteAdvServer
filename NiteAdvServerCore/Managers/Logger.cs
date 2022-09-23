using NiteAdvServerCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NiteAdvServerCore.Managers;

public static class Logger
{

    public static void Write(string dockerID,string logMessage, string logType) //LoggerType logType = LoggerType.info
    {

        try
        {


            LoggerType logTypeToSend = LoggerType.info;
            if (logType.ToLower() =="error")
            {
                //sendMail
                //if (MailingListLogger != null && MailingListLogger.Any())
                //{
                //    MailingListLogger.ForEach(x =>
                //    {
                //        //string body = "Dal processo automatico delle scansioni delle bolle sono state trovate mancate associazioni dei seguenti  file:\n  " + logMessage;
                //        MailHelper.SendMail("marco.bazzoli@outlook.it", x, "Messaggio Servizio Cora", logMessage); //marco@assistenzacora.it
                //    });
                //}
                logTypeToSend =  LoggerType.error;
            }
            else if (logType.ToLower() == "warning")
                logTypeToSend = LoggerType.warning;

            Log(dockerID, logMessage, logTypeToSend);
        }
        catch (Exception ex)
        {

        }
    }


    private static void Log(string dockerID,string logMessage, LoggerType logType)
    {
        try
        {
            var log = new Log();
            log.Id = 0;
            log.Data = DateTime.UtcNow;
            log.DockerID = dockerID;
            log.Message = logMessage;
            log.LogType = logType.ToString();
            log.LastSyncDate = DateTime.UtcNow;
          
            SqlServerManager.Save<Log>(log);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static List<String> MailingListLogger
    {
        get
        {
            List<String> mails = new List<string>();
            mails.Add("marco.bazzoli@outlook.it");
            return mails;
        }
    }

}
public enum LoggerType
{
    error,
    info,
    warning
}
