using UnityEditor;
using UnityEngine;

namespace BaranovskyStudio.Editor
{
    public class Menu : EditorWindow
    {
        [MenuItem("Tools/Arcade Idle Components/Open settings")]
        private static void OpenSettings()
        {
            Selection.activeObject = Resources.Load<Settings>(Constants.SETTINGS);
        }
        
        [MenuItem("Tools/Arcade Idle Components/Open online docs")]
        public static void OpenDocs()
        {
            Application.OpenURL("https://baranovskystudio.wixsite.com/site/forum/arcade-idle-components/main-page");
        }

        [MenuItem("Tools/Arcade Idle Components/Contact us")]
        private static void ContactUs()
        {
            Application.OpenURL("https://baranovskystudio.wixsite.com/site/contact");
        }
    }
}
