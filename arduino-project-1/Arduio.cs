using Godot;
using System;
using System.IO.Ports;

public partial class Arduio : Node2D
{
    SerialPort serialPort;
    RichTextLabel text;
    bool hasHeardFromArduino = false;
    float timer;

    public override void _Ready()
    {
        // BUG FIX: Added quotes around node path and fixed syntax
        text = GetNode<RichTextLabel>("RichTextLabel"); 
        
        // BUG FIX: Added quotes around COM port and increased timeout
        serialPort = new SerialPort("COM6", 9600) 
        { 
            ReadTimeout = 10, // 1ms is too short for a reliable line read
            DtrEnable = true, 
            RtsEnable = true 
        };
        
        serialPort.Open();
    }

    public override void _Process(double delta){
    if (serialPort == null || !serialPort.IsOpen)
        return;

    // Read incoming serial messages
    try
    {
        while (serialPort.BytesToRead > 0)
        {
            string serialMessage = serialPort.ReadLine().Trim();

            if (serialMessage == "SupGodot")
            {
                text.Text = "Hello Controller";
                hasHeardFromArduino = true;
                timer = Time.GetTicksMsec();
            }
            else if (serialMessage == "ByeGodot")
            {
                text.Text = "Bye Controller";
                hasHeardFromArduino = true;
                timer = Time.GetTicksMsec();
            }
            
        }
    }
    catch (TimeoutException)
    {
        // Ignore serial read timeouts
    }

    // Check the timer every frame, even when no serial data arrives
    if (hasHeardFromArduino &&
        Time.GetTicksMsec() - timer >= 3000)
    {
        text.Text += "\nTurning on light";
        serialPort.Write("1");
        hasHeardFromArduino = false;
    }
}
}