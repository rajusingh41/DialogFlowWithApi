using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DialogFlowWithApi
{
    public class JoyMessage
    {
    }

    public class ResponseMessage
    {
        public ICollection<ModuleMessage> ModuleMessage { get; set; }
    }

    public class ModuleMessage
    {
        public int ModuleId { get; set; }

        public string Comment { get; set; }

        public ICollection<ProcessTypeMessage> ProcessTypeMessage { get; set; }
    }

    public class ProcessTypeMessage
    {
        public int ProcessId { get; set; }
        public string Comment { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
