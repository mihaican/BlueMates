using BlueMates.Models;

namespace BlueMates.Controllers
{
    public class EventsOfInterestResults
    {
        public EventsOfInterestResults(Event e, int interestLevel, bool validated)
        {
            Event = e;
            InterestLevel = interestLevel;
            Validated = validated;
        }

        public Event Event { get; set; }
        public int InterestLevel { get; set; }
        public bool Validated { get; set; }
    }
}
