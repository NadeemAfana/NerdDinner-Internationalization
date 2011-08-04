using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace NerdDinner.Models
{
    public class Dinner
    {
        [HiddenInput(DisplayValue = false)]
        public int DinnerID { get; set; }

        [Required(ErrorMessageResourceName="ModelTitleRequired", ErrorMessageResourceType=typeof(Resources.Resources))]
        [StringLength(50, ErrorMessageResourceName = "ModelTitleTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelTitle", ResourceType = typeof(Resources.Resources))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ModelEventDateRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelEventDate", ResourceType = typeof(Resources.Resources))]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessageResourceName = "ModelDescriptionRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(256, ErrorMessageResourceName = "ModelDescriptionTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.MultilineText)]
        [Display(Name = "ModelDescription", ResourceType = typeof(Resources.Resources))]
        public string Description { get; set; }

        [StringLength(20, ErrorMessageResourceName = "ModelHostedByTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelHostName", ResourceType = typeof(Resources.Resources))]
        public string HostedBy { get; set; }

        [Required(ErrorMessageResourceName = "ModelContactPhoneRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(20, ErrorMessageResourceName = "ModelContactPhoneTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelContactInfo", ResourceType = typeof(Resources.Resources))]
        public string ContactPhone { get; set; }

        [Required(ErrorMessageResourceName = "ModelAddressRequired", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(50, ErrorMessageResourceName = "ModelAdderssTooLong", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelAddress", ResourceType = typeof(Resources.Resources))]
        public string Address { get; set; }

        [UIHint("CountryDropDown")]
        [Display(Name = "ModelCountry", ResourceType = typeof(Resources.Resources))]
        public string Country { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double Latitude { get; set; }

        [HiddenInput(DisplayValue = false)]
        public double Longitude { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string HostedById { get; set; }

        public virtual ICollection<RSVP> RSVPs { get; set; }

        public bool IsHostedBy(string userName)
        {
            return String.Equals(HostedById ?? HostedBy, userName, StringComparison.Ordinal);
        }

        public bool IsUserRegistered(string userName)
        {
            return RSVPs.Any(r => r.AttendeeNameId == userName || (r.AttendeeNameId == null && r.AttendeeName == userName));
        }

        [UIHint("LocationDetail")]
        [NotMapped]
        public LocationDetail Location
        {
            get
            {
                return new LocationDetail() { Latitude = this.Latitude, Longitude = this.Longitude, Title = this.Title, Address = this.Address };
            }
            set
            {
                this.Latitude = value.Latitude;
                this.Longitude = value.Longitude;
                this.Title = value.Title;
                this.Address = value.Address;
            }
        }
    }

    public class LocationDetail
    {
        public double Latitude;
        public double Longitude;
        public string Title;
        public string Address;
    }
}