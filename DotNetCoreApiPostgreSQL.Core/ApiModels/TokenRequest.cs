﻿namespace DotNetCoreApiPostgreSQL.Core.ApiModels
{
    public class TokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
