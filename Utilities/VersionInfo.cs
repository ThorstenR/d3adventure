using System;
using System.Runtime.InteropServices;

namespace Utilities.OperatingSystemVersionInfo
{
    public class WindowsVersionInfo
    {
        public string GetOSVersion
        {
            get { return this.GetOSVersionInfo(); }
        }

        public string GetServicePack
        {
            get { return this.GetServicePackInfo(); }
        }

        public string GetProductType
        {
            get { return this.GetProductTypeInfo(); }
        }

        private string GetOSVersionInfo()
        {
            string version = "Unsupported Version";

            NativeMethods.OSVersionInfoEx osvi = new NativeMethods.OSVersionInfoEx();
            osvi.VersionInfoSize =
              Marshal.SizeOf(typeof(NativeMethods.OSVersionInfoEx));
            NativeMethods.GetVersionEx(ref osvi);

            if (OsviConstant.SupportedPlatform == osvi.PlatformId &&
              osvi.MajorVersion > 4)
            {
                if (osvi.MajorVersion == (int)OsviConstant.MajorVersion.NT5 &&
                  osvi.MinorVersion == (int)OsviConstant.MinorVersion.Windows2000)
                {
                    version = "Windows 2000";
                }

                if (osvi.MajorVersion == (int)OsviConstant.MajorVersion.NT5 &&
                  osvi.MinorVersion == (int)OsviConstant.MinorVersion.WindowsXP)
                {
                    version = "Windows XP";
                }

                if (osvi.MajorVersion == (int)OsviConstant.MajorVersion.NT5 &&
                  osvi.MinorVersion == (int)OsviConstant.MinorVersion.WindowsServer2003)
                {
                    if (osvi.ProductType == (byte)OsviConstant.WorkStation)
                    {
                        version = "Windows XP Professional x64";
                    }
                    else
                    {
                        version = "Windows Server 2003";
                        if (NativeMethods.GetSystemMetrics(OsviConstant.ServerR2) != 0)
                        {
                            version += " R2";
                        }
                    }
                }

                if (osvi.MajorVersion == (int)OsviConstant.MajorVersion.NT6 &&
                  osvi.MinorVersion == (int)OsviConstant.MinorVersion.WindowsVista)
                {
                    if (osvi.ProductType ==
                      (byte)OsviConstant.WorkStation)
                    {
                        version = "Windows Vista";
                    }
                    else
                    {
                        version = "Windows Server 2008";
                    }
                }

                if (osvi.MajorVersion == (int)OsviConstant.MajorVersion.NT6 &&
                  osvi.MinorVersion == (int)OsviConstant.MinorVersion.Windows7)
                {
                    if (osvi.ProductType == (byte)OsviConstant.WorkStation)
                    {
                        version = "Windows 7";
                    }
                    else
                    {
                        version = "Windows Server 2008 R2";
                    }
                }
            }

            return version;
        }

        private string GetServicePackInfo()
        {
            NativeMethods.OSVersionInfoEx versionInfo = new NativeMethods.OSVersionInfoEx();
            versionInfo.VersionInfoSize = Marshal.SizeOf(typeof(NativeMethods.OSVersionInfoEx));
            NativeMethods.GetVersionEx(ref versionInfo);
            return versionInfo.CSDVersion;
        }

