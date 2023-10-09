using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP 
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += i => deleteCharacterSlot = true;
                playerControls.UI.Delete.performed += i => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }

        public void DeleteCharacterButtonClicked()
        {
            deleteCharacterSlot = true;
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}