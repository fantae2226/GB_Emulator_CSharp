using GB_Emulator_CSharp.interfaces;
using SFML.Graphics;
using SFML.Window;

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

        static Emu_context ctx = new Emu_context();
        static CartLoader cartLoader = new();
        static Cpu cpuLoader = new(); 


        public Emu_context EmuGetContext()
        {
            return ctx ?? (ctx = new Emu_context());
        }


        public static void delay(uint ms){
            Thread.Sleep((int)ms);
        }

        public int EmuRun(string[] args)
        {
            if (args.Length < 1){ //check later
                Console.WriteLine("Usage: Emu <rom_file>");
                return -1;
            }

            if (!cartLoader.CartLoad(args[0])){
                Console.WriteLine($"Failed to load ROM file: {args[0]}");
                return -2;
            }

            Console.WriteLine("Cart loaded...");

            //SDL Graphics replacement

            RenderWindow window = new RenderWindow(new VideoMode(800,600), "GB Emulator");
            window.Closed += (sender, e) => ctx.Running = false;

            Console.WriteLine("Graphics Init...");

            //TTF replacement
            Console.WriteLine("TTF replacement Init...");


            cpuLoader.cpu_init();


            ctx.Running = true;
            ctx.Paused = false;
            ctx.Ticks = 0;

            while(ctx.Running){

                window.DispatchEvents(); //handle window events

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