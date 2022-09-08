using AutoMapper;
using NiteAdvServerCore.DTO;
using NiteAdvServerCore.Entities;
using NiteAdvServerCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NiteAdvServerCore.Util
{
    public class DatetimeConverter : IValueConverter<double, DateTime>
    {
        public DateTime Convert(double source, ResolutionContext context)
        {
            try
            {
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return dt.AddMilliseconds(source);
            }
            catch(Exception ex)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }
           
        }
    }

    public class DoubleConverter : IValueConverter<DateTime, double>
    {
        public double Convert(DateTime source, ResolutionContext context)
        {
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan ts = source - startDate;
            return ts.TotalMilliseconds;
        }
    }
    
    public  static class MapperConfigurator
    {
        public static MapperConfiguration Configure
        {
            get
            {
              
                return new MapperConfiguration(cfg =>
                {
                    cfg.ShouldMapField = f => false;
                    cfg.ShouldMapProperty = p => (p.GetMethod.IsPublic && !p.GetMethod.IsAbstract) || p.GetMethod.IsAssembly;
                    cfg.AllowNullCollections = true;
                    cfg.AllowNullDestinationValues = true;

                });
              
            }
        }

        public static CompanyDTO ConvertCompany(Company cmp)
        {
            CompanyDTO dto = new CompanyDTO();
            dto.OriginalId = (string) cmp.id;
            dto.Name = cmp.Name;
            dto.Url = cmp.Url;
            dto.Label = cmp.label;
            dto.Longitude = cmp.Longitude;
            dto.Latitude = cmp.Latitude;
            dto.City = cmp.City;
            dto.Country = cmp.Country;
            dto.Street = cmp.Street;
            dto.LastSyncDate = ConvertLastSyncDate<Company>(cmp);
            dto.ImageUrl = cmp.ImageUrl;
            dto.GooglePlaceId = cmp.GooglePlaceId;
            dto.Type = cmp.Type;
            dto.GoogleTypes =cmp.GoogleTypes; 
            dto.Rating = cmp.Rating;
            dto.Reviews = cmp.Reviews;
            dto.GoogleUrl = cmp.GoogleUrl;
            dto.WebSite = cmp.WebSite;
            dto.Email = cmp.Email;
            dto.Phone = cmp.Phone;
            dto.OpeningHours = cmp.OpeningHours;
            dto.RatingTotal = cmp.RatingTotal;
            return dto;

        }
        public static EventDTO ConvertEvent(Event eve, string CompanyId)
        {
            EventDTO dto = new EventDTO();
            dto.OriginalId = (string) eve.id;
            dto.Url = eve.Url;
            dto.Label = eve.label;
            dto.Name = eve.Name;
            dto.Description = eve.Description;
            dto.StartDate = ServerUtil.GetDateTimeFromUnixFormat(eve.StartTimestamp,TimestampFormatter.Seconds);
            dto.EndDate = ServerUtil.GetDateTimeFromUnixFormat(eve.EndTimestamp,TimestampFormatter.Seconds);
            dto.ImageLink = eve.Image;
            dto.LastSyncDate = ConvertLastSyncDate<Event>(eve);
            dto.CompanyId = CompanyId;
            dto.Latitude = eve.Latitude;
            dto.Longitude = eve.Longitude;
            dto.CategoryType = eve.CategoryType;
            dto.DiscoveryCategories = eve.DiscoveryCategories;
            dto.Place = eve.Place;
            dto.TicketingContext = eve.TicketingContext;
            dto.TicketUrl = eve.TicketUrl;
            dto.UsersGoing = eve.UsersGoing;
            dto.UsersInterested = eve.UsersInterested;

            return dto;

        }
        public static User ConvertUserDTO(UserDTO usDTO)
        {
            User dto = new User(usDTO.OriginalId);
            dto.Email = usDTO.Email;
            dto.Name = usDTO.Name;
            dto.Surename = usDTO.Surename;
            dto.Password = usDTO.Password;
            dto.BirthDate = ServerUtil.GetUnixFormatDateTime(usDTO.BirthDate?? new DateTime(1970,1,1), TimestampFormatter.Seconds);
            dto.RegistrationDate = ServerUtil.GetUnixFormatDateTime(usDTO.RegistrationDate, TimestampFormatter.Seconds);
            dto.LastSyncDate = ServerUtil.GetUnixFormatDateTime(usDTO.LastSyncDate);
            dto.PhoneNumber = usDTO.PhoneNumber;
            dto.UserImageLink = usDTO.UserImageLink;
            dto.IsCompany = usDTO.IsCompany;
            dto.IdNation = usDTO.IdNation;

            return dto;

        }

        public static UserDTO ConvertUser(User us)
        {
            UserDTO dto = new UserDTO();
            dto.OriginalId = (string) us.id;
            dto.Email = us.Email;
            dto.Name = us.Name;
            dto.Surename = us.Surename;
            dto.Password = us.Password;
            dto.BirthDate = ServerUtil.GetDateTimeFromUnixFormat(us.BirthDate, TimestampFormatter.Seconds);
            dto.RegistrationDate = ServerUtil.GetDateTimeFromUnixFormat(us.RegistrationDate, TimestampFormatter.Seconds);
            dto.LastSyncDate = ConvertLastSyncDate<User>(us);
            dto.PhoneNumber = us.PhoneNumber;
            dto.UserImageLink = us.UserImageLink;
            dto.IsCompany = us.IsCompany;
            dto.IdNation = us.IdNation;

            return dto;

        }

        public static DateTime ConvertLastSyncDate<T>(T input) where T : VertexEntity
        {
            try
            {
                return ServerUtil.GetDateTimeFromUnixFormat(input.LastSyncDate);
            }
            catch (Exception ex)
            {
                //    //il formato della data è sbagliato
                //    //aggiorno il vertex con la data giusta
                //var date = DateTime.UtcNow.AddDays(-7);
                //input.LastSyncDate = ServerUtil.GetUnixFormatDateTime(date);
                //GremlinManager.Instance.SaveVertex<T>(input, false);
                return DateTime.MinValue;
            }


        }
    }

    public enum TimestampFormatter
    {
        Milliseconds,
        Seconds
    }
}
