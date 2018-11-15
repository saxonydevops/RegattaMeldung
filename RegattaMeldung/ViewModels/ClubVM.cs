using RegattaMeldung.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.ViewModels
{
    public class ClubVM
    {
        public int ClubId {get;set;}
        public int RegattaId {get;set;}
        public string Name {get;set;}
        [EmailAddress]
        public string EMail {get;set;}
    }
}