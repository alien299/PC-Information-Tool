using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics.Contracts;
using System.Net;
using System.Diagnostics;

namespace Alien_PC_Information_Grabber
{
	class Program
	{
		static void Main(string[] args)
		{
			StartProgram();
		}

		static void SetText()
        {
			Console.Title = "Alien Hardware Information Tool";
			Console.CursorVisible = false;
			Console.SetWindowSize(52, 25);
			Console.BufferWidth = 52;
			Console.BufferHeight = 25;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(@"
 _______  _       _________ _______  _       
(  ___  )( \      \__   __/(  ____ \( (    /|
| (   ) || (         ) (   | (    \/|  \  ( |
| (___) || |         | |   | (__    |   \ | |
|  ___  || |         | |   |  __)   | (\ \) |
| (   ) || |         | |   | (      | | \   |
| )   ( || (____/\___) (___| (____/\| )  \  |
|/     \|(_______/\_______/(_______/|/    )_)");
			Console.WriteLine();

			Console.WriteLine("Website: https://patriknagy.hu");
			Console.WriteLine("----------------------------------------------");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("[1] ID-s");
			Console.WriteLine("[2] Video Card (GPU) Information");
			Console.WriteLine("[3] Processor (CPU) Information");
			Console.WriteLine("[4] RAM Information");
			Console.WriteLine("[5] Motherboard Information");
			Console.WriteLine("[6] System and Disk(s) Information");
			Console.WriteLine("[7] BIOS Information");
			Console.WriteLine("-------------------------------------");
			Console.WriteLine("[8] Open Website in default browser");
			Console.WriteLine("[9] Exit");
		}

		static void Title(int windowX, int windowY, int bufferWidth, int bufferHeight, bool changed)
        {
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
            if (changed)
            {
				Console.SetWindowSize(windowX, windowY);
				Console.BufferHeight = bufferHeight;
				Console.BufferWidth = bufferWidth;
			}
			Console.WriteLine(@"
 _______  _       _________ _______  _       
(  ___  )( \      \__   __/(  ____ \( (    /|
| (   ) || (         ) (   | (    \/|  \  ( |
| (___) || |         | |   | (__    |   \ | |
|  ___  || |         | |   |  __)   | (\ \) |
| (   ) || |         | |   | (      | | \   |
| )   ( || (____/\___) (___| (____/\| )  \  |
|/     \|(_______/\_______/(_______/|/    )_)");
			Console.WriteLine();

			Console.WriteLine("Website: https://patriknagy.hu");
			Console.WriteLine("----------------------------------------------");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
		}

		static void EndText()
        {
			Console.WriteLine("\nPress [Enter] to return to the Main Menu");
			Console.ReadLine();
			Console.Clear();
			StartProgram();
		}

		static void StartProgram()
		{
			SetText();
			GetWriteInput();
		}

		static void GetWriteInput()
		{
			int inputNumber = Convert.ToInt32(Console.ReadLine());
			if (inputNumber == 1)
			{
				IDs();
			}
			else if (inputNumber == 2)
			{
				GPU();	
			}
			else if (inputNumber == 3)
			{
				CPU();
			}
			else if (inputNumber == 4)
			{
				RAM();
			}
			else if (inputNumber == 5)
			{
				MotherBoard();
			}
			else if(inputNumber == 6)
			{
				DiskAndSystem();
			}
			else if (inputNumber == 7)
			{
				BIOS();
			}
			else if(inputNumber == 8)
			{
				Website();
			}
			else if(inputNumber == 9)
			{
				ExitApp();
			}
			else
			{
				InvalidInput();
			}
			Console.ReadKey();
		}

		static void IDs()
		{
			Title(Console.WindowWidth, Console.WindowHeight, Console.BufferWidth, Console.BufferHeight, false);

			ManagementObjectCollection mbsList = null;
			ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
			mbsList = mbs.Get();
			string cpuid = "";
			foreach (ManagementObject mo in mbsList)
			{
				cpuid = mo["ProcessorID"].ToString();
			}
			Console.WriteLine($"CPU ID: {cpuid}");

			ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
			dsk.Get();
			string dskid = dsk["VolumeSerialNumber"].ToString();

			Console.WriteLine($"Hard Disk Serial Number: {dskid}");

			ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
			ManagementObjectCollection moc = mos.Get();
			string serial = "";
			foreach (ManagementObject mo in moc)
			{
				serial = (string)mo["SerialNumber"];
			}

			Console.WriteLine($"Motherboard Model Number: {serial}");

			string HWID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
			Console.WriteLine($"HWID: {HWID}");

			EndText();
		}

		static void GPU()
		{
			Title(52, 40, Console.BufferWidth, 300, true);

			using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
			{
				foreach (ManagementObject gpu in searcher.Get())
				{
					string VideoArchitecture;

					switch (Convert.ToInt32(gpu["VideoArchitecture"]))
					{
						case 1:
							VideoArchitecture = "Other";
							break;

						case 2:
							VideoArchitecture = "Unknown";
							break;
						case 3:
							VideoArchitecture = "CGA";
							break;
						case 4:
							VideoArchitecture = "EGA";
							break;
						case 5:
							VideoArchitecture = "VGA";
							break;
						case 6:
							VideoArchitecture = "SVGA";
							break;
						case 7:
							VideoArchitecture = "MDA";
							break;
						case 8:
							VideoArchitecture = "HGC";
							break;
						case 9:
							VideoArchitecture = "MCGA";
							break;
						case 10:
							VideoArchitecture = "8514A";
							break;
						case 11:
							VideoArchitecture = "XGA";
							break;
						case 12:
							VideoArchitecture = "Linear Frame Buffer";
							break;
						case 160:
							VideoArchitecture = "PC-98";
							break;
						default:
							VideoArchitecture = "Unknown";
							break;
					}

					string VideoMemoryType;
					switch (Convert.ToInt32(gpu["VideoMemoryType"]))
					{
						case 1:
							VideoMemoryType = "Other";
							break;
						case 2:
							VideoMemoryType = "Unknown";
							break;
						case 3:
							VideoMemoryType = "VRAM";
							break;
						case 4:
							VideoMemoryType = "DRAM";
							break;
						case 5:
							VideoMemoryType = "SRAM";
							break;
						case 6:
							VideoMemoryType = "WRAM";
							break;
						case 7:
							VideoMemoryType = "EDO RAM";
							break;
						case 8:
							VideoMemoryType = "Burst Synchronous DRAM";
							break;
						case 9:
							VideoMemoryType = "Pipelined Burst SRAM";
							break;
						case 10:
							VideoMemoryType = "CDRAM";
							break;
						case 11:
							VideoMemoryType = "3DRAM";
							break;
						case 12:
							VideoMemoryType = "SDRAM";
							break;
						case 13:
							VideoMemoryType = "SGRAM";
							break;
						default:
							VideoMemoryType = "Unknown";
							break;
					}



					Console.WriteLine("Accelerator Capabilities: " + gpu["AcceleratorCapabilities"]);
					Console.WriteLine("Adapter Compatibility: " + gpu["AdapterCompatibility"]);
					Console.WriteLine("Adapter DAC Type: " + gpu["AdapterDACType"]);
					Console.WriteLine("Adapter RAM: " + gpu["AdapterRAM"]);
					Console.WriteLine("Availability: " + gpu["Availability"]);
					Console.WriteLine("Capability Descriptions: " + gpu["CapabilityDescriptions"]);
					Console.WriteLine("Caption: " + gpu["Caption"]);
					Console.WriteLine("Color Table Entries: " + gpu["ColorTableEntries"]);
					Console.WriteLine("Configuration Manager Error Code: " + gpu["ConfigManagerErrorCode"]);
					Console.WriteLine("Configuration Manager User Configuration: " + gpu["ConfigManagerUserConfig"]);
					Console.WriteLine("Creation Class Name: " + gpu["CreationClassName"]);
					Console.WriteLine("Current Bits Per Pixel: " + gpu["CurrentBitsPerPixel"]);
					Console.WriteLine("Current Horizontal Resolution: " + gpu["CurrentHorizontalResolution"]);
					Console.WriteLine("Current Number Of Colors: " + gpu["CurrentNumberOfColors"]);
					Console.WriteLine("Current Number Of Columns: " + gpu["CurrentNumberOfColumns"]);
					Console.WriteLine("Current Number Of Rows: " + gpu["CurrentNumberOfRows"]);
					Console.WriteLine("Current Refresh Rate: " + gpu["CurrentRefreshRate"]);
					Console.WriteLine("Current Scan Mode: " + gpu["CurrentScanMode"]);
					Console.WriteLine("Current Vertical Resolution: " + gpu["CurrentVerticalResolution"]);
					Console.WriteLine("Description: " + gpu["Description"]);
					Console.WriteLine("Device ID: " + gpu["DeviceID"]);
					Console.WriteLine("Device Specific Pens: " + gpu["DeviceSpecificPens"]);
					Console.WriteLine("Dither Type: " + gpu["DitherType"]);
					Console.WriteLine("Driver Date: " + gpu["DriverDate"]);
					Console.WriteLine("Driver Version: " + gpu["DriverVersion"]);
					Console.WriteLine("Error Cleared: " + gpu["ErrorCleared"]);
					Console.WriteLine("Error Description: " + gpu["ErrorDescription"]);
					Console.WriteLine("ICM Intent: " + gpu["ICMIntent"]);
					Console.WriteLine("ICM Method: " + gpu["ICMMethod"]);
					Console.WriteLine("Inf File Name: " + gpu["InfFilename"]);
					Console.WriteLine("Inf Section: " + gpu["InfSection"]);
					Console.WriteLine("Installation Date: " + gpu["InstallDate"]);
					Console.WriteLine("Installed Display Drivers: " + gpu["InstalledDisplayDrivers"]);
					Console.WriteLine("Last Error Code: " + gpu["LastErrorCode"]);
					Console.WriteLine("Maximum Memory Supported: " + gpu["MaxMemorySupported"]);
					Console.WriteLine("Maximum Number Controlled: " + gpu["MaxNumberControlled"]);
					Console.WriteLine("Maximum Refresh Rate: " + gpu["MaxRefreshRate"]);
					Console.WriteLine("Minimum Refresh Rate: " + gpu["MinRefreshRate"]);
					Console.WriteLine("Monochrome: " + gpu["Monochrome"]);
					Console.WriteLine("Name: " + gpu["Name"]);
					Console.WriteLine("Number Of Color Planes: " + gpu["NumberOfColorPlanes"]);
					Console.WriteLine("Number Of Video Pages: " + gpu["NumberOfVideoPages"]);
					Console.WriteLine("PNP Device ID: " + gpu["PNPDeviceID"]);
					Console.WriteLine("Power Management Capabilities: " + gpu["PowerManagementCapabilities"]);
					Console.WriteLine("Power Management Supported: " + gpu["PowerManagementSupported"]);
					Console.WriteLine("Protocol Supported: " + gpu["ProtocolSupported"]);
					Console.WriteLine("Reserved System Palette Entries: " + gpu["ReservedSystemPaletteEntries"]);
					Console.WriteLine("Specification Version: " + gpu["SpecificationVersion"]);
					Console.WriteLine("Status: " + gpu["Status"]);
					Console.WriteLine("Status Information: " + gpu["StatusInfo"]);
					Console.WriteLine("System Creation Class Name: " + gpu["SystemCreationClassName"]);
					Console.WriteLine("System Name: " + gpu["SystemName"]);
					Console.WriteLine("System Palette Entries: " + gpu["SystemPaletteEntries"]);
					Console.WriteLine("Time Of Last Reset: " + gpu["TimeOfLastReset"]);
					Console.WriteLine("Video Architecture: " + VideoArchitecture);
					Console.WriteLine("Video Memory Type: " + VideoMemoryType);
					if (gpu["VideoMode"] != null && gpu["VideoMode"].ToString() != "")
					{
						Console.WriteLine("Video Mode: " + gpu["VideoMode"]);
					}
					Console.WriteLine("Video Mode Description: " + gpu["VideoModeDescription"]);
					Console.WriteLine("Video Processor: " + gpu["VideoProcessor"]);
				}
			}
			EndText();
		}

		static void CPU()
		{
			Title(52, 30, 52, 30, true);

			using (var searcher = new ManagementObjectSearcher("select * from Win32_Processor"))
			{
				foreach (ManagementObject cpu in searcher.Get())
				{
					Console.WriteLine("Name: " + (string)cpu["Name"]);
					Console.WriteLine("ID: " + (string)cpu["ProcessorId"]);
					Console.WriteLine("Socket: " + (string)cpu["SocketDesignation"]);
					Console.WriteLine("Description: " + (string)cpu["Caption"]);
					Console.WriteLine("AddressWidth: " + (ushort)cpu["AddressWidth"]);
					Console.WriteLine("DataWidth: " + (ushort)cpu["DataWidth"]);
					Console.WriteLine("Architecture: " + (ushort)cpu["Architecture"]);
					Console.WriteLine("SpeedMHz: " + (uint)cpu["MaxClockSpeed"]);
					Console.WriteLine("BusSpeedMHz: " + (uint)cpu["ExtClock"]);
					Console.WriteLine("L2Cache: " + (uint)cpu["L2CacheSize"] * (ulong)1024);
					Console.WriteLine("L3Cache: " + (uint)cpu["L3CacheSize"] * (ulong)1024);
					Console.WriteLine("Cores: " + (uint)cpu["NumberOfCores"]);
					Console.WriteLine("Threads: " + (uint)cpu["NumberOfLogicalProcessors"]);
				}
			}
			EndText();
		}

		static void RAM()
		{
			Title(52, 53, 52, 53, true);

			//ManagementObjectSearcher search = new ManagementObjectSearcher("Select * From Win32_PhysicalMemory");

			//foreach (ManagementObject ram in search.Get())
			//{
			//	Console.WriteLine("\r\nRAM: " + Convert.ToDouble(ram.GetPropertyValue("Capacity")) / 1073741824 + "GB");
			//}

			//Console.WriteLine();
			//Console.Write("In total:");

			//UInt64 total = 0;
			//foreach (ManagementObject ram in search.Get())
			//{
			//	total += (UInt64)ram.GetPropertyValue("Capacity");
			//}

			//Console.WriteLine("\r\nRAM: " + total / 1073741824 + "GB");

			ConnectionOptions connection = new ConnectionOptions();
			connection.Impersonation = ImpersonationLevel.Impersonate;
			ManagementScope scope = new ManagementScope("\\\\.\\root\\CIMV2", connection);
			scope.Connect();

			//UInt64 total = 0;
			//foreach (ManagementObject ram in search.Get())
			//{
			//	total += (UInt64)ram.GetPropertyValue("Capacity");
			//}
			UInt64 total = 0;
			ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory"); ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query); foreach (ManagementObject queryObj in searcher.Get())
			{
				string MemoryType;
				switch (Convert.ToInt32(queryObj["MemoryType"]))
				{
					case 20:
						MemoryType = "DDR";
						break;
					case 21:
						MemoryType = "DDR2";
						break;
					case 22:
						MemoryType = "DDR2 FB-DIMM";
						break;
					case 24:
						MemoryType = "DDR3";
						break;
					case 26:
						MemoryType = "DDR4";
						break;
					default:
						MemoryType = "Other";
						break;
				}
				if (MemoryType == "Other")
				{
					switch (Convert.ToInt32(queryObj["SMBIOSMemoryType"]))
					{
						case 20:
							MemoryType = "DDR";
							break;
						case 21:
							MemoryType = "DDR2";
							break;
						case 22:
							MemoryType = "DDR2 FB-DIMM";
							break;
						case 24:
							MemoryType = "DDR3";
							break;
						case 26:
							MemoryType = "DDR4";
							break;
						default:
							MemoryType = "Other";
							break;
					}
				}

				string formFactor;
				switch (Convert.ToInt32(queryObj["FormFactor"]))
				{
					case 1:
						formFactor = "Other";
						break;

					case 2:
						formFactor = "SIP";
						break;

					case 3:
						formFactor = "DIP";
						break;

					case 4:
						formFactor = "ZIP";
						break;

					case 5:
						formFactor = "SOJ";
						break;

					case 6:
						formFactor = "Proprietary";
						break;

					case 7:
						formFactor = "SIMM";
						break;

					case 8:
						formFactor = "DIMM";
						break;

					case 9:
						formFactor = "TSOP";
						break;

					case 10:
						formFactor = "PGA";
						break;

					case 11:
						formFactor = "RIMM";
						break;

					case 12:
						formFactor = "SODIMM";
						break;

					case 13:
						formFactor = "SRIMM";
						break;

					case 14:
						formFactor = "SMD";
						break;

					case 15:
						formFactor = "SSMP";
						break;

					case 16:
						formFactor = "QFP";
						break;

					case 17:
						formFactor = "TQFP";
						break;

					case 18:
						formFactor = "SOIC";
						break;

					case 19:
						formFactor = "LCC";
						break;

					case 20:
						formFactor = "PLCC";
						break;

					case 21:
						formFactor = "BGA";
						break;

					case 22:
						formFactor = "FPBGA";
						break;

					case 23:
						formFactor = "LGA";
						break;

					default:
						formFactor = "Unknown";
						break;
				}


				total += (UInt64)queryObj["Capacity"];
				Console.WriteLine(queryObj["Name"]);
				Console.WriteLine("Manufacturer: {0}", queryObj["Manufacturer"]);
				Console.WriteLine("Capacity: {0} GB", total / 1073741824);
				Console.WriteLine("MemoryType: {0}", MemoryType);
				Console.WriteLine("Speed: {0}", queryObj["Speed"]);
				Console.WriteLine("Device Locator: {0}", queryObj["DeviceLocator"]);
				Console.WriteLine("Part Number: {0}", queryObj["PartNumber"]);
				Console.WriteLine("Serial Number: {0}", queryObj["SerialNumber"]);
				Console.WriteLine("Form Factor: {0}", formFactor);
				Console.WriteLine("Insall Date: {0}", queryObj["InstallDate"]);
				Console.WriteLine("Data Width: {0}", queryObj["DataWidth"]);
				Console.WriteLine("TotalWidth: {0}", queryObj["TotalWidth"]);
				Console.WriteLine("Description: {0}", queryObj["Description"]);
				Console.WriteLine("Bank Label: {0}", queryObj["BankLabel"]);
				Console.WriteLine("Hot Swappable: {0}", queryObj["HotSwappable"]);
				Console.WriteLine("Position In Row: {0}", queryObj["PositionInRow"]);
				Console.WriteLine("Tag: {0}", queryObj["Tag"]);
				Console.WriteLine("Type Detail: {0}", queryObj["TypeDetail"]);
				Console.WriteLine("-----------------------------------");

				total = 0;
			}

			EndText();
		}

		static void MotherBoard()
		{
			Title(52, 35, 52, 35, true);

			Console.WriteLine("Motherboard Properties:");
			Console.WriteLine("------------------------------------");

			ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
			ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");

			foreach (ManagementObject queryObj in motherboardSearcher.Get())
			{
				switch (queryObj["Availability"])
				{
					case 1:
						Console.WriteLine("Availability: " + "Other");
						break;
					case 2:
						Console.WriteLine("Availability: " + "Unknown");
						break;
					case 3:
						Console.WriteLine("Availability: " + "Running or Full Power");
						break;
					case 4:
						Console.WriteLine("Availability: " + "Warning");
						break;
					case 5:
						Console.WriteLine("Availability: " + "In Test");
						break;
					case 6:
						Console.WriteLine("Availability: " + "Not Applicable");
						break;
					case 7:
						Console.WriteLine("Availability: " + "Power Off");
						break;
					case 8:
						Console.WriteLine("Availability: " + "Off Line");
						break;
					case 9:
						Console.WriteLine("Availability: " + "Off Duty");
						break;
					case 10:
						Console.WriteLine("Availability: " + "Degraded");
						break;
					case 11:
						Console.WriteLine("Availability: " + "Not Installed");
						break;
					case 12:
						Console.WriteLine("Availability: " + "Install Error");
						break;
					case 13:
						Console.WriteLine("Availability: " + "Power Save - Unknown");
						break;
					case 14:
						Console.WriteLine("Availability: " + "Power Save - Low Power Mode");
						break;
					case 15:
						Console.WriteLine("Availability: " + "Power Save - Standby");
						break;
					case 16:
						Console.WriteLine("Availability: " + "Power Cycle");
						break;
					case 17:
						Console.WriteLine("Availability: " + "Power Save - Warning");
						break;
					default:
						Console.WriteLine("Availability: " + "Unknown");
						break;
				}
				Console.WriteLine("PNPDeviceID: " + queryObj["PNPDeviceID"]);
				Console.WriteLine("PrimaryBusType: " + queryObj["PrimaryBusType"]);
				Console.WriteLine("SecondaryBusType: " + queryObj["SecondaryBusType"]);
				Console.WriteLine("RevisionNumber: " + queryObj["RevisionNumber"]);
				Console.WriteLine("SystemName: " + queryObj["SystemName"]);
			}
			Console.WriteLine();
			Console.WriteLine("Baseboard Properties:");
			Console.WriteLine("------------------------------------");
			foreach (ManagementObject bsb in baseboardSearcher.Get())
			{
				Console.WriteLine("HostingBoard: " + bsb["HostingBoard"]);
				Console.WriteLine("Manufacturer: " + bsb["Manufacturer"]);
				Console.WriteLine("Model: " + bsb["Model"]);
				Console.WriteLine("PartNumber: " + bsb["PartNumber"]);
				Console.WriteLine("Product: " + bsb["Product"]);
				Console.WriteLine("Removable: " + bsb["Removable"]);
				Console.WriteLine("Replaceable: " + bsb["Replaceable"]);
				Console.WriteLine("SerialNumber: " + bsb["SerialNumber"]);
				Console.WriteLine("Status: " + bsb["Status"]);
				Console.WriteLine("Version: " + bsb["Version"]);

				EndText();
			}
		}

		static void DiskAndSystem()
		{
			Title(52, 48, 52, 48, true);
			
			StringBuilder StringBuilder1 = new StringBuilder(string.Empty);
			try
			{
				StringBuilder1.AppendFormat("Operation System:  {0}\n", Environment.OSVersion);
				if (Environment.Is64BitOperatingSystem)
					StringBuilder1.AppendFormat("\t\t  64 Bit Operating System\n");
				else
					StringBuilder1.AppendFormat("\t\t  32 Bit Operating System\n");
				StringBuilder1.AppendFormat("SystemDirectory:  {0}\n", Environment.SystemDirectory);
				StringBuilder1.AppendFormat("ProcessorCount:  {0}\n", Environment.ProcessorCount);
				StringBuilder1.AppendFormat("UserDomainName:  {0}\n", Environment.UserDomainName);
				StringBuilder1.AppendFormat("UserName: {0}\n", Environment.UserName);

				//Drives
				StringBuilder1.AppendFormat("LogicalDrives:\n");
				foreach (System.IO.DriveInfo DriveInfo1 in System.IO.DriveInfo.GetDrives())
				{
					try
					{
						double total = DriveInfo1.TotalSize;
						double avafree = DriveInfo1.AvailableFreeSpace;
						double osztas = 1073741824;
						StringBuilder1.AppendFormat("\t Drive: {0}\n\t\t VolumeLabel: " +
						  "{1}\n\t\t DriveType: {2}\n\t\t DriveFormat: {3}\n\t\t " +
						  "TotalSize: {4}\n\t\t AvailableFreeSpace: {5}\n\t\t Root Directory: {6}\n\t\t Is Ready: {7}\n",
						  DriveInfo1.Name, DriveInfo1.VolumeLabel, DriveInfo1.DriveType,
						  DriveInfo1.DriveFormat, Math.Round(total / osztas, 2) + " GB", Math.Round(avafree / osztas, 2) + " GB", DriveInfo1.RootDirectory, DriveInfo1.IsReady);
					}
					catch
					{
					}
				}
				StringBuilder1.AppendFormat("SystemPageSize:  {0}\n", Environment.SystemPageSize);
				StringBuilder1.AppendFormat("Version:  {0}", Environment.Version);
			}
			catch
			{
			}
			Console.WriteLine(StringBuilder1.ToString());

			Console.WriteLine();
			EndText();
		}

		static void BIOS()
		{
			Title(52, 32, 52, 32, true);

			ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\.\root\cimv2", "SELECT * FROM Win32_BIOS");

			foreach (var queryObj in searcher.Get())
			{
				Console.WriteLine("CurrentLanguage: {0}", queryObj["CurrentLanguage"]);
				Console.WriteLine("Description: {0}", queryObj["Description"]);
				Console.WriteLine("InstallableLanguages: {0}", queryObj["InstallableLanguages"]);
				Console.WriteLine("InstallDate: {0}", queryObj["InstallDate"]);
				Console.WriteLine("LanguageEdition: {0}", queryObj["LanguageEdition"]);
				Console.WriteLine("Manufacturer: {0}", queryObj["Manufacturer"]);
				Console.WriteLine("PrimaryBIOS: {0}", queryObj["PrimaryBIOS"]);
				Console.WriteLine("ReleaseDate: {0}", queryObj["ReleaseDate"]);
				Console.WriteLine("SerialNumber: {0}", queryObj["SerialNumber"]);
				Console.WriteLine("Codeset: {0}", queryObj["Codeset"]);
				Console.WriteLine("SMBIOSBIOSVersion: {0}", queryObj["SMBIOSBIOSVersion"]);
				Console.WriteLine("SMBIOSMajorVersion: {0}", queryObj["SMBIOSMajorVersion"]);
				Console.WriteLine("SMBIOSMinorVersion: {0}", queryObj["SMBIOSMinorVersion"]);
				Console.WriteLine("SoftwareElementID: {0}", queryObj["SoftwareElementID"]);
				Console.WriteLine("SoftwareElementState: {0}", queryObj["SoftwareElementState"]);
				Console.WriteLine("TargetOperatingSystem: {0}", queryObj["TargetOperatingSystem"]);
				Console.WriteLine("Version: {0}", queryObj["Version"]);
			}
			EndText();
		}
		static void Website()
		{
			Process.Start("https://patriknagy.hu");

			Console.Clear();
			StartProgram();
		}

		static void ExitApp()
		{
			Environment.Exit(0);
		}

		static void InvalidInput()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid number!");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
			EndText();
		}
	}
}

