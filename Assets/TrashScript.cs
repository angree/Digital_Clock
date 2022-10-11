using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class TrashScript : MonoBehaviour
{
    public enum TrashType { Plastic = 0, Paper = 1, Glass = 2, Bio = 3, Mix = 4 };

    Text TrashFieldText;

    [Serializable]
    public class Trash_Struct
    {
        public int trash_date;
        public TrashType DateType;
    }

    public bool surinamese = false;
    
    public Trash_Struct[] trash_dates = new Trash_Struct[500];

    public int trash_dates_no = 500;

    public Trash_Struct_save loadedData;

    public float brightness_modifier = 1f;

    public float image_time = 60f;

    public void Load()
    {
        loadedData = DataSaver.loadData<Trash_Struct_save>("trash_days");
        if (loadedData == null)
        {
            Debug.Log("save not found. leaving default values");
        }
        else
        {
            for (int i = 0; i < 500; i++)
            {
                trash_dates[i].trash_date = loadedData.saveData[i].trash_date;
                trash_dates[i].DateType = loadedData.saveData[i].DateType;
            }
            image_time = loadedData.image_time;
            trash_dates_no = loadedData.trash_dates_no;
            brightness_modifier = loadedData.brightness;
            surinamese = loadedData.surinamese;
            Debug.Log("loading save");
        }

    }

    public Trash_Struct[] saveData2 = new Trash_Struct[500];
    [Serializable]
    public class Trash_Struct_save
    {
        public Trash_Struct[] saveData = new Trash_Struct[500];
        public int trash_dates_no = 500;
        public float brightness = 4;
        public float image_time = 60;
        public bool surinamese = true;
    }
    public Trash_Struct_save saveData3;

    public void Save()
    {
        for(int i = 0; i < 500; i++) {
            Debug.Log("i=" + trash_dates[i].trash_date);
            saveData3.saveData[i].trash_date = trash_dates[i].trash_date;
            saveData3.saveData[i].DateType = trash_dates[i].DateType;
        }
        saveData3.trash_dates_no = trash_dates_no;
        saveData3.brightness = brightness_modifier;
        saveData3.image_time= image_time;
        saveData3.surinamese = surinamese;
        DataSaver.saveData(saveData3, "trash_days");
    }

    private void Awake()
    {
        Load();

    }

    public DateTime trrrr;
    ConfigWindowManager _ConfigWindowManager;
    void Start()
    {
        _ConfigWindowManager = this.GetComponent<ConfigWindowManager>();
        TrashFieldText = this.GetComponent<Text>();
        //Debug.Log(dt.ToString("HH:mm"));
        //plastic

    }
    public void ChangeDateType(int number_in_db, int new_type)
    {
        TrashType type_temp = (TrashType)new_type;
        trash_dates[number_in_db].DateType = type_temp;
    }
    float current_frameskip = 1.4f;
    float frameskip = 1;
    void Update()
    {
        DateTime dt = DateTime.Now;
        current_frameskip += Time.deltaTime;
        if(current_frameskip >= frameskip)
        {
            current_frameskip = 0f;
            int[] lowest_seconds = new int[5];
            int[] lowest_position = new int[5];

            for (int i = 0; i <= 4; i++) lowest_position[i] = -1;
            //PLASTIC
            string plastic_string = "";
            int plastic_wait = 0;
            for (int i = 0; i < trash_dates_no; i++)
            {
                int this_trash_type = (int)trash_dates[i].DateType;
                DateTime TrashTime; // = DateTime.Parse(trash_dates[i].trash_date.ToString());
                if (DateTime.TryParseExact(trash_dates[i].trash_date.ToString(), "yyyyMMdd",
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.None, out TrashTime))
                {
                    Console.WriteLine(TrashTime);
                }
                int seconds_left = (int)(TrashTime - dt).TotalSeconds;
                if (((lowest_seconds[this_trash_type] == 0) && (seconds_left >= 60)) || ((seconds_left < lowest_seconds[this_trash_type]) && (seconds_left >= -60 * 60 * 23)))
                {
                    //Debug.LogWarning("seconds left for i=" + i + " => " + seconds_left);
                    lowest_seconds[this_trash_type] = seconds_left;
                    lowest_position[this_trash_type] = i;
                }
            }
            int[] position_in_order = new int[5];
            int sorting_done = 1;
            for (int i = 0; i <= 4; i++) { position_in_order[i] = i; }
            while (sorting_done > 0)
            {
                sorting_done = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (lowest_seconds[position_in_order[i]] > lowest_seconds[position_in_order[i + 1]])
                    {
                        int temp_i = position_in_order[i + 1];
                        position_in_order[i + 1] = position_in_order[i];
                        position_in_order[i] = temp_i;
                        sorting_done++;
                    }
                }
            }

            for (int i = 0; i <= 4; i++)
            {
                int type = position_in_order[i];
                if (lowest_position[type] > -1)
                {
                    plastic_wait = lowest_seconds[type];
                    if (type == 0) plastic_string += "<color=#ffff44>";
                    if (type == 1) plastic_string += "<color=#4444ff>";
                    if (type == 2) plastic_string += "<color=#44ff44>";
                    if (type == 3) plastic_string += "<color=#cc5500>";
                    if (type == 4) plastic_string += "<color=#000000>";
                    //DateTime TrashTime = DateTime.Parse(trash_dates[lowest_position[type]].trash_date.ToString());
                    DateTime TrashTime; // = DateTime.Parse(trash_dates[i].trash_date.ToString());
                    if (DateTime.TryParseExact(trash_dates[lowest_position[type]].trash_date.ToString(), "yyyyMMdd",
                                  CultureInfo.InvariantCulture,
                                  DateTimeStyles.None, out TrashTime))
                    {
                        Console.WriteLine(TrashTime);
                    }

                    int diff_yea = TrashTime.Year - dt.Year;
                    int diff_mon = TrashTime.Month - dt.Month;
                    int diff_day = TrashTime.DayOfYear - dt.DayOfYear;
                    if (diff_day < 0) { diff_day += 365; }
                    int diff_hou = TrashTime.Hour - dt.Hour;
                    int diff_min = TrashTime.Minute - dt.Minute;
                    //Debug.LogError("day = " + diff_day);
                    if (diff_day == 0) plastic_string += "today";
                    else if (diff_day == 1) plastic_string += "" + diff_day.ToString("N0") + " day";
                    else if (diff_day > 1) plastic_string += "" + diff_day.ToString("N0") + " days";
                    else if (diff_hou == 1) plastic_string += "" + diff_hou.ToString("N0") + " hour";
                    else if (diff_hou >= 1) plastic_string += "" + diff_hou.ToString("N0") + " hours";
                    if (type == 0) plastic_string += " PLASTIC (";
                    if (type == 1) plastic_string += " PAPER (";
                    if (type == 2) plastic_string += " GLASS (";
                    if (type == 3) plastic_string += " BIO (";
                    if (type == 4) plastic_string += " MIX (";
                    plastic_string += TrashTime.Day + "." + TrashTime.Month + ")</color>\r\n";
                }
            }
            TrashFieldText.text = plastic_string;
            //Debug.LogError(plastic_string);
            //dt.ToString("HH:mm");
            //TimeText_day.text = dt.ToString("dd-MM-yyyy\nddd");
        }
    }
}
