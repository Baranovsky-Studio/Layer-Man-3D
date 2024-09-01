using System;
using UnityEngine;

namespace Source.Scripts
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/New", order = 0)]
    public class Settings : ScriptableObject
    {
        public int[] CirclesCounts;
        public float[] Incomes;

        [Serializable]
        public struct CircleSettings
        {
            public Mesh Big;
            public Mesh Normal;
            public Mesh Nano;

            public float MinScale;
        }

        public CircleSettings[] CirclesSettings;
    }
}