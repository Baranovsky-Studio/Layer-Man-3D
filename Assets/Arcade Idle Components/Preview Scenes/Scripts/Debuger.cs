using UnityEngine;

namespace BaranovskyStudio.Example
{
    public class Debuger : MonoBehaviour
    {
        public void SendLog(string log)
        {
            Debug.Log(log);
        }
    }
}
