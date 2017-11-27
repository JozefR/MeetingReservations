namespace MeetingLibrary
{
    public abstract class MeetingTower
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        abstract public string GetFullName { get; }
    }
}
