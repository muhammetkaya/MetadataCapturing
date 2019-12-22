using System;

namespace MetadataExtractor
{
    public class PostgresqlMetadataConstants
    {
        public const string Table = "SELECT * FROM INFORMATION_SCHEMA.TABLES";
        public const string Column = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS";
        public const string View = "SELECT * FROM INFORMATION_SCHEMA.VIEWS";
        public const string Trigger = "SELECT * FROM INFORMATION_SCHEMA.TRIGGERS";
        public const string KeyColumnUsage = "SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE";
    }
}
