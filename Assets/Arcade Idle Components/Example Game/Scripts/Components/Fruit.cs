using System;
using UnityEngine;

namespace BaranovskyStudio.SimpleGame
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Fruit : MonoBehaviour
    {
        public SpecialBackpack.ResourceType resourceType;
        
        public Action<Fruit> OnPickup;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void FallDown()
        {
            _rigidbody.useGravity = true;
        }
    }
}