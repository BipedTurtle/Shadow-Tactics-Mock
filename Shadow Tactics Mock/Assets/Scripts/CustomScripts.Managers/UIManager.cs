using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnGUI()
        {
            this.ToggleAttackMarker();
            this.MarkerTracksCursor();
        }

        [SerializeField] private Image attackMarker;
        private bool toggleOn;
        private void ToggleAttackMarker()
        {
            if (Input.GetKeyDown(KeyCode.A))
                this.toggleOn = true;
            if (Input.GetMouseButtonDown(0))
                this.toggleOn = false;

            this.attackMarker.gameObject.SetActive(this.toggleOn);
        }

        private void MarkerTracksCursor()
        {
            if (this.attackMarker.isActiveAndEnabled)
                this.attackMarker.transform.position = Input.mousePosition;
        }
    }
}
