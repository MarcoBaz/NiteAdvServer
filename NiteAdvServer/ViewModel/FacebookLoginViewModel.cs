using System;

namespace NiteAdvServer.ViewModel;

public class FacebookLoginViewModel
{
    public string AccessToken { get; set; }
    public string UserId { get; set; }
    public DateTime Expire { get; set; }
}
