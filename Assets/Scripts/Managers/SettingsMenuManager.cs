using Systems.SaveSystem;
using UnityEngine;

namespace Managers
{
    public class SettingsMenuManager : MonoBehaviour
    {
        [SerializeField] ActionConfirmationManager actionConfirmationManager;
    
        public void OnClearSaveDataButtonClick()
        {
            actionConfirmationManager.ProcessConfirmation(() =>
                {
                    SaveManager.Instance.ClearSaveData();
                    GameManager.Instance.ResetCompletedLevels();
                }
            );
        }
    }
}
