﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NerdDinner.Models;
using System.Web.Mvc;

namespace NerdDinner.Tests.Fakes
{
    class FakeDinnerData
    {
        public static List<Dinner> CreateTestDinners()
        {

            List<Dinner> dinners = new List<Dinner>();

            for (int i = 1; i <= 101; i++)
            {

                Dinner sampleDinner = new Dinner()
                {
                    DinnerID = i,
                    Title = "Sample Dinner",
                    HostedBy = "SomeUser",
                    Address = "Some Address",
                    Country = "USA",
                    ContactPhone = "425-555-1212",
                    Description = "Some description",
                    EventDate = DateTime.Now.AddDays(i),
                    Latitude = 99,
                    Longitude = -99,
                    RSVPs = new List<RSVP>()
                };

                RSVP rsvp = new RSVP();
                rsvp.RsvpID = i;
                rsvp.DinnerID = sampleDinner.DinnerID;
                rsvp.AttendeeName = "SomeUser";
                sampleDinner.RSVPs.Add(rsvp);

                dinners.Add(sampleDinner);
            }

            return dinners;
        }

        public static Dinner CreateDinner()
        {
            Dinner dinner = new Dinner();
            dinner.Title = "New Test Dinner";
            dinner.EventDate = DateTime.Now.AddDays(7);
            dinner.Address = "5 Main Street";
            dinner.Description = "Desc";
            dinner.ContactPhone = "503-555-1212";
            dinner.HostedBy = "scottgu";
            dinner.Latitude = 45;
            dinner.Longitude = 45;
            dinner.Country = "USA";
            return dinner;
        }

        public static FormCollection CreateDinnerFormCollection()
        {
            var form = new FormCollection();

            form.Add("Description", "Description");
            form.Add("Title", "New Test Dinner");
            form.Add("EventDate", "2010-02-14");
            form.Add("Address", "5 Main Street");
            form.Add("ContactPhone", "503-555-1212");
            return form;
        }

    }
}
