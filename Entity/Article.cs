using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Article : Entry
    {
        private string _title;
        [Required]
        [MaxLength(200)]
        public string Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                this.LegalTitle = FilterTitle();
            }
        }

        public string LegalTitle { get; private set; }

        public string FilterTitle()
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            string escapedInvalidChars = string.Format(
                   "[{0}]",
                   Regex.Escape(new string(invalidChars)));
            var options = RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant;
            Regex regex = new Regex(escapedInvalidChars, options);
            var replacedWithSpace = regex.Replace(this._title, " ");
            
            regex = new Regex(@"\s+", options);
            var replacedWithSlash = regex.Replace(replacedWithSpace, "-");
            return replacedWithSlash;
        }
    }
}
