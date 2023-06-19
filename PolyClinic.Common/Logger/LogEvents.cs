namespace PolyClinic.Common.Logger
{
    public static class LogEvents
    {
        public static string TraceMethodEntryMessage(string calledClassName, [System.Runtime.CompilerServices.CallerMemberName] string callerMethodName = "")
        {
            var message = "[OnMethodExecuting]\tMethod:\t" + calledClassName + "." + callerMethodName;
            return message;
        }

        public static string TraceMethodExitMessage(string calledClassName, [System.Runtime.CompilerServices.CallerMemberName] string callerMethodName = "")
        {
            var message = "[OnMethodExecuted]\t\tMethod:\t" + calledClassName + "." + callerMethodName;
            return message;
        }

        public static string ErrorMessage(string exceptionMessage, string calledClassName, [System.Runtime.CompilerServices.CallerMemberName] string callerMethodName = "")
        {
            var message = "[Exception]\t\tMethod:\t" + calledClassName + "." + callerMethodName
                            + "\n[Message]\t" + exceptionMessage;
            return message;
        }
    }
}