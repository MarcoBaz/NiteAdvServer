﻿using NiteAdvServerCore.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteAdvServerCore.Entities
{
    public class UnmanagedEntity
    {
        [PrimaryKey(PrimaryKeyType.Autogenerated)]
        public int Id { get; set; }
    }
    public class Entity : UnmanagedEntity
    {

        public DateTime LastSyncDate { get; set; }
    }

    public class BaseEntity
    {

        [PrimaryKey(PrimaryKeyType.Calculated)]
        public int Id { get; set; }
    }
}
