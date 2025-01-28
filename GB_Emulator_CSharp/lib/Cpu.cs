using GB_Emulator_CSharp.interfaces;

namespace GB_Emulator_CSharp.lib{

    public class Cpu : ICpu
    {
        public void cpu_init()
        {
            throw new NotImplementedException();
        }

        public bool cpu_step()
        {
            Console.WriteLine("Cpu not yet implemented.");
            return false;
        }
    }
}