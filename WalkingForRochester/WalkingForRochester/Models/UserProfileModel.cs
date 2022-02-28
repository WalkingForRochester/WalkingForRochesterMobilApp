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
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Uri ImgUrl { get; set; }
        public double Distance { get; set; }
        public double TotalDistance { get; set; }
        public double Duration { get; set; }
        public double TotalDuration { get; set; }
        public int UserId { get; set; }
    }
}