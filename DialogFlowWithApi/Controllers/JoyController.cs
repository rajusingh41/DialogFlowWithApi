using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Grpc.Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DialogFlowWithApi.Controllers
{
    [Route("api/joy")]
    [ApiController]
    public class JoyController : ControllerBase
    {
        private IHostingEnvironment _env;
        public JoyController(IHostingEnvironment env)
        {
            _env = env;
        }
        int currentUserId = 10;
        /// <summary>
        /// InitiateConversation
        /// (Ignore.Action)
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ConversationContext), StatusCodes.Status200OK)]
        public IActionResult InitiateConversation()
        {
            var authFilter = string.Empty;
            //create sessionId
            var joyGuid = Guid.NewGuid();
            var clientSessions = CreateDialogFlowClientSessions();
            //var model = new BaseEntityModel { DomainCode = CurrentUser.DomainCode, LogOnUserId = CurrentUser.UserId };

            var conversationContext = new ConversationContext
            {
                ContextGuid = joyGuid,
                ContextText = InitConversation(clientSessions, authFilter, currentUserId.ToString(CultureInfo.CurrentCulture), joyGuid)
            };
            return Ok(conversationContext);
        }

        /// <summary>
        /// RequestToDialogFlow (Ignore.Action)
        /// </summary>
        /// <remarks>
        ///     { 
        ///     "joyGuid": "00000000-0000-0000-0000-000000000000", 
        ///     "joyText": "string" 
        ///     } 
        /// </remarks>
        /// <param name="conversion"></param>
        /// <returns>string</returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult RequestToDialogFlow([FromBody] JoyConversation conversion)
        {
            if (conversion == null)
            {
                throw new ArgumentNullException(nameof(conversion));
            }
            var authFilter = string.Empty;
            var clientSessions = CreateDialogFlowClientSessions();
            var result = GetAgentResponse(clientSessions, conversion.JoyText, currentUserId.ToString(CultureInfo.CurrentCulture), authFilter, conversion.JoyGuid);
            var responseArray = result.Split(':');
            return Ok(result);
        }

        /// <summary>
        /// GetAgentResponse
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="joyText"></param>
        /// <param name="joyUserId"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "reinitializeConversation")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [NonAction]
        private string GetAgentResponse(SessionsClient clientSessions, string joyText, string joyUserId, string authFilter, Guid joyGuid)
        {
            string result = string.Empty;

            var response = clientSessions.DetectIntent(
                    session: new SessionName("hr-joy", joyGuid.ToString("N", CultureInfo.CurrentCulture)),
                    queryInput: new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = joyText,
                            LanguageCode = ConstDialogFlow.Language
                        }
                    }
                );
            if (response.WebhookStatus == null) // retry
            {
#pragma warning disable S1481 // Unused local variables should be removed
                var reinitializeConversation = ReInitConversation(clientSessions, authFilter, joyUserId, joyGuid);
#pragma warning restore S1481 // Unused local variables should be removed
                response = clientSessions.DetectIntent(
                    session: new SessionName("hr-joy", joyGuid.ToString("N", CultureInfo.CurrentCulture)),
                    queryInput: new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = joyText,
                            LanguageCode = ConstDialogFlow.Language
                        }
                    }
                );
            }

            var queryResult = response.QueryResult;
            result = queryResult.FulfillmentText;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [NonAction]
        private string ReInitConversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid)
        {
            return Conversation(clientSessions, authFilter, joyUserId, joyGuid, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="isReInit"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        private string Conversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid, bool isReInit)
        {
            EventInput evnt = new EventInput
            {
                Name = isReInit ? ConstDialogFlow.DialogFlowReinitializeConversationEvent : ConstDialogFlow.DialogFlowInitConversationEvent,
                LanguageCode = ConstDialogFlow.Language
            };
            evnt.Parameters = new Google.Protobuf.WellKnownTypes.Struct();

            Google.Protobuf.WellKnownTypes.Value v1 = new Google.Protobuf.WellKnownTypes.Value();
            v1.StringValue = joyUserId;
            Google.Protobuf.WellKnownTypes.Value v2 = new Google.Protobuf.WellKnownTypes.Value();
            v2.StringValue = authFilter;

            evnt.Parameters.Fields.Add(ConstDialogFlow.DialogFlowUserName, v1);
            evnt.Parameters.Fields.Add(ConstDialogFlow.DialogFlowToken, v2);

            var response = clientSessions.DetectIntent(
                session: new SessionName("hr-joy", joyGuid.ToString("N", CultureInfo.CurrentCulture)),
                queryInput: new QueryInput
                {
                    Event = evnt
                }
            );

            var queryResult = response.QueryResult;
            //   var joyUserDetails = _subscribeModuleService.GetIdentifier();
            return string.IsNullOrEmpty(queryResult.FulfillmentText) ?
            ConstDialogFlowResponseMessage.WelcomeExceptionMessage
            : string.Format(CultureInfo.CurrentCulture, ConstDialogFlowResponseMessage.WelcomeMessage, "Raju Singh");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSessions"></param>
        /// <param name="authFilter"></param>
        /// <param name="joyUserId"></param>
        /// <param name="joyGuid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [NonAction]
        private string InitConversation(SessionsClient clientSessions, string authFilter, string joyUserId, Guid joyGuid)
        {
            return Conversation(clientSessions, authFilter, joyUserId, joyGuid, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [NonAction]
        private SessionsClient CreateDialogFlowClientSessions()
        {
            var dialogFlowConfigurationFilePath = _env.ContentRootPath+"/App_Data";
            GoogleCredential googleCredential = GoogleCredential.FromFile(dialogFlowConfigurationFilePath + "/hr-joy-26ab70a34ae2.json");
            Channel channel = new Channel(SessionsClient.DefaultEndpoint.Host,
                          googleCredential.ToChannelCredentials());
            return SessionsClient.Create(channel);
        }


    }
}