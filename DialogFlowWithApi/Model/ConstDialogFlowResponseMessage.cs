namespace DialogFlowWithApi
{
    public static class ConstDialogFlowResponseMessage
    {

        /// <summary>
        /// Hi {0} I am Joy. I can assist you in HR related queries. Some things you can ask me: Profile, Leave balance, Open task, Attendance, Salary Slip.
        /// </summary>
        public static string WelcomeMessage
        {
            get
            {
                return @"Hi {0} <br/>I am Joy. I can assist you in HR related queries.</br>Some things you can ask me: Profile, Leave balance, Open task, Attendance, Salary Slip.";
            }
        }

        public static string WelcomeExceptionMessage { get { return "Sorry. I could not connect the HR database. You may ask your question a little later."; } }
        /// <summary>
        /// You have not provide proper date format,Please provide date in dd/mm/yyyy (25/02/2019) format
        /// </summary>
        public static string UnformattedDate { get { return "You have not provided proper date format,</br> Please provide date in dd/mm/yyyy ie 25/02/2019 format"; } }

        /// <summary>
        /// your business unit is {0}, Company is {1} and designation is {2}
        /// </summary>
        public static string ProfileMessage { get { return "your business unit is {0}, Company is {1} and designation is {2}"; } }

        #region leave 
        /// <summary>
        /// your leave {0}
        /// </summary>
        public static string LeaveDetails { get { return "You have  {0}"; } }

        /// <summary>
        /// You do not have leave balance.
        /// </summary>
        public static string NoLeave { get { return "You do not have leave balance."; } }
        #endregion

        #region Open Task 

        /// <summary>
        /// Well done! You have completed all task
        /// </summary>
        public static string NoPendingTask { get { return "Well done! You have completed all task"; } }

        /// <summary>
        /// your have {0} pending task
        /// </summary>
        public static string PendingTask { get { return "You have {0} pending"; } }

        #endregion

        #region Salary Slip
        /// <summary>
        /// Your payslip is not available for <<month>>. You can ask for some other month.
        /// </summary>
        public static string NoSalarySlip { get { return "Your payslip is not available for {0}. You can ask for some other month."; } }

        /// <summary>
        /// Here is your link {1} to view payslip for {1}.
        /// </summary>
        public static string SalarySlip { get { return "Here is your link {0} to view payslip for {1}."; } }

        /// <summary>
        /// Please Provide Month name ie. Jan or January
        /// </summary>
        public static string MonthName { get { return "Please Provide Month name ie. Jan or January"; } }
        #endregion

        #region Attendance

        /// <summary>
        /// You were <<Status>> on <<Date>> and your check- in time is <<min punch>> and check-out time is <<max punch>>
        /// </summary>
        public static string Attendance { get { return "You were {0} on {1} and your check- in time is {2} and check-out time is {3}"; } }

        /// <summary>
        /// Your attendance is not available for <<date>>. You can ask for some other date.
        /// </summary>
        public static string NoAttendance { get { return "Your attendance is not available for {0}. You can ask for some other date."; } }

        #endregion
    }
}
