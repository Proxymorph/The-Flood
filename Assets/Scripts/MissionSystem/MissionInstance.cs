/// <summary>
/// Represents an instance of a mission.
/// </summary>
public class MissionInstance
{
    /// <summary>
    /// The data for this mission.
    /// </summary>
    public MissionData missionData;

    /// <summary>
    /// The status of this mission.
    /// </summary>
    public MissionStatus status;

    /// <summary>
    /// The time this mission was started.
    /// </summary>
    public float startTime;

    /// <summary>
    /// The time elapsed since this mission was started.
    /// </summary>
    public float elapsedTime;

    /// <summary>
    /// Creates a new mission instance based on the given data.
    /// </summary>
    /// <param name="data"> The mission to be instanced data. </param>
    public MissionInstance(MissionData data)
    {
        missionData = data;
        status = MissionStatus.NotStarted;
        startTime = 0f;
        elapsedTime = 0f;
    }
}