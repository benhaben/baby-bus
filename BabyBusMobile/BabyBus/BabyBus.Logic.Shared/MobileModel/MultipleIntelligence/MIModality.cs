using System;

namespace BabyBus.Logic.Shared
{
    public class MIModality
    {
        public int TotalTest
        {
            get;
            set;
        }

        public MIModality()
        {
        }

        public int ModalityId{ get; set; }

        public int Total{ get; set; }

        public int Completed{ get; set; }

        public string  ModalityName{ get; set; }

        public int ModalityImageId{ get; set; }
    }
}

