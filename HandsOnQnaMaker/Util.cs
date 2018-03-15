using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandsOnQnaMaker
{
    public static class Util
    {
        public static int RetornaPorteCachorro(string raca)
        {
            switch (raca)
            {
                case "Pinscher":
                case "Poodle":
                case "pug":
                case "Chihuahua":
                    return 1;
                case "Beagle":
                case "Bulldog":
                case "cocker":
                case "Labrador":
                    return 2;
                case "Pitbull":
                case "Husky":
                case "Pastor Alemao":
                case "Rottweiler":
                    return 3;

            }
            return 0;
        }
    }
}