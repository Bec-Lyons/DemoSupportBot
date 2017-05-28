using Microsoft.Bot.Builder.FormFlow;
using System;

namespace formflow.FormFlow
{

    //TODO: Update with your form questions 
    //The questions are structured for the Form Bot using public properties on the class. 
    //You can see here the Prompt attribute is set and the question options can be complex types like String or enums.

    [Serializable]
    public class Enquiry
    {
        [Prompt("What's your name?")]
        public string Name { get; set; }
        [Prompt("What is your Employee ID?")]
        public string Company { get; set; }
        [Prompt("What is your Job Title?")]
        public string JobTitle { get; set; }
        [Prompt("What's the best number to contact you on?")]
        public string Phone { get; set; }
        [Prompt("Please Describe your support problem.")]
        public string HowCanWeHelp { get; set; }
        [Prompt("Would you like to lodge a ticket? {||}")]
        public bool SignMeUpToTheMailingList { get; set; }
        [Prompt("Do you know which service you require from us? {||}")]

        public Service ServiceRequired { get; set; }

        public enum Service
        {
            Email, Desktop, Windows, Printing, Other
        }

        public static IForm<Enquiry> BuildEnquiryForm()
        {
            return new FormBuilder<Enquiry>()
                .Field("SignMeUpToTheMailingList")
                .Field("ServiceRequired")
                .AddRemainingFields()
                .Build();
        }
    }


}