using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Save()
        {
            GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
        }

        private void Load()
        {
            GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
        }
    }
}
