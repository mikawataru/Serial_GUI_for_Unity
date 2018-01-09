using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UniRx;

public class Serial : MonoBehaviour
{

    public string portName;
    public int baurate;

    SerialPort serial;
    bool isLoop = true;
    Quaternion q;
    string message;
    float dt, degX, degY;

    void Start()
    {
        this.serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);
        try
        {
            this.serial.Open();
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        }
        catch (Exception e)
        {
            Debug.Log("can not open serial port");
        }
    }

    public void ReadData()
    {
        while (this.isLoop)
        {
            message = this.serial.ReadLine();
        }
    }
    
    void Update()
    {
        string[] data = message.Split(',');
        dt = float.Parse(data[0]);
        degX = float.Parse(data[1]);
        degY = float.Parse(data[2]);
        transform.rotation = Quaternion.Euler(degX, 0, -degY);
        Debug.Log("dt:" + dt + "\tX:" + degX + "\tY" + degY);
    }
    
    void OnDestroy()
    {
        this.isLoop = false;
        this.serial.Close();
    }
}