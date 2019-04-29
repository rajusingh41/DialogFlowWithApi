using System;

namespace DialogFlowWithApi
{
    /// <summary>
    /// 
    /// </summary>
    public class JoyConversation
    {

        /// <summary>
        /// JoyGuid
        /// </summary>
        public Guid JoyGuid { get; set; }

        /// <summary>
        /// JoyText
        /// </summary>
        public string JoyText { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConversationContext
    {
        /// <summary>
        /// ContextGuid
        /// </summary>
        public Guid ContextGuid { get; set; }
        /// <summary>
        /// ContextText
        /// </summary>
        public string ContextText { get; set; }
    }
}
