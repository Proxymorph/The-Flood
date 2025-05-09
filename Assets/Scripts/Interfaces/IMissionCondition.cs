/// <summary>
/// An interface that represent an implementation of a conditional check.
/// This will return a boolean, that confirms when the custom condition/s is/are met.
/// </summary>
public interface IMissionCondition
{
    // Returns true if the condition/s is/are met.
    bool IsConditionMet();
}