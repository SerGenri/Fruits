using System.Collections;

namespace Fruits.Domain.Base
{
   
    public class Base : BasePropertyChanged, IEnumerable
    {
        // Реализуем интерфейс IEnumerable
        public virtual IEnumerator GetEnumerator()
        {
            yield return this;
        }
       
    }
}
