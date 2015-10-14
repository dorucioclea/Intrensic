using System.Runtime.InteropServices;

namespace CodeITBL
{

	[StructLayout(LayoutKind.Sequential)]
	public struct structNetResource
	{
		public int iScope;
		public int iType;
		public int iDisplayType;
		public int iUsage;
		public string sLocalName;
		public string sRemoteName;
		public string sComment;
		public string sProvider;
	}

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

	// Network error codes
	public enum NetworkError
	{
		EROOR_SUCCESS = 0,
		ERROR_BAD_NETPATH = 53,
		SPECIFIED_LOGON_SESSION_DOES_NOT_EXIST = 1312,
		SPECIFIED_RESOURCE_IS_NOT_AVAILABLE = 55,
		MULTIPLE_CONNECTIONS_TO_A_SERVER_OR_SHARED_RESOURCE_BY_THE_SAME_USER = 1219
	};

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
	public enum ResurceUsage : int
	{
		Connectable = 1,
		Container = 2,
		NoLocalDevice = 4,
		Sibling = 8,
		Attached = 10
	}
}
