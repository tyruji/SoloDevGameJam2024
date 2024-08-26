using Godot;
using System;

public partial class ScoreHandler : Control
{
    [Export]
    private string _MainMenuScenePath = null;

    [Export]
    private Player _Player = null;

    public int Score { get; private set; }

    private Label _scoreLabel = null;

    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>( "HBoxContainer/Score" );

        _Player ??= GetNode<Player>( "../../Player" );
        _Player.OnMoveLeft += IncreaseScore;
        _Player.OnMoveRight += IncreaseScore;
        _Player.OnHitWall += ShowGameOver;
    }

    private void IncreaseScore() 
    {
        ++Score;
        _scoreLabel.Text = Score.ToString();
    }

    private void ShowGameOver()
    {
        GetNode<Label>( "VBoxContainer/HBoxContainer/Distance" ).Text = Score.ToString();
        GetNode<VBoxContainer>( "VBoxContainer" ).Visible = true;
    }

    private void MainMenu()
    {
        GetTree().ChangeSceneToFile( _MainMenuScenePath );
    }
}
