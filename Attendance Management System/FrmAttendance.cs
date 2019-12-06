using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Attendance_Management_System
{
    public partial class FrmAttendance : Form
    {
        String account_id = "";
        String account_status = "";
        public FrmAttendance()
        {
            InitializeComponent();
        }
        private const int cs = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cs;
                return cp;
            }
        }
        private void DateAndTime_Tick(object sender, EventArgs e)
        {
            lblweek.Text = DateTime.Now.ToString("dddd  MMMM dd yyyy");
            lbltime.Text = DateTime.Now.ToString("hh:mm");
            lblsec.Text = DateTime.Now.ToString("ss");
            lblam.Text = DateTime.Now.ToString("tt");
        }

        private void FrmAttendance_Load(object sender, EventArgs e)
        {
            DateAndTime.Start();
            String a = @"C:\video.mp4";
            axWindowsMediaPlayer1.URL = a;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            GlobalVar.time = "Time In";
            clear();
            this.ActiveControl = textBox1;
            PresentTeacher();
            PresentStudent();
            Retrieve();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            }
            else if (e.KeyCode.ToString() == "F3")
            {
                GlobalVar.time = "Time In";
                this.ActiveControl = textBox1;
            }
            else if (e.KeyCode.ToString() == "F4")
            {
                GlobalVar.time = "Time Out";
                this.ActiveControl = textBox1;
            }
            else if (e.KeyCode.ToString() == "F5")
            {
                clear();
                this.ActiveControl = textBox1;
            }
        }
        #region Clear
        void clear()
        {
            lbllastname.Text = "";
            lbllrn.Text = "";
            lbltrack.Text = "";
            lblgrade.Text = "";
            lblsection.Text = "";
            lblstatus.Text = "";
            lblcurdate.Text = "";
            lbladviser.Text = "";
            lblsemester.Text = "";
            lblstrand.Text = "";
            DateAndTime.Start();
        }
        #endregion
        private void RetrieveInfo_Click(object sender, EventArgs e)
        {
            clear();
            String contactno = null;
            String section_id = null;
            String remarks = null;
            String statuss = null;
            String aaaa = null;
            String last_name = "";
            String first_name = "";
            String middle_name = "";
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            #region Get Account Status
            String query333 = "select account_id, account_status from rfid  where rfid_code = " + textBox1.Text + "";
            con.Open();
            MySqlCommand cmd3 = new MySqlCommand(query333, con);
            cmd3.CommandTimeout = 500;
            MySqlDataReader dr2 = cmd3.ExecuteReader();
            if (dr2.Read())
            {
                GlobalVar.account_status = (dr2["account_status"].ToString());
                GlobalVar.account_id = (dr2["account_id"].ToString());
            }
            dr2.Close();
            con.Close();
            #endregion
            #region Get Week Day
            String query222 = "SELECT WEEKDAY(CURDATE())";
            con.Open();
            MySqlCommand cmd4 = new MySqlCommand(query222, con);
            cmd4.CommandTimeout = 500;
            MySqlDataReader dr4 = cmd4.ExecuteReader();
            if (dr4.Read())
            {
                GlobalVar.weekday = (dr4["WEEKDAY(CURDATE())"].ToString());
            }
            dr4.Close();
            con.Close();
            #endregion
            #region if already time in
            string query555 = "select * from attendance  where timein = curdate() and account_id = " + GlobalVar.account_id + "";
            con.Open();
            MySqlCommand cmd555 = new MySqlCommand(query555, con);
            cmd555.CommandTimeout = 500;
            try
            {
                MySqlDataReader dr222 = cmd555.ExecuteReader();
                if (dr2.Read())
                {
                    aaaa = (dr222["account_id"].ToString());
                }
                dr222.Close();
                con.Close();
                statuss = "Already";
            }
            catch
            {
                statuss = "";
            }
            #endregion
            if (GlobalVar.account_status == "Student")
            {
                String timein = "";
                String currenttime = "";
                #region compare weekday
                String queryweek = "select '" + GlobalVar.timein + "' from timein where account_id = '" + GlobalVar.account_id + "' and '" + GlobalVar.weekday + "' = '" + GlobalVar.weekday + "'";
                con.Open();
                MySqlCommand cmd5 = new MySqlCommand(queryweek, con);
                cmd5.CommandTimeout = 500;
                MySqlDataReader dr6 = cmd5.ExecuteReader();
                if (dr6.Read())
                {
                    timein = (dr6[GlobalVar.timein].ToString());
                    currenttime = DateTime.Now.ToString("HH");
                    GlobalVar.timein = Int32.Parse(timein);
                    GlobalVar.currenttime = Int32.Parse(currenttime);
                }
                dr6.Close();
                con.Close();
                #endregion
                FindImage();
                #region query first
                String query = "select rfid.account_id, rfid.account_status, student.lrn, student.last_name, student.first_name,student.middle_name, student.section_id, student.semester, student.remarks, student.contactno from rfid inner join student on rfid.account_id = student.account_id where rfid_code = " + textBox1.Text + "";
                MySqlCommand cmd1 = new MySqlCommand(query, con);
                cmd1.CommandTimeout = 500;
                con.Open();
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    last_name = (dr1["last_name"].ToString());
                    first_name = (dr1["first_name"].ToString());
                    middle_name = (dr1["middle_name"].ToString());
                    lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    lbllrn.Text = (dr1["lrn"].ToString());
                    account_status = (dr1["account_status"].ToString());
                    lblsemester.Text = (dr1["semester"].ToString()) + " Semester";
                    remarks = (dr1["remarks"].ToString());
                    lblstatus.Text = remarks;
                    section_id = (dr1["section_id"].ToString());
                    account_id = (dr1["account_id"].ToString());
                    contactno = (dr1["contactno"].ToString());
                }
                dr1.Close();
                con.Close();
                #endregion
                #region query 2
                if (section_id == "")
                {

                }
                else
                {
                    String query1 = "select * from section where section_id = " + section_id + "";
                    con.Open();
                    MySqlCommand cmd2 = new MySqlCommand(query1, con);
                    MySqlDataReader dr5 = cmd2.ExecuteReader();
                    if (dr5.Read())
                    {
                        lblgrade.Text = (dr5["grade"].ToString());
                        lblsection.Text = (dr5["section"].ToString());
                        lbltrack.Text = (dr5["track"].ToString());
                        lblstrand.Text = (dr5["strand"].ToString());
                        lbladviser.Text = (dr5["adviser"].ToString());
                    }
                    dr5.Close();
                    lblcurdate.Text = DateTime.Now.ToString("hh:mm tt");
                }
                #endregion
            }
            else if (GlobalVar.account_status == "Teacher")
            {
                String timein = "";
                String currenttime = "";
                #region compare weekday
                String queryweek = "select '" + GlobalVar.timein + "' from timein where account_id = '" + GlobalVar.account_id + "' and '" + GlobalVar.weekday + "' = '" + GlobalVar.weekday + "'";
                con.Open();
                MySqlCommand cmd5 = new MySqlCommand(queryweek, con);
                cmd5.CommandTimeout = 500;
                MySqlDataReader dr6 = cmd5.ExecuteReader();
                if (dr6.Read())
                {
                    timein = (dr6[GlobalVar.timein].ToString());
                    currenttime = DateTime.Now.ToString("HH");
                    GlobalVar.timein = Int32.Parse(timein);
                    GlobalVar.currenttime = Int32.Parse(currenttime);
                }
                dr6.Close();
                con.Close();
                #endregion
                FindImageTeacher();
                #region query first
                String queryteacher = "select rfid.account_id, rfid.account_status, concat(teacher.first_name, teacher.middle_name, teacher.last_name) as full_name, teacher.track, teacher.strand, teacher.employee_number, teacher.position, teacher.ancillary_assignment, teacher.contactno from rfid inner join teacher on rfid.account_id = teacher.account_id where rfid_code = " + textBox1.Text + "";
                MySqlCommand cmd1 = new MySqlCommand(queryteacher, con);
                cmd1.CommandTimeout = 500;
                con.Open();
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    account_id = (dr1["account_id"].ToString());
                    last_name = (dr1["last_name"].ToString());
                    first_name = (dr1["first_name"].ToString());
                    middle_name = (dr1["middle_name"].ToString());
                    lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    lbllrn.Text = (dr1["employee_number"].ToString());
                    contactno = (dr1["contactno"].ToString());
                    lbltrack.Text = (dr1["track"].ToString());
                    lblstrand.Text = (dr1["strand"].ToString());
                    lblcurdate.Text = DateTime.Now.ToString("hh:mm tt");
                    lbladviser.Text = (dr1["ancillary_assignment"].ToString());
                    lblgrade.Text = (dr1["position"].ToString());
                }
                dr1.Close();
                con.Close();
                #endregion
            }
            if (statuss == null)
            {
                if (GlobalVar.account_status == "Student")
                {
                    if (GlobalVar.timein <= GlobalVar.currenttime)
                    {
                        String status = "on time";
                        this.ActiveControl = textBox1;
                        InsertData(account_id, status, first_name, middle_name, last_name);
                        lblontime.Text = "On Time";
                        lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        String status = "late";
                        this.ActiveControl = textBox1;
                        InsertData(account_id, status, first_name, middle_name, last_name);
                        lblontime.Text = "Late";
                        lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    PresentStudent();
                    viewAttendance a = new viewAttendance();
                    MemoryStream ms = new MemoryStream();
                    PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
                    byte[] aaa = ms.GetBuffer();
                    ms.Close();
                    MemoryStream msa = new MemoryStream(aaa);
                    a.PictureStudent.Image = Image.FromStream(msa);
                    a.lbllrn.Text = lbllrn.Text;
                    a.lblstrand.Text = lblstrand.Text;
                    a.lbltrack.Text = lbltrack.Text;
                    a.lblcurdate.Text = DateTime.Now.ToString();
                    String time = lblontime.Text;
                    if (time == "On Time")
                    {
                        a.lblontime.Text = "On Time";
                        a.lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (time == "Late")
                    {
                        a.lblontime.Text = "Late";
                        a.lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    flowLayoutPanel1.Controls.Add(a);
                }
                else if (GlobalVar.account_status == "Teacher")
                {
                    if (GlobalVar.timein <= GlobalVar.currenttime)
                    {
                        String status = "on time";
                        this.ActiveControl = textBox1;
                        InsertDataTeacher(account_id, status, first_name, middle_name, last_name);
                        lblontime.Text = "On Time";
                        lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        String status = "late";
                        this.ActiveControl = textBox1;
                        InsertDataTeacher(account_id, status, first_name, middle_name, last_name);
                        lblontime.Text = "Late";
                        lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    PresentTeacher();
                    viewAttendance a = new viewAttendance();
                    MemoryStream ms = new MemoryStream();
                    PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
                    byte[] aaa = ms.GetBuffer();
                    ms.Close();
                    MemoryStream msa = new MemoryStream(aaa);
                    a.PictureStudent.Image = Image.FromStream(msa);
                    a.lbllrn.Text = lbllrn.Text;
                    a.lblstrand.Text = lblstrand.Text;
                    a.lbltrack.Text = lbltrack.Text;
                    a.lblcurdate.Text = DateTime.Now.ToString();
                    String time = lblontime.Text;
                    if (time == "Present")
                    {
                        a.lblontime.Text = "Time In";
                        a.lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (time == "Time Out")
                    {
                        a.lblontime.Text = "Time Out";
                        a.lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    flowLayoutPanel1.Controls.Add(a);
                }
            }
        }
        #region Count Present Student
        void PresentStudent()
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select count(*) from attendance where status = 'Present' and account_status = 'student' and timein = curdate()";
            MySqlCommand cmd = new MySqlCommand(query, con);
            lblstudentpresent.Text = cmd.ExecuteScalar().ToString();
        }
        #endregion
        #region Count Present Teacher
        void PresentTeacher()
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select count(*) from attendance where status = 'Present' and account_status = 'teacher' and timein = curdate()";
            MySqlCommand cmd = new MySqlCommand(query, con);
            lblteacherpresent.Text = cmd.ExecuteScalar().ToString();
        }
        #endregion
        private void RetrieveInfoOut_Click(object sender, EventArgs e)
        {
            clear();
            String contactno = null;
            String section_id = null;
            String remarks = null;
            String statuss = null;
            String aaaa = null;
            String last_name = "";
            String first_name = "";
            String middle_name = "";
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            #region Get Account Status
            String query333 = "select account_id, account_status from rfid  where rfid_code = " + textBox1.Text + "";
            con.Open();
            MySqlCommand cmd3 = new MySqlCommand(query333, con);
            cmd3.CommandTimeout = 500;
            MySqlDataReader dr2 = cmd3.ExecuteReader();
            if (dr2.Read())
            {
                GlobalVar.account_status = (dr2["account_status"].ToString());
                GlobalVar.account_id = (dr2["account_id"].ToString());
            }
            dr2.Close();
            con.Close();
            #endregion
            #region Get Week Day
            String query222 = "SELECT WEEKDAY(CURDATE())";
            con.Open();
            MySqlCommand cmd4 = new MySqlCommand(query222, con);
            cmd4.CommandTimeout = 500;
            MySqlDataReader dr4 = cmd4.ExecuteReader();
            if (dr4.Read())
            {
                GlobalVar.weekday = (dr4["WEEKDAY(CURDATE())"].ToString());
            }
            dr4.Close();
            con.Close();
            #endregion
            #region if already time in
            string query555 = "select * from attendance  where status = Time Out and account_id = " + GlobalVar.account_id + "";
            con.Open();
            MySqlCommand cmd555 = new MySqlCommand(query555, con);
            cmd555.CommandTimeout = 500;
            try
            {
                MySqlDataReader dr222 = cmd555.ExecuteReader();
                if (dr2.Read())
                {
                    aaaa = (dr222["account_id"].ToString());
                }
                dr222.Close();
                con.Close();
                statuss = "Already";
            }
            catch
            {
                statuss = "";
            }
            #endregion
            if (GlobalVar.account_status == "Student")
            {
                String timein = "";
                String currenttime = "";
                #region compare weekday
                String queryweek = "select '" + GlobalVar.timein + "' from timein where account_id = '" + GlobalVar.account_id + "' and '" + GlobalVar.weekday + "' = '" + GlobalVar.weekday + "'";
                con.Open();
                MySqlCommand cmd5 = new MySqlCommand(queryweek, con);
                cmd5.CommandTimeout = 500;
                MySqlDataReader dr6 = cmd5.ExecuteReader();
                if (dr6.Read())
                {
                    timein = (dr6[GlobalVar.timein].ToString());
                    currenttime = DateTime.Now.ToString("HH");
                    GlobalVar.timein = Int32.Parse(timein);
                    GlobalVar.currenttime = Int32.Parse(currenttime);
                }
                dr6.Close();
                con.Close();
                #endregion
                FindImage();
                #region query first
                String query = "select rfid.account_id, rfid.account_status, student.lrn, student.last_name, student.first_name,student.middle_name, student.section_id, student.semester, student.remarks, student.contactno from rfid inner join student on rfid.account_id = student.account_id where rfid_code = " + textBox1.Text + "";
                MySqlCommand cmd1 = new MySqlCommand(query, con);
                cmd1.CommandTimeout = 500;
                con.Open();
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    last_name = (dr1["last_name"].ToString());
                    first_name = (dr1["first_name"].ToString());
                    middle_name = (dr1["middle_name"].ToString());
                    lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    lbllrn.Text = (dr1["lrn"].ToString());
                    account_status = (dr1["account_status"].ToString());
                    lblsemester.Text = (dr1["semester"].ToString()) + " Semester";
                    remarks = (dr1["remarks"].ToString());
                    lblstatus.Text = remarks;
                    section_id = (dr1["section_id"].ToString());
                    account_id = (dr1["account_id"].ToString());
                    contactno = (dr1["contactno"].ToString());
                }
                dr1.Close();
                con.Close();
                #endregion
                #region query 2
                if (section_id == "")
                {

                }
                else
                {
                    String query1 = "select * from section where section_id = " + section_id + "";
                    con.Open();
                    MySqlCommand cmd2 = new MySqlCommand(query1, con);
                    MySqlDataReader dr5 = cmd2.ExecuteReader();
                    if (dr5.Read())
                    {
                        lblgrade.Text = (dr5["grade"].ToString());
                        lblsection.Text = (dr5["section"].ToString());
                        lbltrack.Text = (dr5["track"].ToString());
                        lblstrand.Text = (dr5["strand"].ToString());
                        lbladviser.Text = (dr5["adviser"].ToString());
                    }
                    dr5.Close();
                    lblcurdate.Text = DateTime.Now.ToString("hh:mm tt");
                }
                #endregion
            }
            else if (GlobalVar.account_status == "Teacher")
            {
                String timein = "";
                String currenttime = "";
                #region compare weekday
                String queryweek = "select '" + GlobalVar.timein + "' from timein where account_id = '" + GlobalVar.account_id + "' and '" + GlobalVar.weekday + "' = '" + GlobalVar.weekday + "'";
                con.Open();
                MySqlCommand cmd5 = new MySqlCommand(queryweek, con);
                cmd5.CommandTimeout = 500;
                MySqlDataReader dr6 = cmd5.ExecuteReader();
                if (dr6.Read())
                {
                    timein = (dr6[GlobalVar.timein].ToString());
                    currenttime = DateTime.Now.ToString("HH");
                    GlobalVar.timein = Int32.Parse(timein);
                    GlobalVar.currenttime = Int32.Parse(currenttime);
                }
                dr6.Close();
                con.Close();
                #endregion
                FindImageTeacher();
                #region query first
                String queryteacher = "select rfid.account_id, rfid.account_status, concat(teacher.first_name, teacher.middle_name, teacher.last_name) as full_name, teacher.track, teacher.strand, teacher.employee_number, teacher.position, teacher.ancillary_assignment, teacher.contactno from rfid inner join teacher on rfid.account_id = teacher.account_id where rfid_code = " + textBox1.Text + "";
                MySqlCommand cmd1 = new MySqlCommand(queryteacher, con);
                cmd1.CommandTimeout = 500;
                con.Open();
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    account_id = (dr1["account_id"].ToString());
                    last_name = (dr1["last_name"].ToString());
                    first_name = (dr1["first_name"].ToString());
                    middle_name = (dr1["middle_name"].ToString());
                    lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    lbllrn.Text = (dr1["employee_number"].ToString());
                    contactno = (dr1["contactno"].ToString());
                    lbltrack.Text = (dr1["track"].ToString());
                    lblstrand.Text = (dr1["strand"].ToString());
                    lblcurdate.Text = DateTime.Now.ToString("hh:mm tt");
                    lbladviser.Text = (dr1["ancillary_assignment"].ToString());
                    lblgrade.Text = (dr1["position"].ToString());
                }
                dr1.Close();
                con.Close();
                #endregion
            }
            if (statuss == null)
            {
                if (GlobalVar.account_status == "Student")
                {
                    String status = "Time Out";
                    this.ActiveControl = textBox1;
                    InsertTimeout(account_id, status, last_name, first_name, middle_name);
                    lblontime.Text = "Time Out";
                    lblontime.ForeColor = System.Drawing.Color.Red;
                }
                else if (GlobalVar.account_status == "Teacher")
                {
                    String status = "Time Out";
                    this.ActiveControl = textBox1;
                    InsertDataTeacherTimeout(account_id, status, last_name, first_name, middle_name);
                    lblontime.Text = "Time Out";
                    lblontime.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        #region Find Image
        void FindImage()
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select student.img  from rfid inner join student on rfid.account_id = student.account_id where rfid_code  = " + textBox1.Text + "";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                try
                {
                    byte[] img = (byte[])(dr["img"]);
                    MemoryStream ms = new MemoryStream(img);
                    PictureStudent.Image = Image.FromStream(ms);
                }
                catch
                {

                }
            }
            dr.Close();
            con.Close();
        }
        #endregion
        #region Find Image
        void FindImageTeacher()
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select teacher.img  from rfid inner join teacher on rfid.account_id = teacher.account_id where rfid_code  = " + textBox1.Text + "";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                try
                {
                    byte[] img = (byte[])(dr["img"]);
                    MemoryStream ms = new MemoryStream(img);
                    PictureStudent.Image = Image.FromStream(ms);
                }
                catch
                {

                }
            }
            dr.Close();
            con.Close();
        }
        #endregion
        #region Insert Data
        void InsertData(String account_id, String status, String first_name, String middle_name, String last_name)
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "insert into attendance(first_name, middle_name, last_name, account_id, number, timein, img, track, strand, account_status, status, remark, remarks) values(@first_name, @middle_name, @last_name, @account_id, @number, @timein, @img, @track, @strand,@account_status, @status, @remark, @remarks)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@first_name", first_name);
            cmd.Parameters.AddWithValue("@middle_name", middle_name);
            cmd.Parameters.AddWithValue("@last_name", last_name);
            cmd.Parameters.AddWithValue("@account_id", account_id);
            cmd.Parameters.AddWithValue("@number", lbllrn.Text);
            cmd.Parameters.AddWithValue("@timein", DateTime.Now);
            MemoryStream ms = new MemoryStream();
            PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
            byte[] a = ms.GetBuffer();
            ms.Close();
            cmd.Parameters.Add("@img", MySqlDbType.LongBlob).Value = a;
            cmd.Parameters.AddWithValue("@track", lbltrack.Text);
            cmd.Parameters.AddWithValue("@strand", lblstrand.Text);
            cmd.Parameters.AddWithValue("@account_status", "Student");
            cmd.Parameters.AddWithValue("@status", "Present");
            cmd.Parameters.AddWithValue("@remark", status);
            cmd.Parameters.AddWithValue("@remarks", "Time In");
            cmd.ExecuteNonQuery();
            con.Close();
            PresentStudent();
        }
        #endregion
        #region Insert Data Teacher
        void InsertDataTeacher(string account_id, String status, String first_name, String middle_name, String last_name)
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "insert into attendance(first_name, middle_name, last_name, account_id, number, timein, img, track, strand, remarks, account_status, status, remark) values(@first_name, @middle_name, @last_name, @account_id, @number, @timein, @img, @track, @strand, @remarks, @account_status, @status, @remark)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@first_name", first_name);
            cmd.Parameters.AddWithValue("@middle_name", middle_name);
            cmd.Parameters.AddWithValue("@last_name", last_name);
            cmd.Parameters.AddWithValue("@account_id", account_id);
            cmd.Parameters.AddWithValue("@number", lbllrn.Text);
            cmd.Parameters.AddWithValue("@timein", DateTime.Now);
            MemoryStream ms = new MemoryStream();
            PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
            byte[] a = ms.GetBuffer();
            ms.Close();
            cmd.Parameters.Add("@img", MySqlDbType.LongBlob).Value = a;
            cmd.Parameters.AddWithValue("@track", lbltrack.Text);
            cmd.Parameters.AddWithValue("@strand", lblstrand.Text);
            cmd.Parameters.AddWithValue("@remarks", "");
            cmd.Parameters.AddWithValue("@account_status", "Teacher");
            cmd.Parameters.AddWithValue("@status", "Present");
            cmd.Parameters.AddWithValue("@remark", "");
            cmd.ExecuteNonQuery();
            con.Close();
            PresentTeacher();
        }
        #endregion
        #region Insert Data
        void InsertTimeout(String account_id, String status, String first_name, String middle_name, String last_name)
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "insert into attendance(first_name, middle_name, last_name, account_id, number, timein, img, track, strand, remarks, account_status, status, remark) values(@first_name, @middle_name, @last_name, @account_id, @number, @timein, @img, @track, @strand, @remarks, @account_status, @status, @remark)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@first_name", first_name);
            cmd.Parameters.AddWithValue("@middle_name", middle_name);
            cmd.Parameters.AddWithValue("@last_name", last_name);
            cmd.Parameters.AddWithValue("@account_id", account_id);
            cmd.Parameters.AddWithValue("@number", lbllrn.Text);
            cmd.Parameters.AddWithValue("@timein", DateTime.Now);
            MemoryStream ms = new MemoryStream();
            PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
            byte[] a = ms.GetBuffer();
            ms.Close();
            cmd.Parameters.Add("@img", MySqlDbType.LongBlob).Value = a;
            cmd.Parameters.AddWithValue("@track", lbltrack.Text);
            cmd.Parameters.AddWithValue("@strand", lblstrand.Text);
            cmd.Parameters.AddWithValue("@remarks", "Time Out");
            cmd.Parameters.AddWithValue("@account_status", "Student");
            cmd.Parameters.AddWithValue("@status", "Time Out");
            cmd.Parameters.AddWithValue("@remark", status);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        #endregion
        #region Insert Data Teacher
        void InsertDataTeacherTimeout(string account_id, String status, String first_name, String middle_name, String last_name)
        {
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "insert into attendance(first_name, middle_name, last_name, account_id, number, timein, img, track, strand, remarks, account_status, status, remark) values(@first_name, @middle_name, @last_name, @account_id, @number, @timein, @img, @track, @strand, @remarks, @account_status, @status, @remark)";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@first_name", last_name);
            cmd.Parameters.AddWithValue("@middle_name", middle_name);
            cmd.Parameters.AddWithValue("@last_name", lbllastname.Text);
            cmd.Parameters.AddWithValue("@account_id", account_id);
            cmd.Parameters.AddWithValue("@number", lbllrn.Text);
            cmd.Parameters.AddWithValue("@timein", DateTime.Now);
            MemoryStream ms = new MemoryStream();
            PictureStudent.Image.Save(ms, PictureStudent.Image.RawFormat);
            byte[] a = ms.GetBuffer();
            ms.Close();
            cmd.Parameters.Add("@img", MySqlDbType.LongBlob).Value = a;
            cmd.Parameters.AddWithValue("@track", lbltrack.Text);
            cmd.Parameters.AddWithValue("@strand", lblstrand.Text);
            cmd.Parameters.AddWithValue("@remarks", "Time Out");
            cmd.Parameters.AddWithValue("@account_status", "Teacher");
            cmd.Parameters.AddWithValue("@status", "Time Out");
            cmd.Parameters.AddWithValue("@remark", "");
            cmd.ExecuteNonQuery();
            con.Close();
        }
        #endregion
        #region Retrieve Data
        void Retrieve()
        {
            String last_name = null;
            String first_name = null;
            String middle_name = null;
            flowLayoutPanel1.Controls.Clear();
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select * from attendance where timein = curdate() order by attendance_id DESC";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                viewAttendance a = new viewAttendance();
                try
                {
                    byte[] img = (byte[])(dt.Rows[i]["img"]);
                    MemoryStream ms = new MemoryStream(img);
                    a.PictureStudent.Image = Image.FromStream(ms);
                    last_name = dt.Rows[i]["last_name"].ToString();
                    first_name = dt.Rows[i]["first_name"].ToString();
                    middle_name = dt.Rows[i]["middle_name"].ToString();
                    a.lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    a.lbllrn.Text = dt.Rows[i]["number"].ToString();
                    a.lblstrand.Text = dt.Rows[i]["strand"].ToString();
                    a.lbltrack.Text = dt.Rows[i]["track"].ToString();
                    a.lblcurdate.Text = dt.Rows[i]["timein"].ToString();
                    String time = dt.Rows[i]["status"].ToString();
                    if (time == "Present")
                    {
                        a.lblontime.Text = "On Time";
                        a.lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (time == "Time Out")
                    {
                        a.lblontime.Text = "Time Out";
                        a.lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    flowLayoutPanel1.Controls.Add(a);
                }
                catch
                {
                    last_name = dt.Rows[i]["last_name"].ToString();
                    first_name = dt.Rows[i]["first_name"].ToString();
                    middle_name = dt.Rows[i]["middle_name"].ToString();
                    a.lbllastname.Text = last_name + ", " + first_name + " " + middle_name;
                    a.lbllrn.Text = dt.Rows[i]["number"].ToString();
                    a.lblstrand.Text = dt.Rows[i]["strand"].ToString();
                    a.lbltrack.Text = dt.Rows[i]["track"].ToString();
                    a.lblstatus.Text = dt.Rows[i]["remarks"].ToString();
                    a.lblcurdate.Text = dt.Rows[i]["timein"].ToString();
                    String time = dt.Rows[i]["status"].ToString();
                    if (time == "Late")
                    {
                        a.lblontime.Text = "Late";
                        a.lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (time == "Present")
                    {
                        a.lblontime.Text = "On Time";
                        a.lblontime.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (time == "Time Out")
                    {
                        a.lblontime.Text = "Time Out";
                        a.lblontime.ForeColor = System.Drawing.Color.Red;
                    }
                    flowLayoutPanel1.Controls.Add(a);
                }
            }
        }
        #endregion
        void RetrieveEvent()
        {
            flowLayoutPanel2.Controls.Clear();
            MySqlConnection con = new MySqlConnection(Connection.GetConnectionStr());
            con.Open();
            String query = "select * from event  order by event_id DESC";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                viewSchoolEvent a = new viewSchoolEvent();
                try
                {
                    byte[] img = (byte[])(dt.Rows[i]["img"]);
                    MemoryStream ms = new MemoryStream(img);
                    a.PictureStudent.Image = Image.FromStream(ms);
                    a.label3.Text = dt.Rows[i]["event_name"].ToString();
                    a.label2.Text = dt.Rows[i]["event"].ToString();
                    String Strand = dt.Rows[i]["strand"].ToString();
                    String Track = dt.Rows[i]["track"].ToString();
                    a.label4.Text = Strand + " " + Track;
                    a.label6.Text = dt.Rows[i]["date"].ToString();
                    flowLayoutPanel2.Controls.Add(a);
                }
                catch
                {
                    a.label3.Text = dt.Rows[i]["event_name"].ToString();
                    a.label2.Text = dt.Rows[i]["event"].ToString();
                    String Strand = dt.Rows[i]["strand"].ToString();
                    String Track = dt.Rows[i]["track"].ToString();
                    a.label4.Text = Strand + " " + Track;
                    a.label6.Text = dt.Rows[i]["date"].ToString();
                    flowLayoutPanel2.Controls.Add(a);
                }
            }
        }
    }
}
