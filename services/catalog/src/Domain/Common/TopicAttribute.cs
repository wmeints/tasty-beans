using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Domain.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TopicAttribute: Attribute
    {
        public TopicAttribute(string topicName)
        {
            TopicName = topicName;
        }

        public string TopicName { get; set; }   
    }
}
