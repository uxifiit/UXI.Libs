namespace UXI.SystemApi.Mouse
{
    public class MouseCoordinatesHook
    {
        public bool TryGetCursorPosition(out Point point)
        {
            return HookInterop.GetPhysicalCursorPos(out point);
        }
    }
}
