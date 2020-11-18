using System;

namespace Drink4Burpee.Entities.Base
{
    public abstract class BaseBurpee
    {
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        private int _count = 1;
        public int Count 
        { 
            get
            {
                return _count;
            } 
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Count));
                }

                _count = value;
            }
        }
    }
}