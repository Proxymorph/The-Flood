using UnityEngine;

/// <summary>
/// An abstract class to be used and extended (such as <see cref="PillBottleCondition"/>).
/// It gives the freedom to create custom mission logic, that is decoupled from the rest of the code.
/// It also extends the <see cref="IMissionCondition"/> interface to ensure that the derived classes implement the <see cref="IsConditionMet"/> method.
/// This returns a boolean value, indicating whether the condition is met.
/// </summary>
public abstract class BaseMissionCondition : ScriptableObject, IMissionCondition
{
    /// <summary>
    /// An abstract method that will be implemented in derived classes.
    /// Here we define the custom condition/s that needs to be met, in order for a mission to be passed.
    /// </summary>
    /// <returns>It returns <see cref="true"/> if every condition is met, and in turn lets the mission be completed. Or <see cref="false"/> for either failing or waiting to complete.</returns>
    public abstract bool IsConditionMet();
}