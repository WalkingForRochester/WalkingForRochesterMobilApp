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
    public class ResetPasswordModel
    {
        public string Request { get; set; } = "ResetPassword";
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}