using System;

namespace MetadataExtractor
{
    public abstract class DatabaseConnectionInfo
    {
       public string Username { get; set; }
       public string Password { get; set; }
       public string ServerAddress { get; set; }

       public abstract string GetConnectionString();
    }
}
