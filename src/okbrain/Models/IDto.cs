using System;
using System.Linq;
using System.Web;

namespace okbrain.Models
{
    public interface IDto
    {
        Guid Id { get; set; }
    }

    public abstract class Dto : IDto
    {
        protected Dto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }

    //public class Member : Dto
    //{
    //    public string DisplayName { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string DefaultEmail { get; set; }
    //}
}