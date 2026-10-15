using Godot;
using System;
using System.IO.Ports;

public partial class player : CharacterBody2D
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

    [Export]
    public int Speed { get; set; } = 300;
    public int Health=10;
    public bool Ladder=false;


    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        if (serialPort == null || !serialPort.IsOpen)
        return;
        try
        {
            while (serialPort.BytesToRead > 0)
            {
                string serialMessage = serialPort.ReadLine().Trim();

                if (serialMessage == "UP")
                {
                    inputDirection.Y -= 1;
                }
                else if (serialMessage == "DOWN")
                {
                    inputDirection.Y += 1;
                }
                else if (serialMessage == "LEFT")
                {
                    inputDirection.X -= 1;
                }
                else if (serialMessage == "RIGHT")
                {
                    inputDirection.X += 1;
                }

                
            }
        }
        catch (TimeoutException)
        {
            // Ignore serial read timeouts
        }

        Velocity = inputDirection * Speed;
        MoveAndSlide();
    }
}
