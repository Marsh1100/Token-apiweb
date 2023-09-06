using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers;

    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; } //Quien lo emite

        public string Audience { get; set; } //A quién va dirigido

        public double DurationInMinutes { get; set; } //Sesión, que tanto se va a tener activa :o


    }
