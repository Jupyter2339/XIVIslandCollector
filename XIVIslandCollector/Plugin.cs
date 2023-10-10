using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using ECommons;
using XIVIslandCollector.Windows;

namespace XIVIslandCollector
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "XIVIslandCollector";
        private const string CommandName = "/xic";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("XIVIslandCollector");

        private Main Main { get; init; }

        public Plugin(
            DalamudPluginInterface pluginInterface,
            CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            ECommonsMain.Init(pluginInterface, this);
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            Main = new Main();
            WindowSystem.AddWindow(Main);
            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "usage command: /xic"
            });
            this.PluginInterface.UiBuilder.Draw += DrawUI;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            Main.Dispose();

            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            Main.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }
    }
}
