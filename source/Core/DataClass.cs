using Source.Model;

namespace Source.Core
{
    static internal class DataClass
    {
        public enum FormEnum
        {
            None = 0,
            SZV_M = 1,
            SZV_TD = 2
        }

        public static diplomeEntities DataBase = null;
        public static User UserInfo = null;
        public static Insurer SelectedInsurer = null;
        public static User SelectedUser = null;
        public static Worker SelectedWorker = null;
        public static FormsSZV_TD CurrentFormsSZV_TD = null;
        public static FormsSZV_TD_Stuff CurrentFormsSZV_TD_Stuff = null;
        public static FormsSZV_M CurrentFormsSZV_M = null;
        public static FormsSZV_M_Stuff CurrentFormsSZV_M_Stuff = null;
    }
}
