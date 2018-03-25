namespace UXI.SystemApi.Screen
{
    public interface IScreenParametersProvider
    {
        void GetScreenDpi(ScreenInfo screen, ScreenInterop.DpiType dpiType, out uint dpiX, out uint dpiY);
        bool TryGetScreenInfo(int pointLeft, int pointTop, out ScreenInfo screenInfo);
    }
}