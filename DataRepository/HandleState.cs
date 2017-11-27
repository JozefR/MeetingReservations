using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public class HandleState
    {
        public static bool SecondLoading { get; private set; }
        public static bool DataChanged { get; private set; }

        public static void  FirstLoading()
        {
            SecondLoading = true;
        }
        /// <summary>
        /// Data was changed ask if you want save all changes
        /// </summary>
        public static void ChangingData()
        {
            DataChanged = true;
        }
        public static void ChangingDataToFalse()
        {
            DataChanged = false;
        }


    }
}
