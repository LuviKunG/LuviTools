public class InputConductor
{
    public delegate bool InputReceived();
    public InputReceived onInputReceived;

    public bool IsHold { get; private set; }
    public bool IsPressed { get; private set; }
    public bool IsRelease { get; private set; }

    bool state;

    public InputConductor(InputReceived eventInput)
    {
        onInputReceived = eventInput;
    }

    public void Reset()
    {
        IsPressed = false;
        IsRelease = false;
    }

    public void Update()
    {
        IsHold = onInputReceived();
        if (IsHold != state)
        {
            if (IsHold && !state)
            {
                IsPressed = true;
                IsRelease = false;
            }
            if (!IsHold && state)
            {
                IsPressed = false;
                IsRelease = true;
            }
        }
        else
        {
            IsPressed = false;
            IsRelease = false;
        }
        state = IsHold;
    }
}