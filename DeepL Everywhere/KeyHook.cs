
// (c) 2025 Kazuki Kohzuki

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DeepLEverywhere;

/// <summary>
/// Wraps the Windows API to hook keyboard events.
/// </summary>
internal static partial class KeyHook
{
    #region P/Invoke

    [LibraryImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowsHookExW")]
    private static partial IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnhookWindowsHookEx(IntPtr hhk);

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetModuleHandle(string lpModuleName);

    #endregion P/Invoke

    private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static KeyboardProc? proc;
    private static IntPtr hookId = IntPtr.Zero;

    private static readonly Dictionary<Keys, DateTime> monitoring = [];

    /// <summary>
    /// The interval in milliseconds to consider a key press as a valid event.
    /// </summary>
    internal static int KeyEventIntervalMillisecond { get; set; } = 1000;

    /// <summary>
    /// Event that is raised when a key is pressed down.
    /// </summary>
    internal static event KeyEventHandler? KeyDownCallback;

    /// <summary>
    /// Hooks the keyboard events to monitor key presses.
    /// </summary>
    /// <remarks>
    /// This method do nothing if the hook is already set.
    /// </remarks>
    internal static void Hook()
    {
        if (hookId != IntPtr.Zero) return;  // Already hooked

        proc = HookProcedure;
        var current = Process.GetCurrentProcess();
        var module = current.MainModule;
        var handle = GetModuleHandle(module?.ModuleName ?? string.Empty);
        hookId = SetWindowsHookEx(13, proc, handle, 0);
    } // internal void Hook ()

    /// <summary>
    /// Unhooks the keyboard events, stopping the monitoring of key presses.
    /// </summary>
    internal static void Unhook()
    {
        UnhookWindowsHookEx(hookId);
        hookId = IntPtr.Zero;
    } // internal void Unhook ()

    private static IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var key = (Keys)(short)Marshal.ReadInt32(lParam);

            InvokeKeyDownCallback(key);
        }

        return CallNextHookEx(hookId, nCode, wParam, lParam);
    } // private static IntPtr HookProcedure (int, uint, uint)

    private static void InvokeKeyDownCallback(Keys key)
    {
        if (!monitoring.TryGetValue(key, out var last)) return;
        var now = monitoring[key] = DateTime.UtcNow;
        if ((now - last).TotalMilliseconds <= KeyEventIntervalMillisecond) return;

        KeyDownCallback?.Invoke(null, new(key));
    } // private static void InvokeKeyDownCallback (Keys)

    /// <summary>
    /// Monitors the specified key by adding it to the monitoring dictionary with a default value of DateTime.MinValue.
    /// </summary>
    /// <param name="key">The key to monitor.</param>
    internal static void Monitor(Keys key)
        => monitoring.Add(key, DateTime.MinValue);

    /// <summary>
    /// Unmonitors the specified key, removing it from the monitoring dictionary.
    /// </summary>
    /// <param name="key">The key to unmonitor.</param>
    internal static void Unmonitor(Keys key)
        => monitoring.Remove(key);
} // internal static class KeyHook