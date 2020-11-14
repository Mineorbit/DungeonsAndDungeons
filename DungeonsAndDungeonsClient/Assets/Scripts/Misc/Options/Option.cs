using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "Option", menuName = "ScriptableObjects/Option", order = 1)]
public class Option : ScriptableObject
{
    
    public enum SettingType { INT,STRING,BOOL }
    
    public SettingType settingType;
    
    string optionTag;
    
    public string OptionTag
    {

        get {
            if (optionTag == "")
                optionTag = this.name;
            return optionTag;
        }

        set { 
            optionTag = value;
        }

    }

    public string defaultValue;
    
    string optionValue;
    
    public string Value
    {
    
        get { 
        
            optionValue = Load();
            
            if (optionValue == "")
            {
                optionValue = defaultValue;
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
            return defaultValue;
    }

    string Load()
    {
        return defaultValue;
    }

    void Save()
    {

    }

}
