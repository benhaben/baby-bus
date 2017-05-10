using System;
using System.Collections.Generic;

namespace BabyBus.Permission
{
    public class PermissionContainer
    {
        static PermissionContainer()
        {
            RolePermissionMap.Add(1
                , new List<string> { "Parent" });
            RolePermissionMap.Add(2
                , new List<string> { "Class","KgOrClass" });
            RolePermissionMap.Add(3
                , new List<string> { "KG","KgOrClass" });
        }

        public static void Setup(int roleType)
        {
            Permission = RolePermissionMap[roleType];
        }

        public static List<string> Permission { get; private set; }

        public static Dictionary<int, List<String>> RolePermissionMap = new Dictionary<int, List<string>>();
    }

}
