using System;
using Godot;

public partial class ChargeBall : Node2D
{


    private Vector2 _chargeDirection = Vector2.Zero;

    private Node2D ball1;
    private Node2D ball2;
    private Node2D ball3;
    private Node2D ball4;

    public override void _Ready()
    {
        ball1 = GetNode<Node2D>("1");
        ball2 = GetNode<Node2D>("2");
        ball3 = GetNode<Node2D>("3");
        ball4 = GetNode<Node2D>("4");

        ball1.Visible = false;
        ball2.Visible = false;
        ball3.Visible = false;
        ball4.Visible = false;

        ball2.Position = new Vector2(100,100);


    }
    public void SetChargeDirection(Vector2 direction, float chargeStrength, int Id)
    {

        Vector2 inverseDirection = direction * -1;

        GD.Print("Charge set for ID ", Id);
        if(Id == 1){
            ball1.Position = inverseDirection  * chargeStrength * 100;
            ball1.Visible = true;
            ball1.Rotation = direction.Angle(); // Angle in radians


        }
        else if(Id == 2){
            ball2.Position = inverseDirection  * chargeStrength * 100;
            ball2.Visible = true;
            ball2.Rotation = direction.Angle(); // Angle in radians


            }
        else if(Id == 3){
            ball3.Position = inverseDirection * chargeStrength * 100;
            ball3.Visible = true;    
            ball3.Rotation = direction.Angle(); // Angle in radians

  
            }
        else if(Id == 4 || Id == 5){
            ball4.Position = inverseDirection  * chargeStrength * 100;
            ball4.Visible = true;       
            ball4.Rotation = direction.Angle(); // Angle in radians

            }
    }
    public override void _Process(double delta)
    {

        
    }

    public void ResetPosition(int Id)
    {
        if(Id == 1){
            ball1.Visible = false;
        }
        else if(Id == 2){
            ball2.Visible = false;        
            }
        else if(Id == 3){
            ball3.Visible = false;        
            }
        else if(Id == 4 || Id == 5){
            ball4.Visible = false;
            }
    }

    public void SetVisible(bool setVisible, int Id){

        if(Id == 0){
            ball1.Visible = setVisible;
        }
        else if(Id == 1){
            ball2.Visible = setVisible;
        }
        else if(Id == 2){
            ball3.Visible = setVisible;
        }
        else if(Id == 3 || Id == 5){
            ball4.Visible = setVisible;
        }
    }

    public void SetStartingPosition(Vector2 positon, int Id){



    }
}
