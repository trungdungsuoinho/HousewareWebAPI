using HousewareWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousewareWebAPI.Test
{
    public class TestService
    {
        public static Reponse Test()
        {
            var reponse = new Reponse();
            int a = 0;
            if (a == 0)
            {
                throw new Exception("Oke chưa!");
            }
            return reponse;
        }
    }
}
