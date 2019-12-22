using System;

namespace MetadataExtractor.Entities
{
    public class KeyColumnUsageMetadata
    {
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ConstraintName { get; set; }
        public string ColumnName { get; set; }
    }
}
