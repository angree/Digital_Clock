using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class ConfigWindowManager : MonoBehaviour
{

    bool window_active = false;
    int no_pos_menu = 0;
    int last_no_pos_menu = -1;

    public enum TrashType { Plastic = 0, Paper = 1, Glass = 2, Bio = 3, Mix = 4 };

    [System.Serializable]
    public class Trash_Struct
    {
        public int trash_date;
        public TrashType DateType;
    }

    

    public void Save()
    {
        _TrashScript.Save();
        last_no_pos_menu = -1;
    }
    public void Load()
    {
        _TrashScript.Load();
        last_no_pos_menu = -1;
    }

    public void ChangeNumber()
    {
        int no_fields = int.Parse(fields_number.GetComponent<InputField>().text);
        _TrashScript.trash_dates_no = no_fields;
        scroll(0);
    }
    public void ToggleType(int button)
    {
        int date_type = (int)_TrashScript.trash_dates[button + no_pos_menu].DateType;
        date_type++; if (date_type > 4) date_type = 0;
        _TrashScript.ChangeDateType(button + no_pos_menu, date_type);
        //_TrashScript.trash_dates[button + no_pos_menu].ChangeDateType(button + no_pos_menu, date_type);
        last_no_pos_menu = -1;
    }

    public void ChangeDate(int button)
    {
        _TrashScript.trash_dates[button + no_pos_menu].trash_date = int.Parse(row_date[button].GetComponent<InputField>().text);
        last_no_pos_menu = -1;
    }

    public void scroll(int direction)
    {
        no_pos_menu += direction;
        if (no_pos_menu > _TrashScript.trash_dates_no - 17) no_pos_menu = _TrashScript.trash_dates_no - 17;
        if (no_pos_menu < 0) no_pos_menu = 0;
        last_no_pos_menu = -1;
    }

    public void ShowConfig()
    {
        if (ConfigWindow != null)
        {
            ConfigWindow.SetActive(true);
            window_active = true;
        }
    }

    public void HideConfig()
    {
        if (ConfigWindow != null)
        {
            ConfigWindow.SetActive(false);
            window_active = false;
        }
    }
    TrashScript _TrashScript;
    public GameObject ConfigWindow;
    GameObject TrashList_obj;
    GameObject[] row_number = new GameObject[17];
    GameObject[] row_date = new GameObject[17];
    GameObject[] row_button = new GameObject[17];
    GameObject[] row_button_text = new GameObject[17];
    GameObject fields_number;


    void Start()
    {
        ConfigWindow = this.transform.Find("TrashConfig2").gameObject;
        if (ConfigWindow != null) { 
            for(int i  = 0; i < 17; i++)
            {
                //Debug.Log("out = "+ "Number_0 (" + i.ToString("N0") + ")");
                row_number[i] = ConfigWindow.transform.Find("Number_0 (" + i.ToString("N0") + ")").gameObject;
                //Debug.Log(""+ row_number[i]);
                row_date[i] = row_number[i].transform.Find("Date").gameObject;
                row_button[i] = row_number[i].transform.Find("Type").gameObject;
                row_button_text[i] = row_number[i].transform.Find("Type").transform.Find("Text (Legacy)").gameObject;
                row_number[i] = row_number[i].transform.Find("Text").gameObject;
            }
            fields_number = ConfigWindow.transform.Find("Date").gameObject;
        }
        TrashList_obj = this.transform.Find("TrashList").gameObject;
        if (TrashList_obj != null) _TrashScript = TrashList_obj.GetComponent<TrashScript>();
    }

    bool first_run = true;
    void Update()
    {
        if (first_run) { ConfigWindow.SetActive(false); first_run = false; }
        if (window_active)
        {
            if(no_pos_menu!=last_no_pos_menu)
            {
                for (int i = 0; i < 17; i++)
                {
                    row_number[i].GetComponent<Text>().text = (i + no_pos_menu + 1).ToString("0");
                    string date_out = _TrashScript.trash_dates[i+no_pos_menu].trash_date.ToString("0");
                    //Debug.Log("date_out = "+date_out);
                    row_date[i].GetComponent<InputField>().text = date_out;
                    //(_TrashScript.TrashType)
                    if (int.Parse(date_out) == 0) row_number[i].GetComponent<Text>().color = new Color(0.2f, 0.4f, 1f, 0.5f);
                    else  row_number[i].GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.9f);
                    int date_type = (int)_TrashScript.trash_dates[i + no_pos_menu].DateType;
                    if(date_type==0) {
                        row_button[i].GetComponent<Image>().color = new Color(1f, 1f, 0.22f, .8f);
                        row_button_text[i].GetComponent<Text>().text = "Plastic";
                        row_button_text[i].GetComponent<Text>().color = new Color(0, 0, 0, 1);
                    }
                    if (date_type == 1)
                    {
                        row_button[i].GetComponent<Image>().color = new Color(0.22f, 0.22f, 1f, .8f);
                        row_button_text[i].GetComponent<Text>().text = "Paper";
                        row_button_text[i].GetComponent<Text>().color = new Color(0, 0, 0, 1);
                    }
                    if (date_type == 2)
                    {
                        row_button[i].GetComponent<Image>().color = new Color(0.22f, 1f, 0.22f, .8f);
                        row_button_text[i].GetComponent<Text>().text = "Glass";
                        row_button_text[i].GetComponent<Text>().color = new Color(0, 0, 0, 1);
                    }
                    if (date_type == 3)
                    {
                        row_button[i].GetComponent<Image>().color = new Color(0.85f, 0.35f, 0f, .8f);
                        row_button_text[i].GetComponent<Text>().text = "Bio";
                        row_button_text[i].GetComponent<Text>().color = new Color(1, 1, 1, 1);
                    }
                    if (date_type == 4)
                    {
                        row_button[i].GetComponent<Image>().color = new Color(0, 0, 0, .8f);
                        row_button_text[i].GetComponent<Text>().text = "Mix";
                        row_button_text[i].GetComponent<Text>().color = new Color(1, 1, 1, 1);
                    }
                }
                fields_number.GetComponent<InputField>().text = _TrashScript.trash_dates_no.ToString("0");
            }
            last_no_pos_menu = no_pos_menu;
        }
    }
}
