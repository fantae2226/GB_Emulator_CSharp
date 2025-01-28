using GB_Emulator_CSharp.interfaces;

namespace GB_Emulator_CSharp.lib{
    /* 
    Emu components:

    |Cart|
    |CPU|
    |Address Bus|
    |PPU|
    |Timer|

    */

    public class Emulator : IEmulator
    {

        static Emu_context? ctx;
        static CartLoader cartLoader = new();
        static Cpu cpuLoader = new(); 


        public Emu_context EmuGetContext()
        {
            return ctx ?? (ctx = new Emu_context());
        }


        public static void delay(uint ms){
            Thread.Sleep((int)ms);
        }

        public int EmuRun(int argc, string[] argv)
        {
            if (argc < 2){
                Console.WriteLine("Usage: Emu <rom_file>");
                return -1;
            }

            if (!cartLoader.CartLoad(argv[1])){
                Console.WriteLine($"Failed to load ROM file: {argv[1]}");
                return -2;
            }

            Console.WriteLine("Cart loaded...");

            //SDL Graphics replacement
            Console.WriteLine("Graphics Init...");

            //TTF replacement
            Console.WriteLine("TTF replacement Init...");


            cpuLoader.cpu_init();


            ctx.Running = true;
            ctx.Paused = false;
            ctx.Ticks = 0;

            while(ctx.Running){
                if(ctx.Paused){
                    delay(10);
                    continue;
                }
                
                if (!cpuLoader.cpu_step()){
                    Console.WriteLine("CPU Stopped.");
                    return -3;
                }

                ctx.Ticks++;
            }

            return 0;

        }
    }



}