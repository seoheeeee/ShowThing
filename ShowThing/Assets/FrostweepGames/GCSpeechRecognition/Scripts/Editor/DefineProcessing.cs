namespace FrostweepGames.Plugins.GoogleCloud.Translation
{
    [UnityEditor.InitializeOnLoad]
    public class DefineProcessing : Plugins.DefineProcessing
    {
        internal static readonly string[] _Defines = new string[]
        {
            "FG_TR"
        };

        static DefineProcessing()
        {
            AddOrRemoveDefines(true, true, _Defines);
        }
    }
}