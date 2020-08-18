using UnityEngine;

namespace Systems
{
    public static class SettingsManager
    {
        //Reference Variables
        
        //Configuration Parameters

        //State Variables

        //Internal Methods

        //Public Methods
        public static void SavePreferences() {
            PlayerPrefs.Save();
        }

        public static void LoadPreferences() {
            PlayerPrefs.Save();
        }
    }
}
