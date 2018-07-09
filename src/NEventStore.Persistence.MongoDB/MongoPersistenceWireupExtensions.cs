// ReSharper disable once CheckNamespace
namespace NEventStore
{
    using System;
#if !NETSTANDARD1_6 && !NETSTANDARD2_0
    using System.Configuration;
#endif
    using NEventStore.Persistence.MongoDB;
    using NEventStore.Serialization;

    public static class MongoPersistenceWireupExtensions
    {
        // System.Configuration will not be ported to dotnet core
#if !NETSTANDARD1_6 && !NETSTANDARD2_0
        public static PersistenceWireup UsingMongoPersistence(this Wireup wireup, string connectionName, IDocumentSerializer serializer, MongoPersistenceOptions options = null)
        {
            return new MongoPersistenceWireup(wireup, () =>
            {
                var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionName];
                if( connectionStringSettings == null)
                    throw new NEventStore.Persistence.MongoDB.ConfigurationException(Messages.ConnectionNotFound.FormatWith(connectionName));

                return connectionStringSettings.ConnectionString;
            }, serializer, options);
        }
#else
        // little API change, let's pass in the connection string, do not assume we are reading from standard config files
        public static PersistenceWireup UsingMongoPersistence(this Wireup wireup, string connectionString, IDocumentSerializer serializer, MongoPersistenceOptions options = null)
        {
            return new MongoPersistenceWireup(wireup, () =>
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new NEventStore.Persistence.MongoDB.ConfigurationException(Messages.ConnectionNotFound.FormatWith(connectionString));

                return connectionString;
            }, serializer, options);
        }
#endif

        public static PersistenceWireup UsingMongoPersistence(this Wireup wireup, Func<string> connectionStringProvider, IDocumentSerializer serializer, MongoPersistenceOptions options = null)
        {
            return new MongoPersistenceWireup(wireup, connectionStringProvider, serializer, options);
        }
    }
}