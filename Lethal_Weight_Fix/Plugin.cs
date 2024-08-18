using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Lethal_Weight_Fix
{
    [BepInPlugin(mod_GUID, mod_name, mod_version)]
    public class Lethal_weight_fix_base : BaseUnityPlugin
    {
        private const string mod_GUID = "Hackattack242.Lethal_Weight_Fix";
        private const string mod_name = "Lethal_Weight_Fix";
        private const string mod_version = "1.1.1";

        private readonly Harmony harmony = new Harmony(mod_GUID);

        private static Lethal_weight_fix_base Instance;

        private ManualLogSource logger;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            logger = BepInEx.Logging.Logger.CreateLogSource(mod_name);

            logger.LogInfo("Lethal Weight Fix has awoken");

            harmony.PatchAll();

            logger.LogInfo("Lethal Weight Fix Patches Applied");
        }

        internal static void LogDebug(string message)
        {
            Instance.Log(message, (LogLevel)32);
        }

        internal static void LogInfo(string message)
        {
            Instance.Log(message, (LogLevel)16);
        }

        internal static void LogWarning(string message)
        {
            Instance.Log(message, (LogLevel)4);
        }

        internal static void LogError(string message)
        {
            Instance.Log(message, (LogLevel)2);
        }

        internal static void LogError(Exception ex)
        {
            Instance.Log(ex.Message + "\n" + ex.StackTrace, (LogLevel)2);
        }

        private void Log(string message, LogLevel logLevel)
        {
            ((Lethal_weight_fix_base)this).logger.Log(logLevel, (object)message);
        }

    }

}
