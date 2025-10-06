using System;
using LabApi.Features.Console;

namespace Scp066;

internal abstract class LogManager
{
    private static bool DebugEnabled => Scp066.Instance?.Config?.Debug == true;

    public static void Debug(string message)
    {
        if (!DebugEnabled)
            return;

        Logger.Raw($"[DEBUG] [{Scp066.Instance?.Name ?? "Scp066"}] {message}", ConsoleColor.Green);
    }

    public static void Info(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        Logger.Raw($"[INFO] [{Scp066.Instance?.Name ?? "Scp066"}] {message}", color);
    }

    public static void Warn(string message)
    {
        Logger.Warn(message);
    }

    public static void Error(string message)
    {
        Logger.Raw($"[ERROR] [{Scp066.Instance?.Name ?? "Scp066"}] {message}", ConsoleColor.Red);
    }
}