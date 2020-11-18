using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using System.Text;

[CreateAssetMenu(fileName = "Option", menuName = "ScriptableObjects/Option", order = 1)]
public class Option : ScriptableObject
{
    
    public enum SettingType { INT,STRING,BOOL }
    
    public SettingType settingType;
    
    string optionTag;
    
    public string OptionTag
    {

        get {
            if (optionTag == "" || optionTag.Length == 0)
                {
                optionTag = this.name;
                }
            return optionTag;
        }

        set { 
            optionTag = value;
        }

    }

    string defaultValue;

    public string DefaultValue
    {
        get
        {

        }
        set
        {
            defaultValue = value;
        }
    }
    
    string optionValue;
    
    public string Value
    {
    
        get { 
        
            optionValue = Load();
            
            if (optionValue == "")
            {
                optionValue = DefaultValue;
                Save();
            }
            return optionValue;
        }

        set {
            optionValue = value;
            Save();
        }

    }

    public int GetIntValue()
    {
        return 0;
    }

    public bool GetBoolValue()
    {
        return false;
    }
    public string GetStringValue()
    {
        if (settingType == SettingType.STRING)
        {
            return Value;
        }
        else
            return DefaultValue;
    }

    string path;

    void OnEnable()
    {
        path = Application.persistentDataPath + "/gameData/settings/settings.txt";
        Debug.Log(OptionTag+" is set to "+Value);
    }

    string Load()
    {

        int counter = 0;
        string line;
        if (File.Exists(path))
        {
            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    string[] pair = line.Split('=');
                    if (pair[0] == OptionTag)
                    {
                        file.Close();
                        return pair[1];
                    }
                }
            }
            file.Close();
        }

        return DefaultValue;
    }

    void Save()
    {
        bool contains = false;
        string content = "";
        if(File.Exists(path))
        { 
            using (StreamReader sr = new StreamReader(@path))
                {
                int i = 0;
                do
                {
                    i++;
                    string line = sr.ReadLine();
                    if (line != "")
                    {
                        string tag = OptionTag;
                        if(line.Contains(tag+"="))
                        {
                            line = tag + "=" + optionValue;
                            contains = true;
                        }
                    }
                    content = content + line + Environment.NewLine;
                } while (sr.EndOfStream == false);
                sr.Close();
                }
        }
        if (!contains)
        {
        content = OptionTag + "=" + optionValue + Environment.NewLine;
        }
        if(File.Exists(path)) File.Delete(path);
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write(content);
            sw.Close();
        }

    }

}
