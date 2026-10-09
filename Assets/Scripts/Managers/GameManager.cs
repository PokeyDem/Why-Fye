using System;
using System.Collections.Generic;
using Systems.SaveSystem;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int amountOfStages;
        [SerializeField] private int amountOfLevels;
        [SerializeField] private List<StageLevelsData> stagesAndLevelsStateData;
    
        private bool _saveLoaded = false;
    
        private bool _isInitialized = false;
    
        public static GameManager Instance;
    
        private bool _loadedFromLevel = false;

        private bool _levelLoaded;

        private int _targetLevelToLoad;

        private int _targetStage;
    
        private bool _isTutorialActive;
    
        public static event Action OnLevelButtonsValidationRequest;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (!_saveLoaded)
            {
                List<StageLevelsData> loadedSave = SaveManager.Instance.LoadGameFromFile();
                if (loadedSave != null)
                {
                    _saveLoaded = true;
                    LoadUnlockedLevelsData(loadedSave);
                    OnLevelButtonsValidationRequest?.Invoke();
                }
                else
                {
                    Initialize();
                }
            }
        }

        private List<StageLevelsData> InitializeStagesAndLevels()
        {
            List<StageLevelsData> stagesAndLevels = new List<StageLevelsData>();
            for (int i = 0; i < amountOfStages; i++)
            {
                StageLevelsData currentStage = new StageLevelsData();
            
                currentStage.isStageUnlocked = i == 0;
                
                List<bool> unlockedLevels = new List<bool>();
                List<bool> completedLevels = new List<bool>();

                for (int j = 0; j < amountOfLevels; j++)
                {
                    unlockedLevels.Add(i == 0 && j == 0);
                    completedLevels.Add(false);
                }
                
                currentStage.unlockedLevels = unlockedLevels;
                currentStage.completedLevels = completedLevels;
                
                currentStage.stageNumber = i;
                stagesAndLevels.Add(currentStage);
            }
            Debug.Log("GameManager initialized");
            return stagesAndLevels;
        }

        private void Initialize()
        {

            if (_isInitialized)
                return;
        
            stagesAndLevelsStateData = InitializeStagesAndLevels();
        
            _isInitialized = true;
            OnLevelButtonsValidationRequest?.Invoke();
        }

        public void MarkAsCompleted(int stage, int level)
        {
            stagesAndLevelsStateData[stage].completedLevels[level] = true;

            if (level + 1 < amountOfLevels)
            {
                stagesAndLevelsStateData[stage].unlockedLevels[level + 1] = true;
                _targetLevelToLoad++;
            }
            else
            {
                MoveToNextStage();
            }
            SaveManager.Instance.SaveGameToFile();
        }

        public void SetLoadedFromLevel(bool loaded)
        {
            _loadedFromLevel = loaded;
        }

        public bool GetLoadedFromLevel()
        {
            return _loadedFromLevel;
        }

        public void SetTargetLevel(int index)
        {
            _targetLevelToLoad = index;
        }

        public int GetTargetLevel()
        {
            return _targetLevelToLoad;
        }

        private void MoveToNextStage()
        {
            if (_targetStage + 1 >= amountOfStages)
                return;
            
            _targetLevelToLoad = 1;
            _targetStage++;
            
            stagesAndLevelsStateData[_targetStage].unlockedLevels[0] = true;
            
            OnLevelButtonsValidationRequest?.Invoke();
        }

        public void SetTargetLevelStage(int stageIndex)
        {
            _targetStage = stageIndex;
        }

        public int GetTargetLevelStage()
        {
            return _targetStage;
        }

        public List<StageLevelsData> GetUnlockedLevelsData()
        {
            return stagesAndLevelsStateData;
        }

        private void LoadUnlockedLevelsData(List<StageLevelsData> unlockedLevels)
        {
            stagesAndLevelsStateData = unlockedLevels;
        }

        public int GetAmountOfLevels()
        {
            return amountOfLevels;
        }
    
        public void ResetCompletedLevels()
        {
            stagesAndLevelsStateData = InitializeStagesAndLevels();
            SaveManager.Instance.SaveGameToFile();
            OnLevelButtonsValidationRequest?.Invoke();
        }

        public bool IsSaveLoaded()
        {
            return _saveLoaded;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        public int GetRawLevelIndex()
        {
            return (_targetLevelToLoad - 1) + (_targetStage) * amountOfLevels;
        }

        public void SetTutorialActive(bool active)
        {
            _isTutorialActive = active;
        }

        public bool IsTutorialActive()
        {
            return _isTutorialActive;
        }
    }

    [Serializable]
    public struct StageLevelsData
    {
        public int stageNumber;
        public bool isStageUnlocked;
        public bool isStageCompleted;
        public List<bool> unlockedLevels;
        public List<bool> completedLevels;
    }
}