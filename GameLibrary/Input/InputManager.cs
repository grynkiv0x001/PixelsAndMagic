namespace GameLibrary.Input;

public class InputManager
{
    public InputManager()
    {
        Keyboard = new KeyboardInput();
        Mouse = new MouseInput();
    }

    public KeyboardInput Keyboard { get; }
    public MouseInput Mouse { get; }

    // TODO: Implement GamePad input

    public void Update()
    {
        Keyboard.Update();
        Mouse.Update();
    }
}
