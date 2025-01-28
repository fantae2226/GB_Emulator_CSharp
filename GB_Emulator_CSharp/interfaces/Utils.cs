namespace GB_Emulator_CSharp.interfaces{
    public class Utils{

        public static int Bit(int a, int n){
            return ((a & (1 << n)) != 0) ? 1 : 0;
        }

        public static void Bit_Set(ref int a, int n, bool on){
            if (on){
                a |=  1 << n;
            }
            else{
                a &= ~(1 << n);
            }
        }

        public static bool Between(int a, int b, int c){
            return (a >= b) && (a <= c);
        }

        public static void Delay(uint ms) {
            Thread.Sleep((int)ms);
        }
    }
}