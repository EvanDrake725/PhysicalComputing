using Godot;
using System;
using System.IO.Ports;

public partial class Title : Node2D
{
    SerialPort serialPort;
    bool hasHeardFromArduino = false;

    public override void _Ready()
    {
        serialPort = new SerialPort("COM6", 9600) 
        { 
            ReadTimeout = 10,
            DtrEnable = true, 
            RtsEnable = true 
        };
        
        serialPort.Open();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (serialPort == null || !serialPort.IsOpen)
        return;
        try
        {
            while (serialPort.BytesToRead > 0)
            {
                string serialMessage = serialPort.ReadLine().Trim();

                if (serialMessage == "ACTIVE")
                {
                    serialPort.Close();
                    GetTree().ChangeSceneToFile("res://P1Test.tscn");
                }
            }
        }
        catch (TimeoutException)
        {
            // Ignore serial read timeouts
        }
    }
}
