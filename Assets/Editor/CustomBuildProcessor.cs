using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DPUtils.Helpers
{
    class CustomBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            var db = Resources.Load<Database>("Database");
            db.SetItemID();
        }
    }
}
