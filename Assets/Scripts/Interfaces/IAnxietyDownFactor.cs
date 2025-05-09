/// <summary>
/// Used to create factors that will decrease the anxiety.
/// They can be active (a gameplay mechanic) or passive (an environmental factor).
/// </summary>
public interface IAnxietyDownFactor
{
    /// <summary>
    /// Method that dictates how much anxiety is take from the current anxiety level of <see cref="AnxietyManager.Instance.currentAnxiety"/>
    /// </summary>
    /// <returns> The current decrese of anxiety. </returns>
    float AnxietyDecrease();
}