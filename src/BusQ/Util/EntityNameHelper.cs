using System;
using System.Linq;

namespace Ringo.BusQ.Util
{
    internal static class EntityNameHelper
    {
        public static bool ValidateSubQueueName(string subQueueName)
        {
            return new[] { "$DeadLetterQueue" }.Any(s => s.Equals(subQueueName, StringComparison.OrdinalIgnoreCase));
        }

        public static string FormatSubscriptionPath(string topicPath, string subscriptionName)
        {
            return topicPath + "/" + "Subscriptions" + "/" + subscriptionName;
        }

        public static string NormalizeEntityName(string entityName)
        {
            return entityName.ToUpperInvariant();
        }

        public static string FormatSubQueuePath(string entityPath, string subQueueName)
        {
            return entityPath + "/" + subQueueName;
        }

        public static string FormatSubQueueEntityName(string pathDelimitedEntityName)
        {
            return pathDelimitedEntityName.Replace("/", "|");
        }

        public static string[] SplitSubQueuePath(string path)
        {
            if (!path.Contains("$"))
                return new string[2] { path, string.Empty };
            if (!path.Contains("/") && !path.Contains("|"))
            {
                return new string[2] { string.Empty, path };
            }
            int startIndex = path.LastIndexOf("$", StringComparison.Ordinal);
            if (startIndex == -1)
                startIndex = path.LastIndexOf("|", StringComparison.Ordinal);
            return new[] { path.Substring(0, startIndex - 1), path.Substring(startIndex) };
        }
    }
}
