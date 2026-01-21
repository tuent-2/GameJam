// using System;
// using System.Text;
// using JWT.Algorithms;
// using JWT.Builder;
// using Newtonsoft.Json.Linq;
//
// public static class JWTUtils
// {
//     public static string Decode(string input)
//     {
//         string base64 = input.Replace('-', '+').Replace('_', '/');
//         switch (input.Length % 4)
//         {
//             case 2: base64 += "=="; break;
//             case 3: base64 += "="; break;
//         }
//         var bytes = Convert.FromBase64String(base64);
//         return Encoding.UTF8.GetString(bytes);
//     }
// }