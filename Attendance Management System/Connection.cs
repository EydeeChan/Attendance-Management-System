namespace Attendance_Management_System
{
    class Connection
    {
        private static string server = "127.0.0.1";
        private static string database = "tshs_sms";
        private static string uid = "root";
        private static string password = "b0c56191b04f4583ba07976e10003209";
        private static string port = "3306";
        private static string connstr = "";

        public static string GetConnectionStr()
        {
            return connstr = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "PORT=" + port + ";";
        }

        public static void SetServer(string server)
        {
            Connection.server = server;
        }
        public static void SetDatabase(string database)
        {
            Connection.database = database;
        }

        public static void SetUid(string uid)
        {
            Connection.uid = uid;
        }
        public static void SetPassword(string pass)
        {
            Connection.password = pass;
        }
        public static void SetPort(string port)
        {
            Connection.port = port;
        }
    }
}
