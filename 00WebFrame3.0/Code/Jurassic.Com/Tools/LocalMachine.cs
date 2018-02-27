using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 获取本地机器信息的类
    /// </summary>
    public class LocalMachine
    {
        /// <summary>
        /// 获取所有本地逻辑磁盘的信息表
        /// </summary>
        public static IList<DiskPartInfo> GetDiskPartInfo()
        {
            IList<DiskPartInfo> parts = new List<DiskPartInfo>();
            ManagementClass managementClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = managementClass.GetInstances();
            foreach (ManagementBaseObject disk in disks)
            {
                DiskPartInfo part = new DiskPartInfo()
                {
                    DeviceID = CommOp.ToStr(disk.Properties["DeviceID"].Value),
                    FileSystem = CommOp.ToStr(disk.Properties["FileSystem"].Value),
                    FreeSpace = CommOp.ToLong(disk.Properties["FreeSpace"].Value),
                    Size = CommOp.ToLong(disk.Properties["Size"].Value),
                    SystemName = CommOp.ToStr(disk.Properties["SystemName"].Value),
                    VolumeSerialNumber = CommOp.ToStr(disk.Properties["VolumeSerialNumber"].Value),
                };
                parts.Add(part);
            }
            return parts;
        }

        /// <summary>
        /// 获取所有驱动器信息表
        /// </summary>
        public static IList<DiskInfo> GetDiskInfo()
        {
            List<DiskInfo> disks = new List<DiskInfo>();
            ManagementClass managementClass = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = managementClass.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                //Console.WriteLine(mo); 
                DiskInfo disk = new DiskInfo()
                {
                    Name = CommOp.ToStr(mo.Properties["Name"].Value),
                    Model = CommOp.ToStr(mo.Properties["Model"].Value),
                    PNPDeviceID = CommOp.ToStr(mo.Properties["PNPDeviceID"].Value),
                    Size = CommOp.ToLong(mo.Properties["Size"].Value),
                };
                disks.Add(disk);
            }
            return disks;
        }

        /// <summary>
        /// 获取所有CPU信息列表
        /// </summary>
        /// <returns></returns>
        public static IList<CpuInfo> GetCpuInfo()
        {
            ManagementClass managementClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = managementClass.GetInstances();
            IList<CpuInfo> cpus = new List<CpuInfo>();
            foreach (ManagementObject mo in moc)
            {
                CpuInfo cpu = new CpuInfo()
                {
                    AddressWidth = CommOp.ToInt(mo.Properties["AddressWidth"]),
                    CurrentClockSpeed = CommOp.ToInt(mo.Properties["CurrentClockSpeed"]),
                    MaxClockSpeed = CommOp.ToInt(mo.Properties["MaxClockSpeed"]),
                    Description = CommOp.ToStr(mo.Properties["Description"]),
                    DeviceID = CommOp.ToStr(mo.Properties["DeviceID"]),
                    ExtClock = CommOp.ToInt(mo.Properties["ExtClock"]),
                    L2CacheSize = CommOp.ToInt(mo.Properties["L2CacheSize"]),
                    Manufacturer = CommOp.ToStr(mo.Properties["Manufacturer"]),
                    Name = CommOp.ToStr(mo.Properties["Name"]),
                    ProcessorId = CommOp.ToStr(mo.Properties["ProcessorId"]),
                };
                cpus.Add(cpu);
            }
            return cpus;
        }

        /// <summary>
        /// 根据机器的硬件信息获取机器的唯一标识
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCode()
        {
            // string id = GetCpuInfo()[0].ProcessorId;
            return Encryption.MD5(GetDiskInfo()[0].PNPDeviceID + GetNetCardInfo()[0].MACAddress);
        }

        /// <summary>
        /// 获取所有网卡信息列表
        /// </summary>
        /// <returns></returns>
        public static IList<NetCardInfo> GetNetCardInfo()
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = managementClass.GetInstances();
            List<NetCardInfo> cards = new List<NetCardInfo>();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    NetCardInfo card = new NetCardInfo()
                    {
                        Caption = CommOp.ToStr(mo.Properties["Caption"].Value),
                        Description = CommOp.ToStr(mo.Properties["Description"].Value),
                        MACAddress = CommOp.ToStr(mo.Properties["MACAddress"].Value),
                        SettingID = CommOp.ToGuid(mo.Properties["SettingID"].Value),
                    };
                    if (card.MACAddress != "00:00:00:00:00:00")
                        cards.Add(card);
                }
            }
            return cards;
        }
    }

    /// <summary>
    /// 逻辑磁盘分区信息实体类
    /// </summary>
    public class DiskPartInfo
    {
        /// <summary>
        /// 磁盘分区ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 磁盘总空间,单位:Byte
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件系统类型
        /// </summary>
        public string FileSystem { get; set; }

        /// <summary>
        /// 可用空间,单位:Byte
        /// </summary>
        public long FreeSpace { get; set; }

        /// <summary>
        /// 卷序列号(格式化时生成的)
        /// </summary>
        public string VolumeSerialNumber { get; set; }

        /// <summary>
        /// 机器名
        /// </summary>
        public string SystemName { get; set; }
    }

    /// <summary>
    /// 物理磁盘信息实体类
    /// </summary>
    public class DiskInfo
    {
        /// <summary>
        /// 磁盘名称,如:\\.\PHYSICALDRIVE0
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 磁盘型号,如:TOSHIBA MK6034GAX
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 磁盘PNP设备ID,如IDE\DISKTOSHIBA_MK6034GAX_______________________AC101A__...
        /// </summary>
        public string PNPDeviceID { get; set; }

        /// <summary>
        /// 磁盘总空量,单位:Byte
        /// </summary>
        public long Size { get; set; }
    }

    /// <summary>
    /// CPU信息实体类
    /// </summary>
    public class CpuInfo
    {
        /// <summary>
        /// 地址宽度 (32,64)
        /// </summary>
        public int AddressWidth { get; set; }

        /// <summary>
        /// CPU名称,如:Intel(R) Celeron(R) M CPU        420  @ 1.60GHz
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CPU描述,如:x86 Family 6 Model 14 Stepping 8
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 当前时钟频率 (MHZ)
        /// </summary>
        public int CurrentClockSpeed { get; set; }

        /// <summary>
        /// 最大时钟频率 (MHZ)
        /// </summary>
        public int MaxClockSpeed { get; set; }

        /// <summary>
        /// 设备标识,如:CPU0
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 外部时钟频率(MHZ)
        /// </summary>
        public int ExtClock { get; set; }

        /// <summary>
        /// 二级缓存容量(KB)
        /// </summary>
        public int L2CacheSize { get; set; }

        /// <summary>
        /// 制造商名称,如:GenuineIntel
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 处理器ID,如:AFE9FBFF000006E8
        /// </summary>
        public string ProcessorId { get; set; }
    }

    /// <summary>
    /// 网卡信息实体类
    /// </summary>
    public class NetCardInfo
    {
        /// <summary>
        /// 网卡的设定ID, 如:{4D9FE433-28B3-4F54-A7A8-428FFE1A81A7}
        /// </summary>
        public Guid SettingID { get; set; }

        /// <summary>
        /// 网卡的标题,如:[00000007] Marvell Yukon 88E8038 PCI-E Fast Ethernet Controller
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 网卡的描述,如:Marvell Yukon 88E8038 PCI-E Fast Ethernet Controller - Virtual Machine Network Services Driver
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 网卡的MAC地址,如:00:16:A6:86:66:49
        /// </summary>
        public string MACAddress { get; set; }

    }
}
