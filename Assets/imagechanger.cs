using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imagechanger : MonoBehaviour
{
    Image Image1, Image2;
    Text TimeText, TimeText_day, TimeDisplay;
    GameObject Image1_go, Image2_go, TimeText_go, TimeText_day_go;

    TrashScript _TrashScript;

    [SerializeField]
    float transition = 1.5f;

    [System.Serializable]
    public class ImageStruct
    {
        public Sprite ImageName;
        public float ImageBrightness = 0.5f;
    }

    [SerializeField]
    public int Image_List_No = 0;
    [SerializeField]
    ImageStruct[] Image_List = new ImageStruct[100];
    [SerializeField]
    public int Image_List_Space_No = 0;
    [SerializeField]
    ImageStruct[] Image_List_Space = new ImageStruct[100];

    GameObject brightness_button_text;
    GameObject image_change_time_text;

    void Start()
    {
        _TrashScript = this.transform.Find("TrashList").GetComponent<TrashScript>();

        Image1_go = this.transform.Find("Image").gameObject; 
        Image2_go = this.transform.Find("Image2").gameObject;
        Image1 = Image1_go.GetComponent<Image>();
        Image2 = Image2_go.GetComponent<Image>();

        TimeText_go = this.transform.Find("TimeText").gameObject;
        TimeText_day_go = this.transform.Find("TimeText_day").gameObject;
        TimeText = TimeText_go.GetComponent<Text>();
        TimeText_day = TimeText_day_go.GetComponent<Text>();

        TimeDisplay = this.transform.Find("TimeDisplay").GetComponent<Text>();

        if(_TrashScript!=null)
            Debug.Log("_trashscrupt = "+_TrashScript);
        current_time = _TrashScript.image_time;

        Image_List_No = Image_List.Length;
        Image_List_Space_No = Image_List_Space.Length;

        brightness_button_text = this.transform.Find("TrashConfig2").Find("Brightness").Find("Text").gameObject;
        image_change_time_text = this.transform.Find("TrashConfig2").Find("Time").Find("Text").gameObject;

        brightness_button_text.GetComponent<Text>().text = "Brightness " + (_TrashScript.brightness_modifier * 4f).ToString("0");

        if (_TrashScript.image_time == 0) image_change_time_text.GetComponent<Text>().text = "Image Change OFF";
        else if (_TrashScript.image_time < 60) image_change_time_text.GetComponent<Text>().text = "Image Change " + ((float)_TrashScript.image_time).ToString("N1") + "S";
        else if (_TrashScript.image_time < 180) image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time / 60).ToString("N1") + "M";
        else if (_TrashScript.image_time < 3600) image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time / 60).ToString("0") + "M";
        else image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time / 3600).ToString("0") + "H";

        if (_TrashScript.surinamese) this.transform.Find("TrashConfig2").Find("ClockType").Find("Text").GetComponent<Text>().text = "Surinamese";
        else this.transform.Find("TrashConfig2").Find("ClockType").Find("Text").GetComponent<Text>().text = "Space";

    }


    float current_time = 0f;
    float transition_time = 0f;

    float current_brightness = 0f;
    float previous_brightness = 0f;


    public void modify_brightness()
    {
        _TrashScript.brightness_modifier += 0.25f;
        if (_TrashScript.brightness_modifier > 4f) _TrashScript.brightness_modifier -= 4f;
        brightness_button_text.GetComponent<Text>().text = "Brightness "+(_TrashScript.brightness_modifier * 4f).ToString("0");
    }
    
    public void change_time()
    {
        if ((_TrashScript.image_time >= 0f) && (_TrashScript.image_time < 1.5f))
        {
            _TrashScript.image_time += 1.5f;
            TimeDisplay.text = _TrashScript.image_time.ToString("N1") + " s";
        } else if ((_TrashScript.image_time >= 1.5f) && (_TrashScript.image_time < 3f))
        {
            _TrashScript.image_time += 0.5f;
            TimeDisplay.text = _TrashScript.image_time.ToString("N1") + " s";
        }
        else if ((_TrashScript.image_time >= 3f) && (_TrashScript.image_time < 5f))
        {
            _TrashScript.image_time += 1f;
            TimeDisplay.text = _TrashScript.image_time.ToString("N0") + " s";
        }
        else if ((_TrashScript.image_time >= 5f) && (_TrashScript.image_time < 30f))
        {
            _TrashScript.image_time += 5f;
            TimeDisplay.text = _TrashScript.image_time.ToString("N0") + " s";
        }
        else if ((_TrashScript.image_time >= 30f) && (_TrashScript.image_time < 60f))
        {
            _TrashScript.image_time += 10f;
            TimeDisplay.text = _TrashScript.image_time.ToString("N0") + " s";
        }
        else if ((_TrashScript.image_time >= 60f) && (_TrashScript.image_time < 120f))
        {
            _TrashScript.image_time += 30f;
            TimeDisplay.text = (_TrashScript.image_time/60f).ToString("N1") + " m";
        }
        else if ((_TrashScript.image_time >= 120f) && (_TrashScript.image_time < 300f))
        {
            _TrashScript.image_time += 60f;
            TimeDisplay.text = (_TrashScript.image_time / 60f).ToString("N0") + " m";
        }
        else if ((_TrashScript.image_time >= 300f))
        {
            _TrashScript.image_time += 180f;
            TimeDisplay.text = (_TrashScript.image_time / 60f).ToString("N0") + " m";
            if (_TrashScript.image_time > 1020f)
            {
                _TrashScript.image_time = 0f;
                TimeDisplay.text = _TrashScript.image_time.ToString("N1") + " s";
            }
        }

        if (_TrashScript.image_time == 0) image_change_time_text.GetComponent<Text>().text = "Image Change OFF";
        else if (_TrashScript.image_time < 60) image_change_time_text.GetComponent<Text>().text = "Image Change " + ((float)_TrashScript.image_time).ToString("N1") + "S";
        else if (_TrashScript.image_time < 180) image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time/60).ToString("N1") + "M";
        else if (_TrashScript.image_time < 3600) image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time/60).ToString("0") + "M";
        else image_change_time_text.GetComponent<Text>().text = "Image Change " + (_TrashScript.image_time/3600).ToString("0") + "H";

    }

    public void Jump_Image()
    {
        jump_image = true;
    }

    public void Switch_Suinamese()
    {
        _TrashScript.surinamese = !_TrashScript.surinamese;
        if (_TrashScript.surinamese) this.transform.Find("TrashConfig2").Find("ClockType").Find("Text").GetComponent<Text>().text = "Surinamese";
        else this.transform.Find("TrashConfig2").Find("ClockType").Find("Text").GetComponent<Text>().text = "Space";
        jump_image = true;
    }

    bool jump_image = false;
    int surinamese_displayed = -1;
    bool next_image_surinamese = true;
    void Update()
    {
        if ((surinamese_displayed < 1) && (_TrashScript.surinamese))
        {
            this.transform.Find("ImageFlag").gameObject.SetActive(true);
            this.transform.Find("SETTINGS").gameObject.SetActive(false);
            surinamese_displayed = 1;
        }
        if ((surinamese_displayed != 0) && (!_TrashScript.surinamese))
        {
            this.transform.Find("ImageFlag").gameObject.SetActive(false);
            this.transform.Find("SETTINGS").gameObject.SetActive(true);
            surinamese_displayed = 0;
        }
        DateTime dt = DateTime.Now;
        //Debug.Log(dt.ToString("HH:mm"));
        TimeText.text = dt.ToString("HH:mm");
        TimeText_day.text = dt.ToString("dd-MM-yyyy\nddd");

        if (((current_time > _TrashScript.image_time) && (_TrashScript.image_time > 0)) || (jump_image))
        {
            if (transition_time==0f)
            {
                Image2.sprite = Image1.sprite;
                Image2.color = new Color(1, 1, 1, current_brightness * _TrashScript.brightness_modifier);
                Image1.color = new Color(1, 1, 1, 0);
                if(_TrashScript.surinamese) Image1.sprite = Image_List[UnityEngine.Random.Range(0, Image_List_No)].ImageName;
                else Image1.sprite = Image_List_Space[UnityEngine.Random.Range(0, Image_List_Space_No)].ImageName;
                previous_brightness = current_brightness;
                if (_TrashScript.surinamese) current_brightness = Image_List[UnityEngine.Random.Range(0, Image_List_No)].ImageBrightness;
                else current_brightness = Image_List[UnityEngine.Random.Range(0, Image_List_Space_No)].ImageBrightness;
                transition_time += Time.deltaTime;
                if (_TrashScript.surinamese) next_image_surinamese = true;
                else next_image_surinamese = false;
            }
            else
            {
                float brightness_factor_1 = (transition_time / transition) * current_brightness;
                if (brightness_factor_1 > current_brightness) brightness_factor_1 = current_brightness;
                float brightness_factor_2 = (transition_time / transition) * previous_brightness;
                if (brightness_factor_2 > previous_brightness) brightness_factor_2 = previous_brightness;

                Image1.color = new Color(1, 1, 1, brightness_factor_1 * _TrashScript.brightness_modifier);
                Image2.color = new Color(1, 1, 1, (previous_brightness - brightness_factor_2) * _TrashScript.brightness_modifier);
                if (transition_time>transition)
                {
                    current_time = 0f;
                    transition_time = 0f;
                    if (next_image_surinamese && _TrashScript.surinamese) jump_image = false;
                    if (!next_image_surinamese && !_TrashScript.surinamese) jump_image = false;
                }
                else
                {
                    transition_time += Time.deltaTime;
                }
            }

        }
        else
        {
            Image1.color = new Color(1, 1, 1, current_brightness * _TrashScript.brightness_modifier);
            current_time += Time.deltaTime;
        }
    }


}
