using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Dropbox.Api;
using System.Linq;

namespace ClassLibrary
{
    public class DropboxClass
    {
        public static string Token { get; set; } 

        public static string GetToken()
        {
            using (StreamReader reader = new StreamReader(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox\token.dat"))
            {
                Token = reader.ReadLine();
            }
            return Token;
        }




        public static async Task Download()
        {
            string RealFile = "";
            string folder = "/Apps/SMSBackupRestore/";

            GetToken();

            using (var dbx = new DropboxClient(Token))
            {
                var list = await dbx.Files.ListFolderAsync(folder);

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    RealFile = item.Name;
                }
            }

            using (var dbx = new DropboxClient(Token))
            {
                using (var response = await dbx.Files.DownloadAsync(folder + RealFile))
                {
                    var s = response.GetContentAsByteArrayAsync();
                    s.Wait();
                    var d = s.Result;

                    File.WriteAllBytes(RealFile, d);
                }
            }

        }



        public static void MoveFileFromMainFolder()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\totha\Source\Repos\iAboutMoneyGit\iAboutMoney\bin\Debug");
            FileInfo[] Files = d.GetFiles("*.xml");
            foreach (var item in Files)
            {
                if (item.Name.Contains("sms-"))
                {
                    try
                    {
                        File.Move(item.Name, @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox\" + item.Name);
                    }
                    catch (IOException)
                    {
                        File.Delete(item.Name);
                    }
                }
            }
        }
    }
}
