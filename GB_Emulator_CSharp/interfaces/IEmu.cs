namespace GB_Emulator_CSharp.interfaces{
    public class Emu_context{
        public bool Paused {get; set;}
        public bool Running {get; set;}
        public ulong Ticks {get; set;}
    }

    public class Emulator{
        private static Emu_context emu_Context = new Emu_context();

        public static int EmuRun(string[] args){

            //placeholder
            Console.WriteLine("Runnign Emulator");

            emu_Context.Running = true;

            return 0;
        }

        public static Emu_context EmuGetContext(){
            return emu_Context;
        }
    }
}