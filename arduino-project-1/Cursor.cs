using Godot;
using System;
using System.IO.Ports;

public partial class Cursor : CharacterBody2D
{
    SerialPort serialPort;
    bool hasHeardFromArduino = false;
    float timer;

    public const float Speed = 300.0f;
    public const float JumpVelocity = -525.0f;
    public bool Ladder = false;
    public bool NearLadder = false;

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
        Vector2 velocity = Velocity;

        // 1. Apply gravity if in the air (And NOT climbing a ladder)
        if (!IsOnFloor() && !Ladder)
        {
            velocity += GetGravity() * (float)delta;
        }

        // 2. Reset HORIZONTAL velocity (Keep Y so gravity/jumping works)
        velocity.X = 0;

        // If on a ladder and not actively moving up/down, stop falling
        if (Ladder)
        {
            velocity.Y = 0;
        }


        // 3. Read serial input
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    string serialMessage = serialPort.ReadLine().Trim();

                    // Horizontal Movement
                    if (serialMessage == "LEFT")
                    {
                        velocity.X = -Speed;
                    }
                    else if (serialMessage == "RIGHT")
                    {
                        velocity.X = Speed;
                    }
                    
                    if (NearLadder)
                    {
                        if (serialMessage == "UP")
                        {
                            Ladder=true;
                        }
                        else if (serialMessage == "DOWN")
                        {
                            Ladder=true;
                        }
                    }

                    // Ladder Movement
                    if (Ladder && NearLadder) 
                    {
                        velocity.Y = 0;
                        CollisionLayer=2;
                        CollisionMask=2;
                        if (serialMessage == "UP")
                        {
                            velocity.Y = -Speed;
                        }
                        else if (serialMessage == "DOWN")
                        {
                            velocity.Y = Speed;
                        }
                    }
                    else
                    {
                        Ladder=false;
                        CollisionLayer=1;
                        CollisionMask=1;
                    }
                    
                    // Jumping (Only allowed if on the floor and not on a ladder)
                    if (serialMessage == "ACTIVE" && IsOnFloor() && !Ladder)
                    {
                        velocity.Y = JumpVelocity;
                    }
                }
            }
            catch (TimeoutException)
            {
                // Timeout is fine, just ignore it
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void DestroyActor()
    {
        serialPort.Write("2");
        serialPort.Close();
        GetTree().ChangeSceneToFile("res://TITLE.tscn");

    }

    public void WinGame()
    {
        hasHeardFromArduino = false;
        serialPort.Write("1");
        serialPort.Close();
        GetTree().ChangeSceneToFile("res://TITLE.tscn");

    
    }
}
