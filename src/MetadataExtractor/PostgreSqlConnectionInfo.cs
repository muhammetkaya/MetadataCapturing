using System;

namespace MetadataExtractor
{
    public class PostgreSqlConnectionInfo : DatabaseConnectionInfo
    {
        const string LOCAL_TEST = "**localtest";
        public override string GetConnectionString()
        {
            if (this.Username == LOCAL_TEST)
                return "User ID=postgres;Host=localhost;Port=5432;Database=postgres;";

            if (string.IsNullOrEmpty(this.Password))
                return $"User ID={this.Username};Host={this.ServerAddress};Port=5432;Database=postgres;";
            else
                return $"User ID={this.Username};Password={this.Password};Host={this.ServerAddress};Port=5432;Database=postgres;";
        }
    }
}
