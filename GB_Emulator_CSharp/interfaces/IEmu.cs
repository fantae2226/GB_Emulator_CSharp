namespace GB_Emulator_CSharp.interfaces{
    public class Emu_context{
        public bool Paused {get; set;}
        public bool Running {get; set;}
        public ulong Ticks {get; set;}
    }

    public interface IEmulator{
    

        public int EmuRun(int argc, string[] argv);

        public Emu_context EmuGetContext();
    }
}