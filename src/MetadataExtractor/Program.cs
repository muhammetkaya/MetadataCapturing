using System;
using System.Collections.Generic;
using MetadataExtractor.Entities;
using MetadataExtractor.Extensions;
using Npgsql;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace MetadataExtractor
{
    class Program
    {
        public const int pageCount = 1000;
        static void Main(string[] args)
        {
            Initialize();

            var connectionInfo = GetConnectionInfoFromUser();

            Console.WriteLine("Trying to connect to the database..");

            ExtractMetadata(connectionInfo);
        }

        static void Initialize()
        {
            Console.WriteLine("Metadata Extractor From PostgreSql");
            Console.WriteLine("This project extracts database metadatas for postgresql");
            Console.WriteLine("Please follow the directions");
        }

        static DatabaseConnectionInfo GetConnectionInfoFromUser()
        {
            Console.WriteLine("You need to enter database connection information");

            Console.WriteLine("Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            Console.WriteLine("Server address:");
            string serverAddress = Console.ReadLine();

            return new PostgreSqlConnectionInfo()
            {
                Username = username,
                Password = password,
                ServerAddress = serverAddress
            };
        }

        static void ExtractMetadata(DatabaseConnectionInfo connectionInfo)
        {
            var connectionString = connectionInfo.GetConnectionString();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("Connection has succeeded");
                Console.WriteLine();

                Console.WriteLine("Table Metadata");
                ExtractTableMetadata(conn);

                Console.WriteLine("");

                Console.WriteLine("Column Metadata");
                ExtractColumnMetadata(conn);

                // Console.WriteLine("");

                // Console.WriteLine("View Metadata");
                // ExtractViewMetadata(conn);

                // Console.WriteLine("");

                // Console.WriteLine("Trigger Metadata");
                // ExtractTriggerMetadata(conn);

                Console.WriteLine("");

                Console.WriteLine("Table Constraints Metadata");
                ExtractKeyUsageMetadata(conn);

                Console.WriteLine("");
            }

            Console.WriteLine("Metadata has been extracted successfully");
        }

        private static void ExtractKeyUsageMetadata(NpgsqlConnection conn)
        {
            List<KeyColumnUsageMetadata> keyColumnUsageMetadatas = new List<KeyColumnUsageMetadata>();
            // Retrieve all rows
            using (var cmd = new NpgsqlCommand(PostgresqlMetadataConstants.KeyColumnUsage, conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    keyColumnUsageMetadatas.Add(new KeyColumnUsageMetadata()
                    {
                        ConstraintName = Convert.ToString(reader.GetValue(3)),
                        TableCatalog = Convert.ToString(reader.GetValue(4)),
                        TableSchema = Convert.ToString(reader.GetValue(5)),
                        TableName = Convert.ToString(reader.GetValue(6)),
                        ColumnName = Convert.ToString(reader.GetValue(7))
                    });
                }

            var metadataTable = new Table();
            metadataTable.SetHeaders(nameof(KeyColumnUsageMetadata.TableCatalog),
                                     nameof(KeyColumnUsageMetadata.TableSchema),
                                     nameof(KeyColumnUsageMetadata.ConstraintName),
                                     nameof(KeyColumnUsageMetadata.TableName),
                                     nameof(KeyColumnUsageMetadata.ColumnName));

            keyColumnUsageMetadatas.Take(pageCount).ToList().ForEach(m =>
            {
                metadataTable.AddRow(m.TableCatalog,
                m.TableSchema,
                m.ConstraintName,
                m.TableName,
                m.ColumnName
                );
            });

            //Console.WriteLine(metadataTable.ToString());
            Console.WriteLine(ToXmlString(keyColumnUsageMetadatas));
        }
        
        private static void ExtractTriggerMetadata(NpgsqlConnection conn)
        {

        }

        private static void ExtractViewMetadata(NpgsqlConnection conn)
        {

        }

        private static void ExtractColumnMetadata(NpgsqlConnection conn)
        {
            List<ColumnMetadataEntity> columnMetadatas = new List<ColumnMetadataEntity>();
            // Retrieve all rows
            using (var cmd = new NpgsqlCommand(PostgresqlMetadataConstants.Column, conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    columnMetadatas.Add(new ColumnMetadataEntity()
                    {
                        TableCatalog = Convert.ToString(reader.GetValue(0)),
                        TableSchema = Convert.ToString(reader.GetValue(1)),
                        TableName = Convert.ToString(reader.GetValue(2)),
                        ColumnName = Convert.ToString(reader.GetValue(3)),
                        // SelfReferencingColumnName = Convert.ToString(reader.GetValue(4)),
                        // ReferenceGeneration = Convert.ToString(reader.GetValue(5)),
                        IsNullable = Convert.ToString(reader.GetValue(6)),
                        DataType = Convert.ToString(reader.GetValue(7)),
                    });
                }

            var metadataTable = new Table();
            metadataTable.SetHeaders(nameof(ColumnMetadataEntity.TableCatalog),
                                     nameof(ColumnMetadataEntity.TableSchema),
                                     nameof(ColumnMetadataEntity.TableName),
                                     nameof(ColumnMetadataEntity.ColumnName),
                                     //  nameof(TableMetadataEntity.SelfReferencingColumnName),
                                     //  nameof(TableMetadataEntity.ReferenceGeneration),
                                     nameof(ColumnMetadataEntity.DataType),
                                     nameof(ColumnMetadataEntity.IsNullable));

            columnMetadatas.Take(pageCount).ToList().ForEach(m =>
            {
                metadataTable.AddRow(m.TableCatalog,
                m.TableSchema,
                m.TableName,
                m.ColumnName,
                m.DataType,
                m.IsNullable
                );
            });

            //Console.WriteLine(metadataTable.ToString());
            Console.WriteLine(ToXmlString(columnMetadatas));
        }

        static void ExtractTableMetadata(NpgsqlConnection connection)
        {
            List<TableMetadataEntity> tableMetadatas = new List<TableMetadataEntity>();
            // Retrieve all rows
            using (var cmd = new NpgsqlCommand(PostgresqlMetadataConstants.Table, connection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    tableMetadatas.Add(new TableMetadataEntity()
                    {
                        TableCatalog = Convert.ToString(reader.GetValue(0)),
                        TableSchema = Convert.ToString(reader.GetValue(1)),
                        TableName = Convert.ToString(reader.GetValue(2)),
                        Type = Convert.ToString(reader.GetValue(3)),
                        // SelfReferencingColumnName = Convert.ToString(reader.GetValue(4)),
                        // ReferenceGeneration = Convert.ToString(reader.GetValue(5)),
                        UserDefinedTypeCatalog = Convert.ToString(reader.GetValue(6)),
                        UserDefinedTypeSchema = Convert.ToString(reader.GetValue(7)),
                        UserDefinedTypeName = Convert.ToString(reader.GetValue(8)),
                        IsInsertableInto = Convert.ToString(reader.GetValue(9)),
                        IsTyped = Convert.ToString(reader.GetValue(10))
                        // CommitAction = Convert.ToString(reader.GetValue(11))
                    });
                }

            var metadataTable = new Table();
            metadataTable.SetHeaders(nameof(TableMetadataEntity.TableCatalog),
                                     nameof(TableMetadataEntity.TableSchema),
                                     nameof(TableMetadataEntity.TableName),
                                     nameof(TableMetadataEntity.Type),
                                     //  nameof(TableMetadataEntity.SelfReferencingColumnName),
                                     //  nameof(TableMetadataEntity.ReferenceGeneration),
                                     nameof(TableMetadataEntity.UserDefinedTypeCatalog),
                                     nameof(TableMetadataEntity.UserDefinedTypeSchema),
                                     nameof(TableMetadataEntity.UserDefinedTypeName),
                                     nameof(TableMetadataEntity.IsInsertableInto),
                                     nameof(TableMetadataEntity.IsTyped)
            //  nameof(TableMetadataEntity.CommitAction)
            );

            tableMetadatas.Take(pageCount).ToList().ForEach(m =>
            {
                metadataTable.AddRow(m.TableCatalog,
                m.TableSchema,
                m.TableName,
                m.Type,
                // m.SelfReferencingColumnName,
                // m.ReferenceGeneration,
                m.UserDefinedTypeCatalog,
                m.UserDefinedTypeSchema,
                m.UserDefinedTypeName,
                m.IsInsertableInto,
                m.IsTyped
                // m.CommitAction
                );
            });

            //Console.WriteLine(metadataTable.ToString());
            Console.WriteLine(ToXmlString(tableMetadatas));
        }

        private static string ToXmlString<T>(T data)
        {
            string xmlString = string.Empty;
            using (var ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, data);
                ms.Position = 0;
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }
    }
}
