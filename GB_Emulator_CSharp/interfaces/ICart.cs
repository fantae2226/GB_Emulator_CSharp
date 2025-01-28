using System.Runtime.InteropServices;

namespace GB_Emulator_CSharp.interfaces{
    public class RomHeader
{
    public byte[] Entry { get; set; }
    public byte[] Logo { get; set; }
    public char[] Title { get; set; }
    public ushort NewLicCode { get; set; }
    public byte SgbFlag { get; set; }
    public byte Type { get; set; }
    public byte RomSize { get; set; }
    public byte RamSize { get; set; }
    public byte DestCode { get; set; }
    public byte LicCode { get; set; }
    public byte Version { get; set; }
    public byte Checksum { get; set; }
    public ushort GlobalChecksum { get; set; }

    public RomHeader()
    {
        Entry = new byte[4];
        Logo = new byte[0x30];
        Title = new char[16];
    }
}

    public interface ICartLoader{
        public bool CartLoad(string cart);
    }
}
