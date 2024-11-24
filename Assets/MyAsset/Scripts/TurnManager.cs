using UnityEngine;

public class TurnManager
{
    private int m_turnCount;
    public event System.Action OnTick;
    public TurnManager()
    {
        m_turnCount = 1;
    }

    public void Tick()
    {
        m_turnCount++;
        OnTick?.Invoke();
    }
}
