namespace Solution.Domain
{
    public record GroupId(uint Value)
    {
        public static implicit operator uint(GroupId groupId) => groupId.Value;
        public static implicit operator GroupId(uint value) => new(value);
    }
}