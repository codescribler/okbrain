using System;

namespace okbrain.Models
{
    public class Subscription : Dto
    {
        public string Email { get; set; }
        public DateTime SubscribedOn{ get; set; }
        public string Source { get; set; }
    }
}