using System;

namespace WalkingForRochester.Models
{
    public class WalkModel
    {
        public int UserId { get; set; }
        public string NoteText { get; set; }
        public string PinData { get; set; }
        public string PickImage { get; set; }
        public double Distances { get; set; }
        public DateTime PickDate { get; set; }
        public int Collection { get; set; }
        public double WalkingTime { get; set; }
    }
}