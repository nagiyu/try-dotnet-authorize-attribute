using System;
using System.Collections.Generic;

namespace CommonAuthService
{
    public class UserManager
    {
        private static readonly Dictionary<string, string> users = new Dictionary<string, string>();
        private static readonly Dictionary<string, List<string>> userRoles = new Dictionary<string, List<string>>();

        // ユーザーを追加
        public static void AddUser(string username, string password)
        {
            if (!users.ContainsKey(username))
            {
                users.Add(username, password);
                userRoles.Add(username, new List<string>());
            }
        }

        // ユーザーが存在するかどうかをチェック
        public static bool UserExists(string username)
        {
            return users.ContainsKey(username);
        }

        // パスワードが正しいかどうかをチェック
        public static bool IsPasswordCorrect(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                return users[username] == password;
            }
            return false;
        }

        // ユーザーにロールを追加
        public static void AddRoleToUser(string username, string role)
        {
            if (users.ContainsKey(username) && !userRoles[username].Contains(role))
            {
                userRoles[username].Add(role);
            }
        }

        // 全てのユーザを取得
        public static List<string> GetAllUsers()
        {
            return new List<string>(users.Keys);
        }

        // ユーザをもとに登録されたロールを取得
        public static List<string> GetRolesForUser(string username)
        {
            if (users.ContainsKey(username))
            {
                return userRoles[username];
            }
            return new List<string>();
        }

        // ユーザーが指定されたロールに所属しているかどうかをチェック
        public static bool IsUserInRole(string username, string role)
        {
            if (users.ContainsKey(username))
            {
                return userRoles[username].Contains(role);
            }
            return false;
        }
    }

}
