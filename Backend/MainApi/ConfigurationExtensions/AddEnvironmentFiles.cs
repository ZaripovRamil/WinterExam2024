using Utils.LocalRun;

namespace WinterExam24.ConfigurationExtensions;

public static class AddEnvironmentFilesExtension
{
    public static void AddEnvironmentFiles()
    {
        EnvFileLoader.LoadFilesFromParentDirectory(".postgres-secrets", "local.secrets",
            Path.Combine("..", "local.hostnames"), "local.kestrel-conf");
    }
}