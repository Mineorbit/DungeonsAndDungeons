using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

public class OptionManager : MonoBehaviour
{

    static Dictionary<string,OptionHandler> optionHandlers;

    public static Dictionary<string,Option> options;

    static string optionFilePath;

    public void Start()
    {
        SetupOptions();
        optionFilePath = Application.persistentDataPath + "/gameData/settings/settings.txt";
        if(File.Exists(optionFilePath))
        {
            Load();
        }else
        {
            Save();
        }
    }


    void SetupOptions()
    {
        optionHandlers = new Dictionary<string, OptionHandler>();
        options = new Dictionary<string, Option>();
        UnityEngine.Object[] optionsLoaded = Resources.LoadAll("options", typeof(Option));
        Type[] types = Assembly.GetAssembly(typeof(OptionHandler)).GetTypes();
        
        foreach(UnityEngine.Object o in optionsLoaded)
        {
            Option opt = (Option)o;
            options.Add(opt.OptionTag, opt);
        }

        foreach (Type t in types)
        {
            if (t.BaseType == typeof(OptionHandler))
            {
                optionHandlers.Add(t.Name,Activator.CreateInstance(t) as OptionHandler);
            }
        }

        foreach(Option opt in options.Values)
        {
            if(opt.optionHandlerName!=String.Empty)
            {
                if(optionHandlers.ContainsKey(opt.optionHandlerName))
                opt.optionHandler = optionHandlers[opt.optionHandlerName];
            }
        }

    }


    public static void Load()
    {
        int counter = 0;
        string line;

        System.IO.StreamReader file = new System.IO.StreamReader(optionFilePath);
        while ((line = file.ReadLine()) != null)
        {
            string[] kV = line.Split('=');
            if(kV.Length==2)
            {
                string tag = kV[0];
                string value = kV[1];
                if(options.ContainsKey(tag))
                {
                    options[tag].Value = value;
                }
            }
            counter++;
        }

        file.Close();
        ApplyOptions();
    }

    static void ApplyOptions()
    {
        foreach(Option opt in options.Values)
        {
            opt.Value = opt.Value;
        }
    }

    public static void Save()
    {
        File.Delete(optionFilePath);
        StreamWriter writer = new StreamWriter(optionFilePath,true);
        foreach(Option o in options.Values)
        {
            writer.WriteLine(o.OptionTag+"="+o.Value);
        }
        writer.Close();
        ApplyOptions();
    }
}
