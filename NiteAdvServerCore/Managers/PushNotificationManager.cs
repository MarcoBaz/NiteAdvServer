using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account.Usage.Record;

namespace NiteAdvServerCore.Managers;

public  class PushNotificationManager
{
    private static PushNotificationManager instance = null;
   
    private FirebaseApp NotificationApp;
    private static Random _random;

    private PushNotificationManager()
    {
        //inizializzazione della push notification
        
            NotificationApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new FirebaseCredentials())),
            });
            var defaultAuth = FirebaseAuth.GetAuth(NotificationApp);
        
        //await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(defaultAuth., claims);
    }

    public static PushNotificationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PushNotificationManager();
                _random = new Random();
            }
            return instance;
        }
    }
   /* public async Task<string> SendPushVerificationCode(User user, string existingCode = "")
    {
        try
        {
            string digitNumbers = String.Empty;
            if (String.IsNullOrWhiteSpace(existingCode))
                digitNumbers = GenerateRandomNo();
            else
                digitNumbers = existingCode;
           
            string messageBody = "Il codice di verifica per la tua carta è: " + digitNumbers;
            // Send an SMS message.
            SendSingleMessage(user.Id, "Codice Verifica", messageBody, false);

            return digitNumbers;
        }
        catch (Exception ex)
        {
            // An exception occurred making the REST call
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }
    private static string GenerateRandomNo()
    {
        return _random.Next(0, 9999).ToString("D4");
    }
    public  async Task<bool> SendSingleMessage(int idUser, string title, string body, bool fromChat,int idTravel=0,int idServiceBook=0)
    {
        try
        {
            var devices = SqlServerManager.GetCollection<Device>("IdUser=" + idUser);
            if (devices != null)
            {

                var tokens = devices.Select(x => x.TokenPush).ToList();
                var message = BuildMessage(title, body, tokens, fromChat, idTravel, idServiceBook);
                //var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);

                BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(false);
                if (response.FailureCount > 0)
                {
                    int counter = 0;
                    foreach (var resp in response.Responses)
                    {
                        if (!resp.IsSuccess && resp.Exception != null )
                        {
                            Logger.Write($"Errore nell'invio della notifica push dell'utente {idUser} con messaggio {resp.Exception.Message}");
                            var device = SqlServerManager.GetEntity<Device>("TokenPush='" + tokens[counter] + "'");

                            if (device != null)
                                SqlServerManager.Delete<Device>(device);
                        }
                        counter += 1;
                    }
                }
#if DEBUG
                Logger.Write($"Notification result: totali {response.SuccessCount + response.FailureCount} - corretti  {response.SuccessCount} - errati  {response.FailureCount} ");
#endif
            }
            // Response is a message ID string.
           // Console.WriteLine("Successfully sent message: " + response);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<bool> SendSingleMessageV2(int idUser, string title, string body, bool fromChat, int idTravel = 0, int idServiceBook = 0)
    {
        try
        {
            var devices = SqlServerManager.GetCollection<Device>("IdUser=" + idUser);
            if (devices != null)
            {

                var tokens = devices.Select(x => x.TokenPush).ToList();
                //var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
                tokens.ForEach(async x =>
                {
               var response = await FirebaseMessaging.DefaultInstance.SendAsync(BuildSingleMessage(title, body, x, fromChat, idTravel, idServiceBook));
                    if (String.IsNullOrWhiteSpace(response))
                    {
                       // error
                        
                    }
                });
              
#if DEBUG
                
#endif
            }
            // Response is a message ID string.
            // Console.WriteLine("Successfully sent message: " + response);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private Message BuildSingleMessage(string title, string body, string token, bool fromChat, int idTravel, int idServiceBook)
    {
        //var badges = SqlServerManager.GetCollection<Travel>("IdRenter=0");
        //int nbadges = badges != null && badges.Any() ? badges.Count() : 1;
        var Data = new Dictionary<string, string>();
        Data.Add("IsChat", fromChat.ToString());
        if (fromChat)
        {
            Data.Add("IdTravel", idTravel.ToString());
            Data.Add("IdServiceBook", idServiceBook.ToString());
        }
        var message = new Message()
        {
            Token = token,
            Notification = new Notification()
            {
                Title = title,
                Body = body,

            },
            Android = new AndroidConfig()
            {
                TimeToLive = TimeSpan.FromHours(1),
                Notification = new AndroidNotification()
                {
                    Sound = "default"
                }
                //Notification = new AndroidNotification()
                //{
                //    Icon = "stock_ticker_update",
                //    Color = "#f45342",
                //},
            },
            Apns = new ApnsConfig()
            {
                Aps = new Aps()
                {
                    Badge = 0,
                    ContentAvailable = true,
                    Sound = "default"
                },
            }
        };
        message.Data = Data;
        return message;
    }
    public  async Task<bool> SendAllVectorsMessage(string title, string body)
    {
        try
        {
            //recupero tutti i token dei vettori
           
            var devices = SqlServerManager.FreeQuery<Device>("select D.* from  [User] as U  inner join Renter as R  on R.IdUser = U.Id inner join Device As D On D.IdUser = U.Id");
            if (devices != null && devices.Any())
            {

                var tokens = devices.Select(x => x.TokenPush).ToList();
                var message = BuildMessage(title, body, tokens,false,0,0);
                BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(false);
                if (response.FailureCount > 0)
                {
                    int counter = 0;
                    foreach (var resp in response.Responses)
                    {
                        if (!resp.IsSuccess && resp.Exception != null && resp.Exception.ErrorCode == ErrorCode.NotFound)
                        {
                          
                            var device = SqlServerManager.GetEntity<Device>("TokenPush='" + tokens[counter] + "'");
                            if (device != null)
                                SqlServerManager.Delete<Device>(device);
                        }
                        counter += 1;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> SendNearVectorsMessage(string title, string body,string boundaryBox)
    {
        try
        {
            //recupero tutti i token dei vettori

            var vectors = SqlServerManager.FreeQuery<Renter>("select V.* from  [User] as U  inner join Renter as V  on V.IdUser = U.Id");
            if (vectors != null && vectors.Any() && !String.IsNullOrWhiteSpace(boundaryBox))
            {
                //recupero i dati del box
                List<string> tokens = new List<string>();
                List<double> boundary = new List<double>();
                var boundaryboxList = boundaryBox.Split(';').ToList();
                boundaryboxList.ForEach(x =>
                {
                    boundary.Add(Convert.ToDouble(x.Replace(',', '.')));
                });
                vectors.ForEach(x =>
                {
                if (x.Ncc)
                {
                    var offices = SqlServerManager.GetCollection<RenterSaleOffice>("IdRenter=" + x.Id);
                    if (offices != null && offices.Any())
                        offices.ForEach(y =>{ 
                             if (CompareCoordinates(y.Latitude, y.Longitude, boundary))
                            {
                                var devices = SqlServerManager.GetCollection<Device>("IdUser=" + x.IdUser);
                                devices.ForEach(z =>
                                {
                                    var existingToken = tokens.Where(tk=> tk == z.TokenPush).FirstOrDefault();
                                    if (String.IsNullOrWhiteSpace(existingToken))
                                        tokens.Add(z.TokenPush);
                                });
                                return;
                            }
                        });

                       
                    }
                    
                });
                // becco tutti i car position
                var carpositions = SqlServerManager.GetCollection<CarPosition>("UntilDate >  CONVERT(DATETIME, '" + DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss.fff tt") + "', 103)");
                if (carpositions != null && carpositions.Any())
                {
                    carpositions.ForEach(x =>
                    {
                        if (CompareCoordinates(x.Latitude, x.Longitude, boundary))
                        {
                            if (!string.IsNullOrWhiteSpace(x.TokenPush))
                            {
                                var existingToken = tokens.Where(tk => tk == x.TokenPush).FirstOrDefault();
                                if (String.IsNullOrWhiteSpace(existingToken))
                                    tokens.Add(x.TokenPush);
                            }
                                    
                        }
                    });
                }
                if (tokens.Any())
                {
                    var message = BuildMessage(title, body, tokens,false,0,0);
                    BatchResponse response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);//.ConfigureAwait(false);
                    if (response.FailureCount > 0)
                    {
                        int counter = 0;
                        foreach (var resp in response.Responses)
                        {
                            if (!resp.IsSuccess && resp.Exception != null && resp.Exception.ErrorCode == ErrorCode.NotFound)
                            {

                                var device = SqlServerManager.GetEntity<Device>("TokenPush='" + tokens[counter] + "'");
                                if (device != null)
                                    SqlServerManager.Delete<Device>(device);
                            }
                            counter += 1;
                        }
                    }
                }
                else
                    await SendAllVectorsMessage(title, body);

             
            }
               

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private bool CompareCoordinates(string latitude,string longitude,List<double> bounds)
    {
        try
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(latitude) || string.IsNullOrWhiteSpace(longitude))
                return result;

            double vectLatitude = Convert.ToDouble(latitude.Replace(',','.'));
            double vectLongitude = Convert.ToDouble(longitude.Replace(',', '.'));
            if (vectLongitude >= bounds[0] && vectLongitude <= bounds[1] && vectLatitude <= bounds[2] && vectLatitude >= bounds[3])
            {
                result = true;
            }
            return result;
        }
        catch
        {
            return false;
        }
       
    }
   


    private MulticastMessage BuildMessage(string title, string body,List<string> tokens, bool fromChat,int idTravel, int idServiceBook)
    {
        //var badges = SqlServerManager.GetCollection<Travel>("IdRenter=0");
        //int nbadges = badges != null && badges.Any() ? badges.Count() : 1;
        var Data = new Dictionary<string, string>();
        Data.Add("IsChat", fromChat.ToString());
        if (fromChat)
        {
            Data.Add("IdTravel", idTravel.ToString());
            Data.Add("IdServiceBook", idServiceBook.ToString());
        }
        var message = new MulticastMessage()
        {
            Tokens = tokens,
            Notification = new Notification()
            {
                Title = title,
                Body = body,
                
            },
            Android = new AndroidConfig()
            {
                TimeToLive = TimeSpan.FromHours(1),
                Notification = new AndroidNotification()
                {
                    Sound = "default"
                }
                //Notification = new AndroidNotification()
                //{
                //    Icon = "stock_ticker_update",
                //    Color = "#f45342",
                //},
            },
            Apns = new ApnsConfig()
            {
                Aps = new Aps()
                {
                    Badge = 0,
                    ContentAvailable = true,
                    Sound = "default"
                },
            }
        };
        message.Data = Data;
        return message;
    }*/
       
}
public class FirebaseCredentials
{
    public  string type { get { return "service_account"; }}
    public  string project_id { get { return "mobyalps-c1277"; } }
    public  string private_key_id { get { return "69817a7b711210686076aa3cf3d7a54b2411ca25"; } }
    public string private_key { get { return "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCh4xDLSL/kJSzA\npLMueyRuCesYOvY+YJOf+8Hcvbdm8twYqIOg/tAm6/NUHym2QKdGtPD9fNoTRLKK\nwfB4DU3DXp+LIz/2kX5rjl8dANngphRgHvy8REgzKoZIjAhjd7o4pUPgNxISiJ5Y\n40daOI//6LcitsMdh8o3nsbfqQVIkmfyPX3RiMWqar0yzlwlGH3uqXozWfeq6ngg\nedRihklv7kxwxJjHJxujr2LQcWw0nGb/NmEEaTD+iCu/muLucc7V1PaCLs6SeIx2\npWRrvDPuTxZmfKe0suMk9kIIUiie8ItG+2quJsOhGGTzR9q9S4dbLCYkDdDB0JID\nLGblvyVnAgMBAAECggEASmH7JvSL2weruu5SOEKzl+3tvAnnd/zHB+TVDBiNqvte\n3uOa63/5Y1DZ0qao80lcstPTwR07VEwl3zTCSV8Pgnu5QeSV8LSkSkY19K0d7ydQ\nSGQki1qyc/QPlEjnV9C822cwO1cCckqcu0ne0JkZrSOf9g7Usv5gD1NgnIbcWx3S\nu+YC7G9dV6tV4xSxpC/8TrlaqfTwlpgz8W4a45SZDkSN0NLh7RFlOlqol6AftuMZ\nG+Oz64LuqdEGDrT2VnoQouD1QzY++Pykax0B/H9Mcl6pvITvoocwEmm96ySkK4Wb\neX+0WIN1c4gacr0BGx2jCsC3xnRMQsldZvTBGKeClQKBgQDYv9UBYGE06hJV7lr4\njvDOtxavZwIvkvFDwGrDgt64/An9FpNzogIaAjykOf30POHbagfwwpHBahwUZnFy\np60Uvpdq38ccLhlZbdfX93pl6ymnGZqKGiyVBp2X6H4ROvyldnxK/mr4kit4KAhd\nnkuDbpyxGk0kRB9CN3scwTIC2wKBgQC/M+XnwZ013tsD1clhGumh46d5ER1RFfTF\nULwM/tNCtUNIgnqKTv1/YuMSBL8/aE1h4zE8ssSBgdlmUyfJ9oltpoO8irP0C/xM\nauSwUlv9XBVPlHFZ3udAKIYv39msldY+ov/hLXwGK0KnX1bnMD5B40mO2NfY0AYc\nsekZ6RqfZQKBgQC4rOHxFUxdiF5tBvRNuYSlrEvQ0SSEol+ciXKt+HCKDnz4PecO\na+f8fxFqH9sn4UPS7gmr8iu8LwuTuiFJPmJzPVzH+7irYv/IOZXhdgO8mNxZerA4\noMqBgyx9ROM9DRQ8arLGNvWvquMMspdGJNxUNMefAjjeAIjdo2Xq/xT8ZwKBgQCi\nxZvDYKBlbAMnkKiPcknS9+tqtZcvoHFwSPrOVWGkOIu+/tUzSWulNpn4jPIDGNhH\nrRINimKfndtlsQT+elMkdc1QhQQNjuaKZ7WoJXUKi4nY5ZZZ29kiVV1nBDpbInPx\nfvI20HH1Dn7/3bottnRJwySr5w5aFT4ez5aR6FAxwQKBgDAt7s9cHNsoqumvdQkM\nQEP5/Tc7Fouw+coX67gnOGCqVYsEUykQLJ7oV+4VJWdUjfxkAooiWJRkrXod3+Hk\nELUlbQiLFrpY8YqXNxI4wI7uco+n/E0jf/QR++KqFRcfBT0d/w1BTnFtj7YTWUFQ\nUIVoEPDEIhjlWb0mQsqy25ZF\n-----END PRIVATE KEY-----\n"; } }
    public  string client_email { get { return "firebase-adminsdk-lx59r@mobyalps-c1277.iam.gserviceaccount.com"; } }
    public  string client_id { get { return "113529143261661774258"; } }
    public  string auth_uri { get { return "https://accounts.google.com/o/oauth2/auth"; } }
    public  string token_uri { get { return "https://oauth2.googleapis.com/token"; } }
    public  string auth_provider_x509_cert_url { get { return "https://www.googleapis.com/oauth2/v1/certs"; } }
    public  string client_x509_cert_url { get { return "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-lx59r%40mobyalps-c1277.iam.gserviceaccount.com"; } }
}

