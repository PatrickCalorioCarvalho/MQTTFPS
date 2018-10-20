using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System;

[RequireComponent(typeof(AudioSource))]
public class Interuptor : MonoBehaviour {

    public Transform jogador;
    public AudioClip somBotao;
    public KeyCode teclaAcenderLuz = KeyCode.E;
    [Range(1,5)]
    public float distanciaMinima = 2;
    public bool LuzLigada = false;
    public string topico = "";
    [Space(15)]
    public GameObject objInterruptorON;
    public GameObject objInterruptorOFF;
    [Space(15)]
    public Light luz;
    public GameObject objLuzAcessa;
    public GameObject objLuzApagada;

    float distancia;
    AudioSource aud;

    private MqttClient client;
    void Awake () {
        client = new MqttClient(IPAddress.Parse("192.168.0.100"), 1883, false, null);
        client.Connect(Guid.NewGuid().ToString(), "pi", "raspberry");
        aud = GetComponent<AudioSource>();
        if(somBotao)
        {
            aud.clip = somBotao;
        }
        aud.playOnAwake = false;
        aud.loop = false;
        if (objLuzAcessa)
        {
            objLuzAcessa.SetActive(LuzLigada);
        }
        if (objLuzApagada)
        {
            objLuzApagada.SetActive(!LuzLigada);
        }
        if (luz)
        {
            luz.enabled = LuzLigada;
        }
        if (objInterruptorON)
        {
            objInterruptorON.SetActive(LuzLigada);
        }
        if (objInterruptorOFF)
        {
            objInterruptorOFF.SetActive(!LuzLigada);
        }
        if (LuzLigada)
        {
            client.Publish(topico, System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
        else
        {
            client.Publish(topico, System.Text.Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

    }
    void Update () {
		if(jogador)
        {
            distancia = Vector3.Distance(transform.position, jogador.transform.position);
            if(distancia < distanciaMinima)
            {
                if(Input.GetKeyDown(teclaAcenderLuz))
                {
                    LuzLigada = !LuzLigada;
                    if(LuzLigada)
                    {
                        client.Publish(topico, System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                    }else
                    {
                        client.Publish(topico, System.Text.Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
                    }
                    if(aud.clip != null)
                    {
                        aud.PlayOneShot(aud.clip);
                    }
                    if (objLuzAcessa)
                    {
                        objLuzAcessa.SetActive(LuzLigada);
                    }
                    if (objLuzApagada)
                    {
                        objLuzApagada.SetActive(!LuzLigada);
                    }
                    if (luz)
                    {
                        luz.enabled = LuzLigada;
                    }
                    if (objInterruptorON)
                    {
                        objInterruptorON.SetActive(LuzLigada);
                    }
                    if (objInterruptorOFF)
                    {
                        objInterruptorOFF.SetActive(!LuzLigada);
                    }
                }
            }
        }
	}
}
