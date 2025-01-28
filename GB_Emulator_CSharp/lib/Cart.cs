using System.Runtime.InteropServices;
using GB_Emulator_CSharp.interfaces;

namespace GB_Emulator_CSharp.lib{
    public class Cart_context{

        public string Filename { get; set; } = new string(new char[1024]);

        public uint RomSize { get; set; }
        public byte[]? RomData { get; set; }

        public RomHeader? RomHeader { get; set; }

    }

    public class CartLoader : ICartLoader
    {

        static Cart_context? ctx;

        public static readonly string[] ROM_TYPES = [ 
            "ROM ONLY",
            "MBC1",
            "MBC1+RAM",
            "MBC1+RAM+BATTERY",
            "0x04 ???",
            "MBC2",
            "MBC2+BATTERY",
            "0x07 ???",
            "ROM+RAM 1",
            "ROM+RAM+BATTERY 1",
            "0x0A ???",
            "MMM01",
            "MMM01+RAM",
            "MMM01+RAM+BATTERY",
            "0x0E ???",
            "MBC3+TIMER+BATTERY",
            "MBC3+TIMER+RAM+BATTERY 2",
            "MBC3",
            "MBC3+RAM 2",
            "MBC3+RAM+BATTERY 2",
            "0x14 ???",
            "0x15 ???",
            "0x16 ???",
            "0x17 ???",
            "0x18 ???",
            "MBC5",
            "MBC5+RAM",
            "MBC5+RAM+BATTERY",
            "MBC5+RUMBLE",
            "MBC5+RUMBLE+RAM",
            "MBC5+RUMBLE+RAM+BATTERY",
            "0x1F ???",
            "MBC6",
            "0x21 ???",
            "MBC7+SENSOR+RUMBLE+RAM+BATTERY" 
        ];

        public static readonly Dictionary<int, string> LIC_CODES = new Dictionary<int, string>
        {
            { 0x00, "None" },
            { 0x01, "Nintendo R&D1" },
            { 0x08, "Capcom" },
            { 0x13, "Electronic Arts" },
            { 0x18, "Hudson Soft" },
            { 0x19, "b-ai" },
            { 0x20, "kss" },
            { 0x22, "pow" },
            { 0x24, "PCM Complete" },
            { 0x25, "san-x" },
            { 0x28, "Kemco Japan" },
            { 0x29, "seta" },
            { 0x30, "Viacom" },
            { 0x31, "Nintendo" },
            { 0x32, "Bandai" },
            { 0x33, "Ocean/Acclaim" },
            { 0x34, "Konami" },
            { 0x35, "Hector" },
            { 0x37, "Taito" },
            { 0x38, "Hudson" },
            { 0x39, "Banpresto" },
            { 0x41, "Ubi Soft" },
            { 0x42, "Atlus" },
            { 0x44, "Malibu" },
            { 0x46, "angel" },
            { 0x47, "Bullet-Proof" },
            { 0x49, "irem" },
            { 0x50, "Absolute" },
            { 0x51, "Acclaim" },
            { 0x52, "Activision" },
            { 0x53, "American sammy" },
            { 0x54, "Konami" },
            { 0x55, "Hi tech entertainment" },
            { 0x56, "LJN" },
            { 0x57, "Matchbox" },
            { 0x58, "Mattel" },
            { 0x59, "Milton Bradley" },
            { 0x60, "Titus" },
            { 0x61, "Virgin" },
            { 0x64, "LucasArts" },
            { 0x67, "Ocean" },
            { 0x69, "Electronic Arts" },
            { 0x70, "Infogrames" },
            { 0x71, "Interplay" },
            { 0x72, "Broderbund" },
            { 0x73, "sculptured" },
            { 0x75, "sci" },
            { 0x78, "THQ" },
            { 0x79, "Accolade" },
            { 0x80, "misawa" },
            { 0x83, "lozc" },
            { 0x86, "Tokuma Shoten Intermedia" },
            { 0x87, "Tsukuda Original" },
            { 0x91, "Chunsoft" },
            { 0x92, "Video system" },
            { 0x93, "Ocean/Acclaim" },
            { 0x95, "Varie" },
            { 0x96, "Yonezawa/s’pal" },
            { 0x97, "Kaneko" },
            { 0x99, "Pack in soft" },
            { 0xA4, "Konami (Yu-Gi-Oh!)" }
        };