        private string GetProductTypeInfo()
        {
            string product = String.Empty;

            NativeMethods.OSVersionInfoEx osvi = new NativeMethods.OSVersionInfoEx();
            osvi.VersionInfoSize =
              Marshal.SizeOf(typeof(NativeMethods.OSVersionInfoEx));
            NativeMethods.GetVersionEx(ref osvi);

            if (osvi.MajorVersion > 5)
            {
                uint productType = 0;

                NativeMethods.GetProductInfo(
                  osvi.MajorVersion,
                  osvi.MinorVersion,
                  osvi.ServicePackMajor,
                  osvi.ServicePackMinor,
                  ref productType);

                switch (productType)
                {
                    case (uint)OsviConstant.ProductInfo.Business:
                        product = "Business Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.BusinessN:
                        product = "Business N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.ClusterServer:
                        product = "HPC Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.DatacenterServer:
                        product = "Server Datacenter (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.DatacenterServerCore:
                        product = "Server Datacenter (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.DataCenterServerCoreV:
                        product = "Server Datacenter without Hyper-V (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.DataCenterServerV:
                        product = "Server Datacenter without Hyper-V (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.Enterprise:
                        product = "Enterprise Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseE:
                        product = "Enterprise E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseN:
                        product = "Enterprise N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseServer:
                        product = "Server Enterprise (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseServerCore:
                        product = "Server Enterprise (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseServerCoreV:
                        product = "Server Enterprise without Hyper-V (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseServerIA64:
                        product = "Server Enterprise for Itanium-based Systems";
                        break;
                    case (uint)OsviConstant.ProductInfo.EnterpriseServerV:
                        product = "Server Enterprise without Hyper-V (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomeBasic:
                        product = "Home Basic Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomeBasicE:
                        product = "Home Basic E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomeBasicN:
                        product = "Home Basic N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomePremium:
                        product = "Home Premium Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomePremiumE:
                        product = "Home Premium E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomePremiumN:
                        product = "Home Premium N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HomeServer:
                        product = "Home Server Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.HyperV:
                        product = "Microsoft Hyper-V Server";
                        break;
                    case (uint)OsviConstant.ProductInfo.MediumBusinessServerManagement:
                        product = "Windows Essential Business Server Management Server";
                        break;
                    case (uint)OsviConstant.ProductInfo.MediumBusinessServerMessaging:
                        product = "Windows Essential Business Server Messaging Server";
                        break;
                    case (uint)OsviConstant.ProductInfo.MediumBusinessServerSecurity:
                        product = "Windows Essential Business Server Security Server";
                        break;
                    case (uint)OsviConstant.ProductInfo.Professional:
                        product = "Professional Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.ProfessionalE:
                        product = "Professional E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.ProfessionalN:
                        product = "Professional N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.ServerForSmallBusiness:
                        product = "Windows Server 2008 for Windows Essential Server Solutions";
                        break;
                    case (uint)OsviConstant.ProductInfo.ServerForSmallBusinessV:
                        product = "Windows Server 2008 without Hyper-V for Windows Essential Server Solutions";
                        break;
                    case (uint)OsviConstant.ProductInfo.ServerFoundation:
                        product = "Server Foundation";
                        break;
                    case (uint)OsviConstant.ProductInfo.SmallBusinessServer:
                        product = "Windows Small Business Server";
                        break;
                    case (uint)OsviConstant.ProductInfo.SmallBusinessServerPremium:
                        product = "Windows Small Busines Server Premium";
                        break;
                    case (uint)OsviConstant.ProductInfo.StandardServer:
                        product = "Server Standard (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.StandardServerCore:
                        product = "Server Standard (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.StandardServerCoreV:
                        product = "Server Standard without Hyper-V (Core)";
                        break;
                    case (uint)OsviConstant.ProductInfo.StandardServerV:
                        product = "Server Standard without Hyper-V (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.Starter:
                        product = "Starter Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.StarterE:
                        product = "Starter E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.StarterN:
                        product = "Starter N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.StorageEnterpriseServer:
                        product = "Storage Server Enterprise";
                        break;
                    case (uint)OsviConstant.ProductInfo.StorageExpressServer:
                        product = "Storage Server Express";
                        break;
                    case (uint)OsviConstant.ProductInfo.StorageStandardServer:
                        product = "Storage Server Standard";
                        break;
                    case (uint)OsviConstant.ProductInfo.StorageWorkgroupServer:
                        product = "Storage Server Workgroup";
                        break;
                    case (uint)OsviConstant.ProductInfo.Ultimate:
                        product = "Ultimate Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.UltimateE:
                        product = "Ultimate E Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.UltimateN:
                        product = "Ulitmate N Edition";
                        break;
                    case (uint)OsviConstant.ProductInfo.Undefined:
                        product = "Unknown Product";
                        break;
                    case (uint)OsviConstant.ProductInfo.Unlicensed:
                        product = "Unlicensed or Expired";
                        break;
                    case (uint)OsviConstant.ProductInfo.WebServer:
                        product = "Web Server (Full)";
                        break;
                    case (uint)OsviConstant.ProductInfo.WebServerCore:
                        product = "Web Server (Core)";
                        break;
                }
            }

            return product;
        }
    }

