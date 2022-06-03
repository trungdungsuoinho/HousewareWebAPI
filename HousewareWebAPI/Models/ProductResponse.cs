﻿using System;
using System.Collections.Generic;

namespace HousewareWebAPI.Models
{
    public class ProInGetCat
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public uint Price { get; set; }
        //public int Review { get; set; }
        //public float Rate { get; set; }
    }

    public class GetProductResponse
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; }
        public uint Price { get; set; }
        public uint Weight { get; set; }
        public uint Length { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint View { get; set; }
        public List<string> Highlights { get; set; }
        public List<GetSpecByPro> Specifications { get; set; }
    }

    public class GetProAdminResponse
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public List<string> Images { get; set; }
        public uint Price { get; set; }
        public uint Weight { get; set; }
        public uint Length { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint View { get; set; }
        public List<string> Highlights { get; set; }
        //public string Overview { get; set; }
        //public string Design { get; set; }
        //public string Performance { get; set; }
        public List<GetSpecByPro> Specifications { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Enable { get; set; }
    }
}
