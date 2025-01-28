namespace GB_Emulator_CSharp.interfaces
{
    public interface IBus{
        byte bus_read(ushort address);
        void bus_write(ushort address, byte value);
    }
}