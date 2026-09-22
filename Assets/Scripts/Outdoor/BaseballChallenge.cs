using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace EscapeRoom.Baseball
{
    public class BaseballChallenge : MonoBehaviour
    {
        public BaseballTargetRing[] targets;
        public TMP_Text progressLabel;
        public UnityEvent onChallengeCompleted = new UnityEvent();
        public bool IsComplete { get; private set; }
        public int PassedCount { get; private set; }
        void OnEnable()
        {
            if(targets!=null)
            {
                foreach (var target in targets)
                {
                    if (target) 
                    {
                        target.onScored.AddListener(CheckProgress);
                        CheckProgress();
                    }
                }
            }
        }
        void OnDisable()
        {
            if(targets!=null)
            {
                foreach (var target in targets)
                if (target)
                {
                    target.onScored.RemoveListener(CheckProgress);
                }
            }
        }
        public void CheckProgress()
        {
            PassedCount = 0;

            foreach (var target in targets)
            {
                if (target.IsPassed)
                {
                    PassedCount++;
                }
            }

            bool complete = PassedCount == targets.Length;

            if (progressLabel)
                progressLabel.text = complete
                    ? "CHALLENGE COMPLETE"
                    : $"OUTFIELD CHALLENGE\n{PassedCount} / {targets.Length} TARGETS";

            if (complete && !IsComplete)
            {
                IsComplete = true;
                onChallengeCompleted.Invoke();
            }
        }
        public void ResetChallenge()
        {
            IsComplete=false;
            if (targets!=null)
            {
                foreach (var target in targets)
                {
                    if(target) 
                    {
                        target.ResetTarget();
                        CheckProgress();
                    }
                }
            }
        }
    }
}