/*{
"type": "service_account",
"project_id": "mobyalps-c1277",
"private_key_id": "69817a7b711210686076aa3cf3d7a54b2411ca25",
"private_key": "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCh4xDLSL/kJSzA\npLMueyRuCesYOvY+YJOf+8Hcvbdm8twYqIOg/tAm6/NUHym2QKdGtPD9fNoTRLKK\nwfB4DU3DXp+LIz/2kX5rjl8dANngphRgHvy8REgzKoZIjAhjd7o4pUPgNxISiJ5Y\n40daOI//6LcitsMdh8o3nsbfqQVIkmfyPX3RiMWqar0yzlwlGH3uqXozWfeq6ngg\nedRihklv7kxwxJjHJxujr2LQcWw0nGb/NmEEaTD+iCu/muLucc7V1PaCLs6SeIx2\npWRrvDPuTxZmfKe0suMk9kIIUiie8ItG+2quJsOhGGTzR9q9S4dbLCYkDdDB0JID\nLGblvyVnAgMBAAECggEASmH7JvSL2weruu5SOEKzl+3tvAnnd/zHB+TVDBiNqvte\n3uOa63/5Y1DZ0qao80lcstPTwR07VEwl3zTCSV8Pgnu5QeSV8LSkSkY19K0d7ydQ\nSGQki1qyc/QPlEjnV9C822cwO1cCckqcu0ne0JkZrSOf9g7Usv5gD1NgnIbcWx3S\nu+YC7G9dV6tV4xSxpC/8TrlaqfTwlpgz8W4a45SZDkSN0NLh7RFlOlqol6AftuMZ\nG+Oz64LuqdEGDrT2VnoQouD1QzY++Pykax0B/H9Mcl6pvITvoocwEmm96ySkK4Wb\neX+0WIN1c4gacr0BGx2jCsC3xnRMQsldZvTBGKeClQKBgQDYv9UBYGE06hJV7lr4\njvDOtxavZwIvkvFDwGrDgt64/An9FpNzogIaAjykOf30POHbagfwwpHBahwUZnFy\np60Uvpdq38ccLhlZbdfX93pl6ymnGZqKGiyVBp2X6H4ROvyldnxK/mr4kit4KAhd\nnkuDbpyxGk0kRB9CN3scwTIC2wKBgQC/M+XnwZ013tsD1clhGumh46d5ER1RFfTF\nULwM/tNCtUNIgnqKTv1/YuMSBL8/aE1h4zE8ssSBgdlmUyfJ9oltpoO8irP0C/xM\nauSwUlv9XBVPlHFZ3udAKIYv39msldY+ov/hLXwGK0KnX1bnMD5B40mO2NfY0AYc\nsekZ6RqfZQKBgQC4rOHxFUxdiF5tBvRNuYSlrEvQ0SSEol+ciXKt+HCKDnz4PecO\na+f8fxFqH9sn4UPS7gmr8iu8LwuTuiFJPmJzPVzH+7irYv/IOZXhdgO8mNxZerA4\noMqBgyx9ROM9DRQ8arLGNvWvquMMspdGJNxUNMefAjjeAIjdo2Xq/xT8ZwKBgQCi\nxZvDYKBlbAMnkKiPcknS9+tqtZcvoHFwSPrOVWGkOIu+/tUzSWulNpn4jPIDGNhH\nrRINimKfndtlsQT+elMkdc1QhQQNjuaKZ7WoJXUKi4nY5ZZZ29kiVV1nBDpbInPx\nfvI20HH1Dn7/3bottnRJwySr5w5aFT4ez5aR6FAxwQKBgDAt7s9cHNsoqumvdQkM\nQEP5/Tc7Fouw+coX67gnOGCqVYsEUykQLJ7oV+4VJWdUjfxkAooiWJRkrXod3+Hk\nELUlbQiLFrpY8YqXNxI4wI7uco+n/E0jf/QR++KqFRcfBT0d/w1BTnFtj7YTWUFQ\nUIVoEPDEIhjlWb0mQsqy25ZF\n-----END PRIVATE KEY-----\n",
"client_email": "firebase-adminsdk-lx59r@mobyalps-c1277.iam.gserviceaccount.com",
"client_id": "113529143261661774258",
"auth_uri": "https://accounts.google.com/o/oauth2/auth",
"token_uri": "https://oauth2.googleapis.com/token",
"auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
"client_x509_cert_url": "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-lx59r%40mobyalps-c1277.iam.gserviceaccount.com"
}*/

