using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.DTO;

public class UserDTO:VertexDTO
{
    public UserDTO()
    {
        IdNation = 0;
    }
    public string Name { get; set; }
    public string Surename { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserImageLink { get; set; }
    public bool IsCompany { get; set; }
    public bool UserActivated { get; set; }
    public DateTime RegistrationDate { get; set; }
    public long IdNation { get; set; }
}
