﻿using Avalonia;
using Avalonia.ReactiveUI;

namespace Maurice.UI
{
    class Program
    {
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .LogToTrace()
                    .UseReactiveUI();
    }
}
