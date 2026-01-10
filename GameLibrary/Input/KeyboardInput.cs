using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input;

public class KeyboardInput
{
    public KeyboardInput()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }

    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    public bool IsKeyReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }
}
