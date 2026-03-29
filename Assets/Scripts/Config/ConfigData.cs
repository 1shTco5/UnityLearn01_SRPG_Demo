using System.Collections.Generic;
using UnityEngine;

///<summary>
///读取csv格式数据表
///</summary>
public class ConfigData
{
    //将每个数据表中的数据存储到字典中
    //key: 字典ID
    //value: 每一行数据
    private Dictionary<int, Dictionary<string, string>> data;

    public string fileName; //配置表文件名

    public ConfigData(string fileName)
    {
        this.fileName = fileName;
        this.data = new();
    }

    public TextAsset LoadFile()
    {
        return Resources.Load<TextAsset>($"Data/{fileName}");
    }

    //读取
    public void Load(string text)
    {
        string[] dataArr = text.Split("\n"); //换行
        string[] titleArr = dataArr[0].Trim().Split(','); //逗号分割 获取第一行数据 (列名)
        //内容从第三行开始读取
        for (int i = 2; i < dataArr.Length; i++)
        {
            string[] row = dataArr[i].Trim().Split(',');
            Dictionary<string, string> tempData = new();
            for (int j = 0; j < titleArr.Length; j++)
            {
                tempData.Add(titleArr[j], row[j]);
            }
            data.Add(int.Parse(tempData["Id"]), tempData);
        }
    }

    public Dictionary<string, string> GetDataByID(int id)
    {
        if (data.ContainsKey(id))
        {
            return data[id];
        }
        return null;
    }

    public Dictionary<int, Dictionary<string, string>> GetLines()
    {
        return data;
    }
}
