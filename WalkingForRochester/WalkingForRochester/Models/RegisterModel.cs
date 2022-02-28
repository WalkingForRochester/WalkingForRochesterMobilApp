using System;

namespace WalkingForRochester.Models
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
{
    public class RegisterModel
    {
        public string Request { get; set; } = "Reginster";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}