    // ****************************************************************************
    // NEW CLASS - SHOULD BE PLACED IN SEPARATE FILE
    // ****************************************************************************

    internal class OsviConstant
    {
        internal const int SupportedPlatform = 2;
        internal const int ServerR2 = 89;
        internal const int WorkStation = 0x00000001;

        private OsviConstant()
        {
        }

        internal enum MajorVersion
        {
            NT5 = 5,
            NT6 = 6
        }

        internal enum MinorVersion
        {
            Windows2000 = 0,
            WindowsXP = 1,
            WindowsServer2003 = 2,
            WindowsVista = 0,
            Windows7 = 1
        }

        internal enum ProductInfo : uint
        {
            Business = 0x00000006,
            BusinessN = 0x00000010,
            ClusterServer = 0x00000012,
            DatacenterServer = 0x00000008,
            DatacenterServerCore = 0x0000000C,
            DataCenterServerCoreV = 0x00000027,
            DataCenterServerV = 0x00000025,
            Enterprise = 0x00000004,
            EnterpriseE = 0x00000046,
            EnterpriseN = 0x0000001B,
            EnterpriseServer = 0x0000000A,
            EnterpriseServerCore = 0x0000000E,
            EnterpriseServerCoreV = 0x00000029,
            EnterpriseServerIA64 = 0x0000000F,
            EnterpriseServerV = 0x00000026,
            HomeBasic = 0x00000002,
            HomeBasicE = 0x00000043,
            HomeBasicN = 0x00000005,
            HomePremium = 0x00000003,
            HomePremiumE = 0x00000044,
            HomePremiumN = 0x0000001A,
            HyperV = 0x0000002A,
            MediumBusinessServerManagement = 0x0000001E,
            MediumBusinessServerSecurity = 0x0000001F,
            MediumBusinessServerMessaging = 0x00000020,
            Professional = 0x00000030,
            ProfessionalE = 0x00000045,
            ProfessionalN = 0x00000031,
            ServerForSmallBusiness = 0x00000018,
            ServerForSmallBusinessV = 0x00000023,
            ServerFoundation = 0x00000021,
            SmallBusinessServer = 0x00000009,
            StandardServer = 0x00000007,
            StandardServerCore = 0x0000000D,
            StandardServerCoreV = 0x00000028,
            StandardServerV = 0x00000024,
            Starter = 0x0000000B,
            StarterE = 0x00000042,
            StarterN = 0x0000002F,
            StorageEnterpriseServer = 0x00000017,
            StorageExpressServer = 0x00000014,
            StorageStandardServer = 0x00000015,
            StorageWorkgroupServer = 0x00000016,
            Undefined = 0x00000000,
            Ultimate = 0x00000001,
            UltimateE = 0x00000047,
            UltimateN = 0x0000001C,
            WebServer = 0x00000011,
            WebServerCore = 0x0000001D,
            Unlicensed = 0xABCDABCD,
            HomeServer = 0x00000013,
            SmallBusinessServerPremium = 0x00000019,
        }
    }

    // ****************************************************************************
    // NEW CLASS - SHOULD BE PLACED IN SEPARATE FILE
    // ****************************************************************************

    internal class NativeMethods
    {
        private NativeMethods()
        {
        }

        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetVersionEx(ref OSVersionInfoEx osvi);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetProductInfo(
          int osMajorVersion,
          int osMinorVersion,
          int spMajorVersion,
          int spMinorVersion,
          ref uint type);

        [DllImport("kernel32.dll")]
        internal static extern int GetSystemMetrics(
          int index);

        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVersionInfoEx
        {
            public int VersionInfoSize;
            public int MajorVersion;
            public int MinorVersion;
            public int BuildNumber;
            public int PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string CSDVersion;
            public Int16 ServicePackMajor;
            public Int16 ServicePackMinor;
            public Int16 SuiteMask;
            public byte ProductType;
            public byte Reserved;
        }
    }
}