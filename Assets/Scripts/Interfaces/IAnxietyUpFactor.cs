/// <summary>
/// Used to create factors that will increase the anxiety.
/// They can be active (a gameplay mechanic) or passive (an environmental factor).
/// </summary>
public interface IAnxietyUpFactor
{
    /// <summary>
    /// Method that dictates how much anxiety is added to the current anxiety level of <see cref="AnxietyManager.Instance.currentAnxiety"/>
    /// </summary>
    /// <returns> The current increase in anxiety. </returns>
    float AnxietyIncrease();
}