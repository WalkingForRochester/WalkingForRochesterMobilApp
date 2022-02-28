using System;

/// <summary>
/// Authors: 
///         NTID MAD TEAM
///             Mangers:        Xiangyu Shi
///                             Michelle Olson
///                         
///             Programmer:     Xiangyu Shi, 
///                             Michelle Olson, 
///                             Zhencheng Chen,
///                             Harsh Mathur,
///                             Quoc Nhan,
///                             Chase Roth  
/// </summary>

namespace WalkingForRochester.Models
{
    public class LeaderboardModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Uri ImgUrl { get; set; }
        public string Nickname { get; set; }
        public int Collection { get; set; }
        public DateTime Date { get; set; }
        public double Distances { get; set; }
        public double Duration { get; set; }
        public string DurationFormat { get; set; }
        public bool CollectionVisible { get; set; } = true;
        public bool DistancesVisible { get; set; }
        public bool DurationVisible { get; set; }
    }
}