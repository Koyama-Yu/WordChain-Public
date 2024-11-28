using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{

    private Player _player;
    public Slider staminaSlider;


    void Start()
    {
        staminaSlider = GameObject.Find("StaminaBar").GetComponent<Slider>();
        staminaSlider.value = staminaSlider.maxValue;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        staminaSlider.value = _player.GetStamina();
    }

}
