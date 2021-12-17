using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IData : MonoBehaviour
{
    #region PLAYER PREFS
    // PLAYER PREFS
    int currentLevel = 0;

    void SavePP() => PlayerPrefs.SetInt("currentLevel", currentLevel);

    void LoadPP() => currentLevel = PlayerPrefs.GetInt("currentLevel");

    void DeletePP(string s) => PlayerPrefs.DeleteKey(s);

    void DeleteAllPP() => PlayerPrefs.DeleteAll();
    #endregion

    #region JSON
    public Inventory inventory;
    void LoadJSON()
    {
        string data = PlayerPrefs.GetString("saveData");
        inventory = JsonUtility.FromJson<Inventory>(data);
    }

    void SaveJSON()
    {
        inventory.objects.Add("A");
        inventory.objects.Add("B");
        inventory.objects.Add("C");
        inventory.objects.Add("D");

        //{"objects":["A","B","C","D"]}
        string saveData = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString("saveData", saveData);
    }

    [System.Serializable]
    public class Inventory
    {
        public List<string> objects;
    }

    // EJ 2

    void SaveJSON2()
    {
        PlayerPos player = new PlayerPos(transform);
        string data = JsonUtility.ToJson(player);
        PlayerPrefs.SetString("data2", data);
    }

    void LoadJSON2()
    {
        string data = PlayerPrefs.GetString("data2");
        PlayerPos player = JsonUtility.FromJson<PlayerPos>(data);
        transform.SetPositionAndRotation(player.position, player.rotation);
    }

    [System.Serializable]
    public class PlayerPos
    {
        public Vector3 position;
        public Quaternion rotation;

        public PlayerPos(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
        }
    }
    #endregion
}
