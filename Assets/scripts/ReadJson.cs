using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ReadJson : MonoBehaviour
{
    public static ReadJson Instance;

    RootRole rootRole = new RootRole();
    int level = 0;
    public int Level { get { return level; } }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        ReadMonsterValue();

        DontDestroyOnLoad(gameObject);
    }

    //讀monster數值
    void ReadMonsterValue()
    {
        //讀取resource底下的json檔案
        //注意這邊是使用.text
        string path = Application.dataPath + "/streamingAssets" + "/enemyValue.json";
        string info = File.ReadAllText(path);

        //這裡得到的資料是RootRole type 裡面是列出所有的monster value
        rootRole = JsonConvert.DeserializeObject<RootRole>(info);

    }

    //輸入index即可取monster數值
    public Role GetMonsterValue(int monsterIndex)
    {
        Role monster = rootRole.roles[monsterIndex - 1];
        return monster;
    }

    //讀取存擋紀錄
    public int GetSavedLevel()
    {
        //儲存檔案的路徑
        string path = Application.dataPath + "/streamingAssets/save.json";

        if (File.Exists(path))
        {
            string info = File.ReadAllText(path);
            SaveData levelData = JsonConvert.DeserializeObject<SaveData>(info);
            level = levelData.levelNumber;
            return level;
        }
        else { return 0; }
            

    }
}
