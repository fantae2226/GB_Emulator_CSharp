using SFML.Graphics;
using SFML.Window;

class Program{
    static void Main(){
        RenderWindow window = new RenderWindow(new VideoMode(800,600), "SMFL.Net Test");

        window.Closed += (sender, e) => window.Close();

        while(window.IsOpen){
            window.DispatchEvents();
            window.Clear();
            window.Display();
        }
    }
}


