
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoom.Baseball
{
    public class BaseballTargetRing : MonoBehaviour
    {
        internal static readonly HashSet<BaseballTargetRing> Active = new();

        public string targetName = "CENTER";
        public float innerRadius = .83f;
        public TMP_Text label;
        public UnityEvent onScored = new();

        public int Hits { get; private set; }
        public bool IsPassed => Hits > 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRegistry() => Active.Clear();

        private void OnEnable()
        {
            Active.Add(this);
            Refresh();
        }

        private void OnDisable() => Active.Remove(this);

        public bool Crosses(Vector3 from, Vector3 to, float ballRadius)
        {
            Vector3 a = transform.InverseTransformPoint(from);
            Vector3 b = transform.InverseTransformPoint(to);

            if (a.z * b.z > 0 || Mathf.Approximately(a.z, b.z))
                return false;

            float t = -a.z / (b.z - a.z);
            if (t < 0 || t > 1) return false;

            Vector3 p = Vector3.Lerp(a, b, t);
            float scale = Mathf.Min(
                Mathf.Abs(transform.lossyScale.x),
                Mathf.Abs(transform.lossyScale.y)
            );
            float r = innerRadius - ballRadius / scale;

            return r > 0 && p.x * p.x + p.y * p.y < r * r;
        }

        internal void Score()
        {
            Hits++;
            Refresh();
            onScored.Invoke();
        }

        public void ResetTarget()
        {
            Hits = 0;
            Refresh();
        }

        private void Refresh()
        {
            if (label)
            {
                label.text = targetName + "\n" + (IsPassed ? "DONE" : "TARGET");
                label.color = IsPassed ? new Color(.3f, 1f, .45f) : Color.white;
            }

            var renderer = GetComponent<Renderer>();
            if (!renderer) return;

            var block = new MaterialPropertyBlock();

            if (IsPassed)
                block.SetColor("_BaseColor", new Color(.1f, .8f, .2f));

            renderer.SetPropertyBlock(block);
        }
    }
}
