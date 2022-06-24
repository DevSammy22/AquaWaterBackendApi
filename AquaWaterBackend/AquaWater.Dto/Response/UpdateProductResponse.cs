﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Response
{
    public class UpdateProductResponse
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public bool IsFeature { get; set; }
        public virtual ICollection<ProductGalleryUpdateResponse> ProductGallery { get; set; }
    }
}
