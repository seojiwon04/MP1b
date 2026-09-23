
using System.Collections.Generic;
using UnityEngine;

namespace EscapeRoom.Baseball
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class BaseballProjectile : MonoBehaviour
    {
        public float lifetime = 12f;
        public float radius = .055f;
        public bool IsLaunched { get; private set; }

        private Rigidbody body;
        private Vector3 previous;
        private readonly HashSet<BaseballTargetRing> scored = new();

        private void Awake() => body = GetComponent<Rigidbody>();

        public void Launch(Vector3 velocity, Collider[] ignore)
        {
            body.isKinematic = false;
            body.useGravity = true;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            body.linearVelocity = velocity;
            body.angularVelocity = Random.insideUnitSphere * 12f;

            foreach (var other in ignore)
                if (other) Physics.IgnoreCollision(GetComponent<Collider>(), other);

            previous = body.position;
            IsLaunched = true;
            Destroy(gameObject, lifetime);
        }

        private void FixedUpdate()
        {
            if (!IsLaunched) return;

            Vector3 current = body.position;

            foreach (var ring in BaseballTargetRing.Active)
            {
                if (scored.Contains(ring) || !ring.Crosses(previous, current, radius))
                    continue;

                scored.Add(ring);
                ring.Score();
            }

            previous = current;
        }
    }
}
