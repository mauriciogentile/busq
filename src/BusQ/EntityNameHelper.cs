using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ringo.BusQ.ServiceBus.Messaging
{
    internal static class EntityNameHelper
    {
        public static bool ValidateSubQueueName(string subQueueName)
        {
            return Enumerable.Any<string>(new string[] { "$DeadLetterQueue" }, (Func<string, bool>)(s => s.Equals(subQueueName, StringComparison.OrdinalIgnoreCase)));
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
            else if (!path.Contains("/") && !path.Contains("|"))
            {
                return new string[2] { string.Empty, path };
            }
            else
            {
                int startIndex = path.LastIndexOf("$");
                if (startIndex == -1)
                    startIndex = path.LastIndexOf("|");
                return new string[2] { path.Substring(0, startIndex - 1), path.Substring(startIndex) };
            }
        }
    }
}
