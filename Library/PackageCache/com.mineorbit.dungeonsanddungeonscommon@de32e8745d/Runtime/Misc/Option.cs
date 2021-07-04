using System;
using UnityEngine;


namespace com.mineorbit.dungeonsanddungeonscommon
{
    [CreateAssetMenu(fileName = "Option", menuName = "ScriptableObjects/Option", order = 1)]
    public class Option : ScriptableObject
    {
        public enum SettingType
        {
            INT,
            STRING,
            BOOL,
            FLOAT
        }

        //Visible Options: 
        public string defaultValue;

        public SettingType settingType;
        public string optionHandlerName;


        private object _defValue;

        public OptionHandler optionHandler;


        private string optionTag;

        private object optionValue;

        public string OptionTag
        {
            get
            {
                optionTag = name;
                return optionTag;
            }

            set => optionTag = value;
        }

        public object DefaultValue
        {
            get
            {
                if (defaultValue != string.Empty)
                {
                    if (settingType == SettingType.BOOL) _defValue = Convert.ToBoolean(defaultValue);
                    if (settingType == SettingType.INT) _defValue = Convert.ToInt32(defaultValue);
                    if (settingType == SettingType.STRING) _defValue = defaultValue;
                    if (settingType == SettingType.FLOAT) _defValue = Convert.ToSingle(defaultValue);
                }
                else
                {
                    if (settingType == SettingType.BOOL) _defValue = false;
                    if (settingType == SettingType.INT) _defValue = 0;
                    if (settingType == SettingType.STRING) _defValue = "default";
                    if (settingType == SettingType.FLOAT) _defValue = 0.0f;
                }

                return _defValue;
            }
        }

        public object Value
        {
            get
            {
                if (optionValue == null) optionValue = DefaultValue;
                return optionValue;
            }

            set
            {
                
                if (value != string.Empty)
                {
                    if (settingType == SettingType.BOOL) optionValue = Convert.ToBoolean(value);
                    if (settingType == SettingType.INT) optionValue = Convert.ToInt32(value);
                    if (settingType == SettingType.STRING) optionValue = value;
                    if (settingType == SettingType.FLOAT) optionValue = Convert.ToSingle(value);
                }
                else
                {
                    optionValue = DefaultValue;
                }

                if (optionHandler != null) optionHandler.OnValueChanged(optionValue);
                }
        }

        private void OnEnable()
        {
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
                return (string) Value;
            return (string) DefaultValue;
        }
    }
}