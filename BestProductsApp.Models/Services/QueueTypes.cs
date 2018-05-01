﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BestProductsApp.Models.Services
{
    public enum QueueTypes
    {
        [Description("start-app")]
        StartApp,
        [Description("updated-product")]
        UpdateProduct,
        [Description("delete-product")]
        DeleteProduct
    }
}