        public static string cart_lic_name(){
            ContextCheck();
            if (ctx.RomHeader != null && ctx.RomHeader.NewLicCode <= 0xA4){
                return LIC_CODES[ctx.RomHeader.LicCode];
            }

            return "UNKNOWN";
        }

        public static string cart_type_name(){
            ContextCheck();
            if (ctx.RomHeader != null && ctx.RomHeader.Type <= 0x22){
                return ROM_TYPES[ctx.RomHeader.Type];
            }

            return "UNKNOWN";
        }

        public static void ContextCheck(){
            if (ctx == null){
                throw new ArgumentNullException("Card Context cannot be null.");
            }
        }




        public bool CartLoad(string cart)
        {
            ContextCheck();
            ctx.Filename = $"{cart}";

            using (FileStream fs = new FileStream(cart, FileMode.Open, FileAccess.Read) ){
                if(!fs.CanRead){
                    Console.WriteLine($"Failed to open: {cart}");
                    return false;
                }

                Console.WriteLine($"Opened {cart}");

                ctx.RomSize = (uint)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);

                ctx.RomData = new byte[ctx.RomSize];
                fs.ReadExactly(ctx.RomData, 0, (int)ctx.RomSize);
                fs.Close();
            }

            if(ctx.RomData != null){

                ctx.RomHeader = new RomHeader();

                //Manually copy bytes into RomHeader properties
                Array.Copy(ctx.RomData, 0x0100, ctx.RomHeader.Entry, 0 , 4);
                Array.Copy(ctx.RomData, 0x0104, ctx.RomHeader.Logo, 0, 0x30);
                Array.Copy(ctx.RomData, 0x0134, ctx.RomHeader.Title, 0, 16);

                 // Set primitive data types from the appropriate offsets
                ctx.RomHeader.NewLicCode = BitConverter.ToUInt16(ctx.RomData, 0x013C); // NewLicCode (2 bytes)
                ctx.RomHeader.SgbFlag = ctx.RomData[0x013E]; // SgbFlag (1 byte)
                ctx.RomHeader.Type = ctx.RomData[0x013F]; // Type (1 byte)
                ctx.RomHeader.RomSize = ctx.RomData[0x0140]; // RomSize (1 byte)
                ctx.RomHeader.RamSize = ctx.RomData[0x0141]; // RamSize (1 byte)
                ctx.RomHeader.DestCode = ctx.RomData[0x0142]; // DestCode (1 byte)
                ctx.RomHeader.LicCode = ctx.RomData[0x0143]; // LicCode (1 byte)
                ctx.RomHeader.Version = ctx.RomData[0x0144]; // Version (1 byte)
                ctx.RomHeader.Checksum = ctx.RomData[0x0145]; // Checksum (1 byte)
                ctx.RomHeader.GlobalChecksum = BitConverter.ToUInt16(ctx.RomData, 0x0146); // GlobalChecksum (2 bytes)

                ctx.RomHeader.Title[15] = '\0';

            }

            Console.WriteLine("Cartridge Loaded");
            Console.WriteLine($"Title: {ctx.RomHeader.Title}");
            Console.WriteLine($"Type: {ctx.RomHeader.Type} ({cart_type_name()})");
            Console.WriteLine($"ROM Size: {32 << ctx.RomHeader.RomSize} KB");
            Console.WriteLine($"RAM Size: {ctx.RomHeader.RamSize}");
            Console.WriteLine($"LIC Code: {ctx.RomHeader.LicCode} ({cart_lic_name()})");
            Console.WriteLine($"ROM Vers: {ctx.RomHeader.Version}");

            ushort sum = 0;

            for(ushort i = 0x0134; i <= 0x014C; i++){
                sum = (ushort)(sum - ctx.RomData[i] - 1);
            }

            Console.WriteLine($"Checksum: {ctx.RomHeader.Checksum} ( {((sum & 0xFF) == 0 ? "Passed" : "Failed")} )");

            return true;


            

        }
    }
}