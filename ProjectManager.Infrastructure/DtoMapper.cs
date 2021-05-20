using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure
{
    public static class DtoMapper<Dto, Entity> 
        where Dto : class 
        where Entity : class
    {
        public static Entity SetProperties(Dto dto, Entity entity)
        {
            var props = dto.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(dto);
                if (value != null)
                {
                    entity.GetType().GetProperty(prop.Name).SetValue(entity, value);
                }
            }
            return entity;
        }
    }
}
