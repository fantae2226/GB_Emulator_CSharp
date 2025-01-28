using SFML.Graphics;
using SFML.Window;
using GB_Emulator_CSharp.interfaces;
using GB_Emulator_CSharp.lib;

class Program{
    static int Main(string[] args){

        //test for SFML

        // RenderWindow window = new RenderWindow(new VideoMode(800,600), "SMFL.Net Test");

        // window.Closed += (sender, e) => window.Close();

        // while(window.IsOpen){
        //     window.DispatchEvents();
        //     window.Clear();
        //     window.Display();
        // }


        //test emu
        Emulator emulator= new Emulator();
        return emulator.EmuRun(args);

    }
}


