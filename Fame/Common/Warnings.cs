using System.Collections.Generic;

namespace Fame.Common
{
    public class Warnings
    {
        Dictionary<object, List<string>> messages = new Dictionary<object, List<string>> ();
        public void Add(string message, object sender)
        {
            if (messages[sender] == null) {
                messages[sender] = new List<string>();
            }
            messages[sender].Add(message);
        }
    }
}