using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace CodeITBL
{
    /// <summary>
    /// Opens a connection to a network resource. Automatically closes 
    /// the connection when disposed.
    /// </summary>
    public class NetworkConnection : IDisposable
{
    string _networkName;

	private readonly ILog _logger = LogManager.GetLogger(typeof(NetworkConnection));
    public NetworkConnection(string networkName, 
        NetworkCredential credentials)
    {
        _networkName = networkName;
				
		//string driveLetter = GetNextDriveLetter();
		//_logger.Info("drive letter for net path: " + driveLetter);
        var userName = string.IsNullOrEmpty(credentials.Domain)
            ? credentials.UserName
            : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

		NetworkDrive oNetDrive = new NetworkDrive();
		
		try
		{
			//set propertys
			oNetDrive.Force = false;
			oNetDrive.Persistent = true;
			//oNetDrive.LocalDrive = driveLetter;
			oNetDrive.PromptForCredentials = false;
			oNetDrive.ShareName = networkName;
			oNetDrive.SaveCredentials = false;
			//match call to options provided

			_logger.Info("Unmap network path");
			oNetDrive.UnMapDrive();
			_logger.Info("Before Drive Mapping");
			oNetDrive.MapDrive(userName, credentials.Password);
			_logger.Info("After Drive Mapping");	
			//update status
			
		}
		catch (Exception err)
		{
			//report error
			_logger.Error("Error occure while map net path", err);
			
		}
		oNetDrive = null;
    }

	public static string GetNextDriveLetter()
	{
		List<string> InUse = new List<string>();

		foreach (DriveInfo drive in DriveInfo.GetDrives())
		{
			InUse.Add(drive.Name.Substring(0, 1).ToUpper());
		}
		// Skip the first 4 letters.
		char[] alphas = "EFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

		foreach (char alpha in alphas)
		{
			if (!InUse.Contains(alpha.ToString()))
			{
				return alpha.ToString();
			}
		}
		return string.Empty;
	}

    ~NetworkConnection()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        WNetCancelConnection2(_networkName, 0, true);
    }

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource, 
        string password, string username, int flags);

	[DllImport("mpr.dll")]
	private static extern int WNetAddConnection2A(ref structNetResource pstNetRes, string psPassword, string psUsername, int piFlags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags,
        bool force);
}
}
