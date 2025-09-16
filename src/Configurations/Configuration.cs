using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace DownedAwareness;

class DownedAwarenessConfig
{
    public class Section
    {
        public readonly ConfigEntry<float> deadTimer;
        private List<string> _customItemsList = new List<string>();
        public IReadOnlyList<string> CustomItemsList => _customItemsList;

        internal Section(ConfigFile cfg, string sectionName)
        {
            deadTimer = cfg.Bind(
                sectionName,
                "DeadTimer",
                10f,
                new ConfigDescription(
                    "How long should the dead marker exists? (set 0 to turn off)",
                    new AcceptableValueRange<float>(0, 100)
                )
            );
        }
    }

    public readonly Section General;

    public DownedAwarenessConfig(ConfigFile cfg)
    {
        cfg.SaveOnConfigSet = false;

        General = new Section(cfg, "General");

        ClearOrphanedEntries(cfg);
        cfg.Save();
        cfg.SaveOnConfigSet = true;
    }

    static void ClearOrphanedEntries(ConfigFile cfg)
    {
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
        orphanedEntries.Clear();
    }
}
