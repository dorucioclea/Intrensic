using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeITLicence
{
    public enum StorageType
    {
        Local,
        Cloud,
    }

    public static class Licence
    {
        private static frmLicence frm = null;

        public static String ClientId { get; set; }
        public static DateTime? DateValid { get; set; }
        public static int? UsersLimit { get; set; }
        public static String SqlServerName { get; set; }
        public static String SqlServerDatabase { get; set; }
        public static String SqlServerUsername { get; set; }
        public static String SqlServerPassword { get; set; }
        public static StorageType StorageType { get; set; }
        public static String LocalPath { get; set; }
        public static String LocalUsername { get; set; }
        public static String LocalPassword { get; set; }
        public static String CloudId { get; set; }
        public static String CloudSecret { get; set; }

        public static readonly List<String> ValidationMessagesList = new List<string>();

        public static bool IsValid()
        {
            ValidationMessagesList.Clear();

            if (String.IsNullOrEmpty(ClientId))
            {
                ValidationMessagesList.Add("Client ID can not be empty!");
            }
            if (String.IsNullOrEmpty(SqlServerName))
            {
                ValidationMessagesList.Add("SQL Server Name can not be empty!");
            }
            if (String.IsNullOrEmpty(SqlServerDatabase))
            {
                ValidationMessagesList.Add("SQL Server Database Name can not be empty!");
            }
            if (String.IsNullOrEmpty(SqlServerUsername))
            {
                ValidationMessagesList.Add("SQL Server Username can not be empty!");
            }
            if (String.IsNullOrEmpty(SqlServerPassword))
            {
                ValidationMessagesList.Add("SQL Server Password can not be empty!");
            }
            if (StorageType == StorageType.Local)
            {
                if (String.IsNullOrEmpty(LocalPath))
                {
                    ValidationMessagesList.Add("Local Storage Path can not be empty!");
                }
            }
            else if (StorageType == StorageType.Cloud)
            {
                if (String.IsNullOrEmpty(CloudId))
                {
                    ValidationMessagesList.Add("Cloud Access Key Id can not be empty!");
                }

                if (String.IsNullOrEmpty(CloudSecret))
                {
                    ValidationMessagesList.Add("Cloud Secret Access Key can not be empty!");
                }
            }

            if (ValidationMessagesList.Count > 0)
            {
                return false;
            }

            return true;
        }

        public static String ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}|", ClientId);
            sb.AppendFormat("{0}|", DateValid);
            sb.AppendFormat("{0}|", UsersLimit);
            sb.AppendFormat("{0}|", SqlServerName);
            sb.AppendFormat("{0}|", SqlServerDatabase);
            sb.AppendFormat("{0}|", SqlServerUsername);
            sb.AppendFormat("{0}|", SqlServerPassword);
            sb.AppendFormat("{0}|", (int)StorageType);
            sb.AppendFormat("{0}|", LocalPath);
            sb.AppendFormat("{0}|", LocalUsername);
            sb.AppendFormat("{0}|", LocalPassword);
            sb.AppendFormat("{0}|", CloudId);
            sb.AppendFormat("{0}|", CloudSecret);
            return sb.ToString();
        }

        public static String GenerateKey()
        {
            String licenceStr = ToString();
            return licenceStr.Encrypt();
        }

        public static void ParseFromString(String key)
        {
            String[] item = null;

            ValidationMessagesList.Clear();

            String decripted = String.Empty;

            try
            {
                decripted = key.Decript();
            }
            catch (Exception e)
            {
                ValidationMessagesList.Add("Licence Key is not valid.");
                return;
            }

            if (String.IsNullOrEmpty(decripted))
            {
                ValidationMessagesList.Add("Licence Key is not valid.");
                return;
            }

            String[] items = decripted.Split('|');
            if (items.Length != 14)
            {
                ValidationMessagesList.Add("Licence Key is not valid.");
                ValidationMessagesList.Add("Wrong Number of items.");
                return;
            }

            try
            {
                ClientId = items[0];
                DateValid = String.IsNullOrEmpty(items[1]) ? null : (DateTime?)DateTime.Parse(items[1]);
                UsersLimit = String.IsNullOrEmpty(items[2]) ? null : (Int32?)Int32.Parse(items[2]);
                SqlServerName = items[3];
                SqlServerDatabase = items[4];
                SqlServerUsername = items[5];
                SqlServerPassword = items[6];
                StorageType = (StorageType) int.Parse(items[7]);
                LocalPath = items[8];
                LocalUsername = items[9];
                LocalPassword = items[10];
                CloudId = items[11];
                CloudSecret = items[12];
            }
            catch (Exception e)
            {
                ValidationMessagesList.Add("Licence Key is not valid.");
            }
        }

        public static void ParseFromFile(String filePath)
        {
            ValidationMessagesList.Clear();

            if (!File.Exists(filePath))
            {
                ValidationMessagesList.Add("Licence Key is not found.");
                return;
            }

            String key = File.ReadAllText(filePath, Encoding.UTF8);
            ParseFromString(key);
        }

		public static void ParseFromDB(string clientlicense)
		{
			ValidationMessagesList.Clear();
			
			if (String.IsNullOrEmpty(clientlicense))
			{
				ValidationMessagesList.Add("Licence Key is not found.");
				return;
			}

			//String key = File.ReadAllText(filePath, Encoding.UTF8);
			ParseFromString(clientlicense);
		}

        public static DialogResult ValidateLicence(String filePath)
        {
            ParseFromFile(filePath);
            if (ValidationMessagesList.Count > 0)
            {
                return ShowDialog();
            }

            if (!IsValid())
            {
                return ShowDialog();
            }

            return DialogResult.OK;
        }

		public static bool ValidateLicenceFromDB(String clientLicense)
		{
			ParseFromDB(clientLicense);
			if (ValidationMessagesList.Count > 0)
			{
				//MessageBox.Show("You don't have license. Please contact administrator.","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return false;
			}

			if (!IsValid())
			{
				//MessageBox.Show("You don't have license. Please contact administrator.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			return true;
		}

        private static DialogResult ShowDialog()
        {
            if (frm == null)
            {
                frm = new frmLicence();
            }

            var proc = Process.GetCurrentProcess();
            IWin32Window w = Control.FromHandle(proc.MainWindowHandle);
            return frm.ShowDialog(w);
        }

        public static String GetConnectionString()
        {
            var sb = new StringBuilder();
            sb.Append("metadata=res://*/IntrensicDB.csdl|res://*/IntrensicDB.ssdl|res://*/IntrensicDB.msl;provider=System.Data.SqlClient;provider connection string=';");
            sb.AppendFormat("data source={0};", SqlServerName);
            sb.AppendFormat("initial catalog={0};", SqlServerDatabase);
            sb.AppendFormat("User Id={0};", SqlServerUsername);
            sb.AppendFormat("password={0};", SqlServerPassword);
            sb.Append("integrated security=False;MultipleActiveResultSets=True;App=EntityFramework'"); 
            return sb.ToString();
        }
    }
}
