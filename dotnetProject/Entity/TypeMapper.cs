using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class TypeMapper
    {
        public Dictionary<int?, int?> mapper = new Dictionary<int?,int?>();

        public TypeMapper()
        {
            mapper.Add(6, 1);
            mapper.Add(7, 2);
            mapper.Add(8, 2);
            mapper.Add(9, 1);
            mapper.Add(10, 3);
            mapper.Add(11, 1);
            mapper.Add(12, 1);
            mapper.Add(13, 1);
            mapper.Add(14, 1);
            mapper.Add(15, 1);
            mapper.Add(16, 3);
            mapper.Add(17, 1);
            mapper.Add(18, 1);
            mapper.Add(19, 1);
            mapper.Add(20, 1);
            mapper.Add(21, 1);
            mapper.Add(22, 2);
            mapper.Add(23, 3);
        }
    }
}
