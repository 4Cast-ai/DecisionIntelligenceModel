﻿using System;
using System.Collections.Generic;

namespace Infrastructure.DataContext
{
    public class UnitOfWorkPoolOptions
    {
        public Dictionary<string, Type> RegisteredUoWs { get; set; } = new Dictionary<string, Type>();
    }
}
