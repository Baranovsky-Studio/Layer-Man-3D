using System;
using UnityEngine;

namespace BaranovskyStudio
{
    [Serializable]
    public abstract class Initializable : MonoBehaviour
    {
        public abstract void Initialize();
    }
}