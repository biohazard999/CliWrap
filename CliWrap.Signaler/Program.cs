﻿using System.Runtime.InteropServices;
using CliWrap.Signaler.Utils;

namespace CliWrap.Signaler;

// Implementation reference:
// https://github.com/madelson/MedallionShell/blob/3fddb89860842ffc836a0d0f69b161f67e4aa7c4/MedallionShell.ProcessSignaler/Signals/Signaler.cs
// MIT License, Michael Adelson

public static class Program
{
    public static int Main(string[] args)
    {
        var processId = int.Parse(args[0]);
        var signal = int.Parse(args[1]);

        // Detach from the current console, if it exists.
        // Potential error here would mean that we're not attached to any console, so just ignore it.
        NativeMethods.Windows.FreeConsole();

        var isSuccess =
            // Attach to the target process's console
            NativeMethods.Windows.AttachConsole((uint) processId) &&
            // Ignore signals on ourselves
            NativeMethods.Windows.SetConsoleCtrlHandler(null, true) &&
            // Send the signal to the target process
            NativeMethods.Windows.GenerateConsoleCtrlEvent((uint) signal, 0);

        return isSuccess
            ? 0
            : Marshal.GetLastWin32Error();
    }
}