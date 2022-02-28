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

using System;

namespace WalkingForRochester.Models
{
    public class ArticleModel
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public string ImageName { get; set; }
        public string Content { get; set; }
    }
}