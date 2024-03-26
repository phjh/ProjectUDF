public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsWithStrikingDistance { get; set; }

    void SetAggroStatus(bool aggroStatus);
    void SetStrikingDistance(bool isWithStrikingDistance);
}
