namespace DocScanner.LibCommon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;

    //	using Tigera.LibBasic.Network.Tcp;

    public static class NetHelper
    {
        // Methods
        public static void DownLoadFileFromUrl(string url, string saveFilePath)
        {
            FileStream stream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write);
            WebRequest request = WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                int contentLength = (int)response.ContentLength;
                byte[] buffer = new byte[0x400];
                int count = 0;
                int num3 = 0;
                bool flag = false;
                while (!flag)
                {
                    count = response.GetResponseStream().Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        stream.Write(buffer, 0, count);
                        num3 += count;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                stream.Flush();
            }
            finally
            {
                stream.Close();
                request = null;
            }
        }

        public static string GetFirstMacAddress()
        {
            IList<string> macAddress = GetMacAddress();
            if ((macAddress != null) && (macAddress.Count > 0))
            {
                return macAddress[0];
            }
            return "";
        }

        public static string GetHostIP4Address()
        {
            string str = string.Empty;
            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    return address.ToString();
                }
            }
            return str;
        }

        public static IPAddress[] GetLocalIp()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }

        public static string GetLocalPublicIp()
        {
            IPAddress[] localIp = GetLocalIp();
            foreach (IPAddress address in localIp)
            {
                if (IsPublicIPAddress(address.ToString()))
                {
                    return address.ToString();
                }
            }
            return null;
        }

        public static IList<string> GetMacAddress()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            IList<string> list = new List<string>();
            foreach (ManagementObject obj2 in instances)
            {
                if ((bool)obj2["IPEnabled"])
                {
                    list.Add(obj2["MacAddress"].ToString().Replace(":", ""));
                }
                obj2.Dispose();
            }
            return list;
        }

        public static object GetRemotingHanler(string channelTypeStr, string ip, int port, string remotingServiceName, Type destInterfaceType)
        {
            try
            {
                object[] args = new object[] { channelTypeStr, ip, port, remotingServiceName };
                string url = string.Format("{0}://{1}:{2}/{3}", args);
                return Activator.GetObject(destInterfaceType, url);
            }
            catch
            {
                return null;
            }
        }

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int description = 0;
            return InternetGetConnectedState(description, 0);
        }

        public static bool IsPublicIPAddress(string ip)
        {
            if (ip.StartsWith("10."))
            {
                return false;
            }
            if (ip.StartsWith("172.") && (ip.Substring(6, 1) == "."))
            {
                int num = int.Parse(ip.Substring(4, 2));
                if ((0x10 <= num) && (num <= 0x1f))
                {
                    return false;
                }
            }
            if (ip.StartsWith("192.168."))
            {
                return false;
            }
            return true;
        }

        public static byte[] ReceiveData(NetworkStream stream, int size)
        {
            byte[] buff = new byte[size];
            ReceiveData(stream, buff, 0, size);
            return buff;
        }

        public static void ReceiveData(NetworkStream stream, byte[] buff, int offset, int size)
        {
            int num = 0;
            int num2 = 0;
            int num3 = offset;
            while (num2 < size)
            {
                int count = size - num2;
                num = stream.Read(buff, num3, count);
                if (num == 0)
                {
                    throw new IOException("NetworkStream Interruptted !");
                }
                num3 += num;
                num2 += num;
            }
        }
    }

}