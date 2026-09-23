using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoom.LabSecurity
{
    public class LabDisplayLock : MonoBehaviour
    {
        [SerializeField] private string password = "0000";
        public GameObject transparentCover;
        public LabAccessCard accessCard;
        public TMP_Text pinDisplay;
        public TMP_Text statusDisplay;
        public AudioSource audioSource;
        public AudioClip keySound;
        public AudioClip deniedSound;
        public AudioClip unlockedSound;

        private string input = "";
        private bool denied;
        public bool IsUnlocked { get; private set; }

        private void Awake()
        {
            IsUnlocked = false;
            transparentCover.SetActive(true);
            accessCard.SetLocked(true);
            RefreshDisplay();
        }

        public void PressKey(string key)
        {
            if (!enabled || IsUnlocked) return;
            if (key == "C")
            {
                input = "";
                denied = false;
                Play(keySound);
                RefreshDisplay();
                return;
            }
            if (key == "#")
            {
                Submit();
                return;
            }
            if (denied) { input = ""; denied = false; }
            if (input.Length < password.Length) input += key;
            Play(keySound);
            RefreshDisplay();
        }

        private void Submit()
        {
            if (input != password)
            {
                input = "";
                denied = true;
                Play(deniedSound);
                RefreshDisplay();
                return;
            }
            IsUnlocked = true;
            transparentCover.SetActive(false);
            accessCard.SetLocked(false);
            Play(unlockedSound);
            RefreshDisplay();
        }

        private void Play(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        private void RefreshDisplay()
        {
            if (pinDisplay != null)
            {
                pinDisplay.text = IsUnlocked ? "OPEN" : denied ? "RETRY" :
                    new string('*', input.Length).PadRight(password.Length, '_');
                pinDisplay.color = denied ? new Color(1f, .36f, .25f) :
                    IsUnlocked ? new Color(.32f, 1f, .63f) : new Color(.25f, .88f, 1f);
            }
            statusDisplay.text = IsUnlocked ? "ACCESS GRANTED" : denied ? "INCORRECT PIN" : "LOCKED / ENTER PIN";
        }
    }
}
